using ByteBox.FileStore.Application.Commands;
using ByteBox.FileStore.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ByteBox.FileStore.Application.BackgroundJobs;

public class RefreshThumbnailPresignedJob : BackgroundService
{
    private const int JobRecurringIntervalInMinutes = 5;
    private const int ExpiredInDays = 5;    // Each thumbnail presigned URL will valid for 7 days. We will refresh it after 5 days. So, ExpiredInDays will be 5.
    private const int BatchSize = 500;

    private readonly IServiceProvider _serviceProvider;

    public RefreshThumbnailPresignedJob(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested)
        {
            await RefreshThumbnailPresignedAsync();
            await Task.Delay(TimeSpan.FromMinutes(JobRecurringIntervalInMinutes), stoppingToken);
        }
    }

    private async Task RefreshThumbnailPresignedAsync()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<RefreshThumbnailPresignedJob>>();
            var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

            try
            {
                var lastExpiredDate = DateTime.UtcNow.AddDays(-ExpiredInDays);
                var thumbnailExpiredFiles = await dbContext.Files
                    .Where(f => f.TrashedAt == null && f.ThumbnailPresignedGeneratedAt <= lastExpiredDate)
                    .Take(BatchSize)
                    .ToListAsync();

                var refreshThumbnailPresignedCommands = thumbnailExpiredFiles.Select(file =>
                    new RefreshThumbnailPresignedCommand
                    {
                        FileId = file.FileId
                    }
                );
                foreach (var command in refreshThumbnailPresignedCommands)
                {
                    await mediatr.Send(command);
                }

                logger.LogInformation("{ThumbnailRefreshedFileCount} expired thumbnail refreshed", thumbnailExpiredFiles.Count);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while executing RefreshThumbnailPresignedJob. Message: {ErrorMessage}", ex.Message);
            }
        }
    }
}
