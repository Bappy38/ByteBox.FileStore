using ByteBox.FileStore.Application.Abstraction;
using ByteBox.FileStore.Domain.Constants;
using ByteBox.FileStore.Domain.Entities;
using ByteBox.FileStore.Domain.Enums;
using ByteBox.FileStore.Domain.Repositories;
using ByteBox.FileStore.Infrastructure.Data;

namespace ByteBox.FileStore.Application.Commands.Handlers;

public class CreateFolderCommandHandler : ICommandHandler<CreateFolderCommand, Guid>
{
    private readonly IFolderRepository _folderRepository;
    private readonly IFolderPermissionRepository _folderPermissionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateFolderCommandHandler(
        IFolderRepository folderRepository,
        IFolderPermissionRepository folderPermissionRepository,
        IUnitOfWork unitOfWork)
    {
        _folderRepository = folderRepository;
        _folderPermissionRepository = folderPermissionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateFolderCommand request, CancellationToken cancellationToken)
    {
        if (!await _folderRepository.IsUniqueFolderName(request.FolderName, request.ParentFolderId))
        {
            throw new Exception($"Folder with name {request.FolderName} has already exist");
        }

        var folder = new Folder
        {
            FolderId = Guid.NewGuid(),
            FolderName = request.FolderName,
            ParentFolderId = request.ParentFolderId
        };
        await _folderRepository.AddAsync(folder);

        var folderPermission = new FolderPermission
        {
            FolderId = folder.FolderId,
            UserId = Default.User.UserId,       //TODO:: Replace with real UserId
            AccessLevel = AccessLevel.Owner,
            GrantedAtUtc = DateTime.UtcNow
        };
        await _folderPermissionRepository.AddAsync(folderPermission);

        await _unitOfWork.SaveChangesAsync();
        return folder.FolderId;
    }
}
