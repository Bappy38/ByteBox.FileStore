using ByteBox.FileStore.Application.Abstraction;
using ByteBox.FileStore.Domain.Repositories;
using ByteBox.FileStore.Infrastructure.Data;
using ByteBox.FileStore.Infrastructure.Messages;
using Microsoft.Extensions.Options;
using NexaWrap.SQS.NET.Interfaces;
using NexaWrap.SQS.NET.Models;

namespace ByteBox.FileStore.Application.Commands.Handlers;

public class DeleteFolderCommandHandler : ICommandHandler<DeleteFolderCommand>
{
    private readonly IFileRepository _fileRepository;
    private readonly IFolderRepository _folderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageSender _messageSender;
    private readonly SqsOptions _sqsOptions;

    public DeleteFolderCommandHandler(
        IFileRepository fileRepository,
        IFolderRepository folderRepository,
        IUnitOfWork unitOfWork,
        IMessageSender messageSender,
        IOptions<SqsOptions> sqsOptions)
    {
        _fileRepository = fileRepository;
        _folderRepository = folderRepository;
        _unitOfWork = unitOfWork;
        _messageSender = messageSender;
        _sqsOptions = sqsOptions.Value;
    }

    public async Task Handle(DeleteFolderCommand request, CancellationToken cancellationToken)
    {
        var folder = await _folderRepository.GetFolderByIdAsync(request.FolderId);

        if (folder is null)
        {
            throw new Exception("Folder not found");
        }

        if (folder.ParentFolderId == null)
        {
            throw new Exception("Root folder cannot be deleted");
        }

        folder.MoveToTrash();
        await _folderRepository.UpdateAsync(folder);

        foreach (var file in folder.Files)
        {
            file.MoveToTrash();
            await _fileRepository.UpdateAsync(file);
        }

        await _unitOfWork.SaveChangesAsync();

        await SendFolderDeletedMessageAsync(request.FolderId);
    }

    private async Task SendFolderDeletedMessageAsync(Guid folderId)
    {
        var folderDeletedMessage = new FolderDeletedMessage
        {
            FolderId = folderId,
            CorrelationId = Guid.NewGuid().ToString()
        };
        await _messageSender.SendMessageAsync(_sqsOptions.SubscribedQueueName, folderDeletedMessage);
    }
}
