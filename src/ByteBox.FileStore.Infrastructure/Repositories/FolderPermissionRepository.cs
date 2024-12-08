using ByteBox.FileStore.Domain.Entities;
using ByteBox.FileStore.Domain.Repositories;
using ByteBox.FileStore.Infrastructure.Data;

namespace ByteBox.FileStore.Infrastructure.Repositories;

public class FolderPermissionRepository : IFolderPermissionRepository
{
    private readonly ApplicationDbContext _dbContext;

    public FolderPermissionRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(FolderPermission folderPermission)
    {
        await _dbContext.FolderPermissions.AddAsync(folderPermission);
    }
}
