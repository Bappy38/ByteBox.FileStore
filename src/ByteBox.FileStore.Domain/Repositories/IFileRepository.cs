using File = ByteBox.FileStore.Domain.Entities.File;

namespace ByteBox.FileStore.Domain.Repositories;

public interface IFileRepository
{
    Task AddAsync(File file);
    Task<File?> GetByIdAsync(Guid id);
    Task<List<File>> GetFilesInsideFolderAsync(Guid folderId);
    Task<File?> GetTrashedFileByIdAsync(Guid id);
    Task UpdateAsync(File file);
    Task RemoveAsync(File file);
    Task<bool> IsUniqueFileName(string fileName, Guid folderId);
}
