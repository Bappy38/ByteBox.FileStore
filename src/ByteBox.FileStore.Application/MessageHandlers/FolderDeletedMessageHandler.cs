using ByteBox.FileStore.Application.Commands;
using ByteBox.FileStore.Application.Queries;
using ByteBox.FileStore.Infrastructure.Messages;
using MediatR;
using NexaWrap.SQS.NET.Interfaces;

namespace ByteBox.FileStore.Application.MessageHandlers;

public class FolderDeletedMessageHandler : IMessageHandler<FolderDeletedMessage>
{
    private readonly IMediator _mediator;

    public FolderDeletedMessageHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task HandleAsync(FolderDeletedMessage message)
    {
        var getFolderQuery = new GetFolderQuery
        {
            FolderId = message.FolderId
        };
        var deletedFolder = await _mediator.Send(getFolderQuery);

        foreach (var subFolder in deletedFolder.SubFolders)
        {
            var command = new DeleteFolderCommand
            {
                FolderId = subFolder.FolderId
            };
            await _mediator.Send(command);
        }
    }
}
