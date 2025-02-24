using ByteBox.FileStore.Domain.BackgroundJobs;
using ByteBox.FileStore.Infrastructure.Data;
using ByteBox.FileStore.Infrastructure.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NexaWrap.SQS.NET.Interfaces;
using NexaWrap.SQS.NET.Models;

namespace ByteBox.FileStore.Application.BackgroundJobs;

public class DeleteTrashFilesJob : IDeleteTrashFilesJob
{
    private const int ExpiredInDays = 30;
    private const int BatchSize = 500;

    private readonly ApplicationDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SqsOptions _sqsOptions;
    private readonly IMessageSender _messageSender;
    private readonly ILogger<DeleteTrashFilesJob> _logger;

    public DeleteTrashFilesJob(
        ApplicationDbContext dbContext,
        IUnitOfWork unitOfWork,
        IOptions<SqsOptions> sqsOptions,
        IMessageSender messageSender,
        ILogger<DeleteTrashFilesJob> logger)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
        _sqsOptions = sqsOptions.Value;
        _messageSender = messageSender;
        _logger = logger;
    }

    public async Task ExecuteAsync()
    {
        try
        {
            var lastExpiredDate = DateTime.UtcNow.AddDays(-ExpiredInDays);

            var trashedFiles = await _dbContext.Files
                    .Where(f => f.TrashedAt <= lastExpiredDate)
                    .Take(BatchSize)
                    .OrderBy(f => f.TrashedAt)
                    .ToListAsync();

            var trashedFilesId = trashedFiles.Select(f => f.FileId).ToList();

            var filePermission = await _dbContext.FilePermissions
                .Where(fp => trashedFilesId.Contains(fp.FileId))
                .ToListAsync();

            _dbContext.FilePermissions.RemoveRange(filePermission);
            _dbContext.Files.RemoveRange(trashedFiles);
            
            await _unitOfWork.SaveChangesAsync();

            var folderIdsToRefresh = trashedFiles.Select(f => f.FolderId).Distinct().ToList();
            await SendRefreshFolderMessages(folderIdsToRefresh);

            _logger.LogInformation("{DeletedFileCount} expired files deleted from trash", trashedFiles.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while executing DeleteTrashFilesJob. Message: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    private async Task SendRefreshFolderMessages(List<Guid> folderIdsToRefresh)
    {
        var refreshFolderMessages = folderIdsToRefresh.Select(fileId => new RefreshFolderMessage
        {
            FolderId = fileId,
            CorrelationId = Guid.NewGuid().ToString()
        }).ToList();
        await _messageSender.SendMessagesAsync(_sqsOptions.SubscribedQueueName, refreshFolderMessages);
    }
}
