using ByteBox.FileStore.Application.Abstraction;
using ByteBox.FileStore.Domain.Constants;
using ByteBox.FileStore.Domain.DTOs;
using ByteBox.FileStore.Domain.Repositories;

namespace ByteBox.FileStore.Application.Queries.Handlers;

public class GetFolderQueryHandler : IQueryHandler<GetFolderQuery, FolderDto>
{
    private readonly IFolderRepository _folderRepository;
    private readonly IFolderPermissionRepository _permissionRepository;

    public GetFolderQueryHandler(
        IFolderRepository folderRepository,
        IFolderPermissionRepository folderPermissionRepository)
    {
        _folderRepository = folderRepository;
        _permissionRepository = folderPermissionRepository;
    }

    public async Task<FolderDto> Handle(GetFolderQuery request, CancellationToken cancellationToken)
    {
        if (!await _permissionRepository.HasPermission(Default.User.UserId, request.FolderId))
        {
            throw new Exception($"Folder not found");
        }

        var folder = await _folderRepository.GetFolderDtoByIdAsync(request.FolderId);
        if (folder is null)
        {
            throw new Exception($"Folder not found");
        }
        return folder;
    }
}
