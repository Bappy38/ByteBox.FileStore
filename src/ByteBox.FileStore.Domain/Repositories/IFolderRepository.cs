using ByteBox.FileStore.Domain.Entities;

namespace ByteBox.FileStore.Domain.Repositories;

public interface IFolderRepository
{
    Task AddAsync(Folder folder);
    Task<bool> IsUniqueFolderName(string folderName, Guid parentFolderId);
}
