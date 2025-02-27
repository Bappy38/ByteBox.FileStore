using ByteBox.FileStore.Application.Abstraction;
using ByteBox.FileStore.Domain.Repositories;
using ByteBox.FileStore.Infrastructure.Data;

namespace ByteBox.FileStore.Application.Commands.Handlers;

public class RenameFolderCommandHandler : ICommandHandler<RenameFolderCommand>
{
    private readonly IFolderRepository _folderRepository;
    private readonly IUnitOfWork _unitOfWork;


    public RenameFolderCommandHandler(
        IFolderRepository folderRepository,
        IUnitOfWork unitOfWork
        )
    {
        _folderRepository = folderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RenameFolderCommand request, CancellationToken cancellationToken)
    {
        var folder = await _folderRepository.GetFolderByIdAsync(request.FolderId);

        if (folder is null)
        {
            throw new Exception("Folder not found");
        }

        folder.FolderName = request.FolderName;
        await _folderRepository.UpdateAsync(folder);
        await _unitOfWork.SaveChangesAsync();
    }
}
