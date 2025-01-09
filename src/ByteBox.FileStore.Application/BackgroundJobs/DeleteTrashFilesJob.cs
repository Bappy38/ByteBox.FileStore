using ByteBox.FileStore.Domain.BackgroundJobs;
using ByteBox.FileStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ByteBox.FileStore.Application.BackgroundJobs;

public class DeleteTrashFilesJob : IDeleteTrashFilesJob
{
    private const int ExpiredInDays = 30;
    private const int BatchSize = 500;

    private readonly ApplicationDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteTrashFilesJob> _logger;

    public DeleteTrashFilesJob(
        ApplicationDbContext dbContext,
        IUnitOfWork unitOfWork,
        ILogger<DeleteTrashFilesJob> logger)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
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

            await PublishDeleteMessages(trashedFilesId);

            _logger.LogInformation("{DeletedFileCount} expired files deleted from trash", trashedFiles.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while executing DeleteTrashFilesJob. Message: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    // TODO:: Publish FileDeletedEvent which handler will resize the anchestor folders size
    private async Task PublishDeleteMessages(List<Guid> deletedFileIds)
    {
        ;
    }
}
