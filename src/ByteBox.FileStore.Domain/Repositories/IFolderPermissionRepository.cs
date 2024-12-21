using ByteBox.FileStore.Domain.Entities;

namespace ByteBox.FileStore.Domain.Repositories;

public interface IFolderPermissionRepository
{
    Task AddAsync(FolderPermission folderPermission);
    Task<bool> HasPermission(Guid userId, Guid folderId);
}
