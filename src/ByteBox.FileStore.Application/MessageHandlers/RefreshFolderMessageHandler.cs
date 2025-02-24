using ByteBox.FileStore.Application.Commands;
using ByteBox.FileStore.Infrastructure.Messages;
using MediatR;
using NexaWrap.SQS.NET.Interfaces;

namespace ByteBox.FileStore.Application.MessageHandlers;

public class RefreshFolderMessageHandler : IMessageHandler<RefreshFolderMessage>
{
    private readonly IMediator _mediator;

    public RefreshFolderMessageHandler(IMediator mediator) => _mediator = mediator;

    public async Task HandleAsync(RefreshFolderMessage message)
    {
        var refreshFolderCommand = new RefreshFolderCommand
        {
            FolderId = message.FolderId
        };
        await _mediator.Send(refreshFolderCommand);
    }
}
