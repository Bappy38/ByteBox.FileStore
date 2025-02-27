using ByteBox.FileStore.Application.Abstraction;
using ByteBox.FileStore.Domain.Repositories;
using ByteBox.FileStore.Infrastructure.Data;
using NexaWrap.SQS.NET.Interfaces;

namespace ByteBox.FileStore.Application.Commands.Handlers;

public class DeleteFolderCommandHandler : ICommandHandler<DeleteFolderCommand>
{
    private readonly IFolderRepository _folderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageSender _messageSender;

    public DeleteFolderCommandHandler(
        IFolderRepository folderRepository,
        IUnitOfWork unitOfWork,
        IMessageSender messageSender)
    {
        _folderRepository = folderRepository;
        _unitOfWork = unitOfWork;
        _messageSender = messageSender;
    }

    public async Task Handle(DeleteFolderCommand request, CancellationToken cancellationToken)
    {
        var folder = await _folderRepository.GetFolderByIdAsync(request.FolderId);

        if (folder is null)
        {
            throw new Exception("Folder not found");
        }

        folder.MoveToTrash();
        await _folderRepository.UpdateAsync(folder);
        await _unitOfWork.SaveChangesAsync();

        await SendFolderDeletedMessageAsync();
    }

    private async Task SendFolderDeletedMessageAsync()
    {
        // TODO:: In the handler, we will move all the descendant files and folders to trash also. Then, background job will clean these files, folders along with their permission entries after certain period.
    }
}
