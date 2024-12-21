using ByteBox.FileStore.Domain.DTOs;
using ByteBox.FileStore.Domain.Entities;

namespace ByteBox.FileStore.Domain.Repositories;

public interface IFolderRepository
{
    Task AddAsync(Folder folder);
    Task<FolderDto?> GetFolderByIdAsync(Guid folderId);
    Task<bool> IsUniqueFolderName(string folderName, Guid parentFolderId);
}
