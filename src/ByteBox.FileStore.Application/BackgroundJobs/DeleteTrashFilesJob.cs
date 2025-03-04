using ByteBox.FileStore.Infrastructure.Data;
using ByteBox.FileStore.Infrastructure.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NexaWrap.SQS.NET.Interfaces;
using NexaWrap.SQS.NET.Models;

namespace ByteBox.FileStore.Application.BackgroundJobs;

public class DeleteTrashFilesJob : BackgroundService
{
    private const int JobRecurringIntervalInMinutes = 5;
    private const int ExpiredInDays = 30;
    private const int BatchSize = 500;

    private readonly IServiceProvider _serviceProvider;

    public DeleteTrashFilesJob(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await DeleteTrashFilesAsync();
            await Task.Delay(TimeSpan.FromMinutes(JobRecurringIntervalInMinutes), stoppingToken);
        }
    }

    private async Task DeleteTrashFilesAsync()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var sqsOptions = scope.ServiceProvider.GetRequiredService<IOptions<SqsOptions>>().Value;
            var messageSender = scope.ServiceProvider.GetRequiredService<IMessageSender>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<DeleteTrashFilesJob>>();

            try
            {
                var lastExpiredDate = DateTime.UtcNow.AddDays(-ExpiredInDays);

                var trashedFiles = await dbContext.Files
                        .Where(f => f.TrashedAt <= lastExpiredDate)
                        .Take(BatchSize)
                        .OrderBy(f => f.TrashedAt)
                        .ToListAsync();

                var trashedFilesId = trashedFiles.Select(f => f.FileId).ToList();

                var filePermission = await dbContext.FilePermissions
                    .Where(fp => trashedFilesId.Contains(fp.FileId))
                    .ToListAsync();

                dbContext.FilePermissions.RemoveRange(filePermission);
                dbContext.Files.RemoveRange(trashedFiles);

                await unitOfWork.SaveChangesAsync();

                var folderIdsToRefresh = trashedFiles.Select(f => f.FolderId).Distinct().ToList();
                var refreshFolderMessages = folderIdsToRefresh.Select(fileId => new RefreshFolderMessage
                {
                    FolderId = fileId,
                    CorrelationId = Guid.NewGuid().ToString()
                }).ToList();
                await messageSender.SendMessagesAsync(sqsOptions.SubscribedQueueName, refreshFolderMessages);

                logger.LogInformation("{DeletedFileCount} expired files deleted from trash", trashedFiles.Count);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while executing DeleteTrashFilesJob. Message: {ErrorMessage}", ex.Message);
            }
        }
    }
}
