using ByteBox.FileStore.Domain.DTOs;
using ByteBox.FileStore.Domain.Entities;

namespace ByteBox.FileStore.Domain.Repositories;

public interface IFolderRepository
{
    // INFO:: Follow CRUD order
    Task AddAsync(Folder folder);
    Task UpdateAsync(Folder folder);
    Task<Folder?> GetFolderByIdAsync(Guid folderId);
    Task<FolderDto?> GetFolderDtoByIdAsync(Guid folderId);
    Task<FolderPathDto?> GetFolderPathByIdAsync(Guid folderId);
    Task<List<FolderPathDto>> GetFoldersPathByIdsAsync(List<Guid> folderIds);
    Task<bool> IsUniqueFolderName(string folderName, Guid parentFolderId);
    Task RemoveAsync(Folder folder);
}
