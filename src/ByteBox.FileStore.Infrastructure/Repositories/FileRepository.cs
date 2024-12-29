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

    public async Task<bool> IsUniqueFileName(string fileName, Guid folderId)
    {
        return !await _dbContext.Files.AnyAsync(f => f.FolderId == folderId && f.FileName == fileName);
    }
}
