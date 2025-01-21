using ByteBox.FileStore.Domain.DTOs;
using ByteBox.FileStore.Domain.Entities;

namespace ByteBox.FileStore.Domain.Repositories;

public interface IFolderPermissionRepository
{
    // INFO:: Follow CRUD order
    Task AddAsync(FolderPermission folderPermission);
    Task<List<FolderPermissionDto>> GetFolderPermissionsAsync(Guid userId, List<Guid> folderIds);
    Task<bool> HasPermission(Guid userId, Guid folderId);
}
