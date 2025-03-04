using ByteBox.FileStore.Domain.Repositories;
using ByteBox.FileStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using File = ByteBox.FileStore.Domain.Entities.File;

namespace ByteBox.FileStore.Infrastructure.Repositories;

public class FileRepository : IFileRepository
{
    private readonly ApplicationDbContext _dbContext;

    public FileRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(File file)
    {
        await _dbContext.Files.AddAsync(file);
    }

    public async Task<File?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Files
            .Where(f => f.TrashedAt == null)
            .FirstOrDefaultAsync(f => f.FileId == id);
    }

    public async Task<List<File>> GetFilesInsideFolderAsync(Guid folderId)
    {
        return await _dbContext.Files
            .Where(f => f.TrashedAt == null && f.FolderId == folderId)
            .ToListAsync();
    }

    public async Task<File?> GetTrashedFileByIdAsync(Guid id)
    {
        return await _dbContext.Files
            .Where(f => f.TrashedAt != null)
            .FirstOrDefaultAsync(f => f.FileId == id);
    }

    public async Task UpdateAsync(File file)
    {
        _dbContext.Files.Update(file);
    }

    public async Task RemoveAsync(File file)
    {
        _dbContext.Files.Remove(file);
    }

    public async Task<bool> IsUniqueFileName(string fileName, Guid folderId)
    {
        return !await _dbContext.Files
            .Where(f => f.FolderId == folderId && f.TrashedAt == null && f.IsUploadCompleted && f.FileName == fileName)
            .AnyAsync();
    }
}
