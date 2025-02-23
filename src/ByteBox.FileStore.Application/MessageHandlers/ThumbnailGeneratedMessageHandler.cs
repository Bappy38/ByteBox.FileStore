using ByteBox.FileStore.Application.Commands;
using ByteBox.FileStore.Infrastructure.Messages;
using MediatR;
using Microsoft.Extensions.Logging;
using NexaWrap.SQS.NET.Interfaces;

namespace ByteBox.FileStore.Application.MessageHandlers;

public class ThumbnailGeneratedMessageHandler : IMessageHandler<ThumbnailGeneratedMessage>
{
    private readonly IMediator _mediator;
    private readonly ILogger<ThumbnailGeneratedMessageHandler> _logger;

    public ThumbnailGeneratedMessageHandler(
        IMediator mediator,
        ILogger<ThumbnailGeneratedMessageHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task HandleAsync(ThumbnailGeneratedMessage message)
    {
        try
        {
            var refreshThumbnailCommand = new RefreshThumbnailPresignedCommand
            {
                FileId = message.FileId,
                ThumbnailKey = message.ThumbnailKey,
                ThumbnailPresignedUrl = message.ThumbnailPresignedUrl,
                ThumbnailPresignedGeneratedAt = message.ThumbnailPresignedGeneratedAt
            };
            await _mediator.Send(refreshThumbnailCommand);
            _logger.LogInformation("ThumbnailGeneratedMessage processed successfully for file with ID: {FileId}", message.FileId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process ThumbnailGeneratedMessage for file with ID: {FileId}. ExceptionMessage: {ExceptionMessage}", message.FileId, ex.Message);
            throw;
        }
    }
}
