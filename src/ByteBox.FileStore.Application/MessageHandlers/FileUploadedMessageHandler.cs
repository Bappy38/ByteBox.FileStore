using ByteBox.FileStore.Application.Commands;
using ByteBox.FileStore.Infrastructure.Messages;
using MediatR;
using NexaWrap.SQS.NET.Interfaces;

namespace ByteBox.FileStore.Application.MessageHandlers;

/// <summary>
/// This handler will propagate permission to all the user who have access on any of it's ancestor folder.
/// It will also refresh folder size
/// </summary>
public class FileUploadedMessageHandler : IMessageHandler<FileUploadedMessage>
{
    private readonly IMediator _mediator;

    public FileUploadedMessageHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task HandleAsync(FileUploadedMessage message)
    {
        var refreshFolderCommand = new RefreshFolderCommand
        {
            FolderId = message.FolderId
        };
        await _mediator.Send(refreshFolderCommand);
    }
}
