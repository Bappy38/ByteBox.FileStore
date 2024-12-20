using ByteBox.FileStore.Domain.Entities;
using ByteBox.FileStore.Domain.Repositories;
using ByteBox.FileStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ByteBox.FileStore.Infrastructure.Repositories;

public class FolderRepository : IFolderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public FolderRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Folder folder)
    {
        await _dbContext.Folders.AddAsync(folder);
    }

    public async Task<bool> IsUniqueFolderName(string folderName, Guid parentFolderId)
    {
        return !await _dbContext.Folders.AnyAsync(f => f.ParentFolderId == parentFolderId && f.FolderName == folderName);
    }
}
