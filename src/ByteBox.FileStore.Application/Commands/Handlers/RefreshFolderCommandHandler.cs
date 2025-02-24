using ByteBox.FileStore.Application.Abstraction;
using ByteBox.FileStore.Domain.Repositories;
using ByteBox.FileStore.Infrastructure.Data;

namespace ByteBox.FileStore.Application.Commands.Handlers;

public class RefreshFolderCommandHandler : ICommandHandler<RefreshFolderCommand>
{
    private readonly IFolderRepository _folderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshFolderCommandHandler(IFolderRepository folderRepository, IUnitOfWork unitOfWork)
    {
        _folderRepository = folderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RefreshFolderCommand request, CancellationToken cancellationToken)
    {
        var folderId = request.FolderId;
        while (true)
        {
            var folder = await _folderRepository.GetFolderByIdAsync(folderId);
            if (folder is null)
            {
                break;
            }

            var totalSubFoldersSize = folder.SubFolders.Where(sf => !sf.IsDeleted).Sum(sf => sf.FolderSizeInMb);
            var totalFilesSize = folder.Files.Where(f => !f.IsDeleted).Sum(f => f.FileSizeInMb);
            folder.FolderSizeInMb = totalSubFoldersSize + totalFilesSize;

            await _folderRepository.UpdateAsync(folder);
            await _unitOfWork.SaveChangesAsync();

            if (folder.ParentFolderId is null)
            {
                break;
            }

            folderId = folder.ParentFolderId ?? Guid.Empty;
        }
    }
}
