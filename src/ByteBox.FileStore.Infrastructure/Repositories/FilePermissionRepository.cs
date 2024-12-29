using ByteBox.FileStore.Domain.Entities;
using ByteBox.FileStore.Domain.Repositories;
using ByteBox.FileStore.Infrastructure.Data;

namespace ByteBox.FileStore.Infrastructure.Repositories;

public class FilePermissionRepository : IFilePermissionRepository
{
    private readonly ApplicationDbContext _dbContext;

    public FilePermissionRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(FilePermission file)
    {
        await _dbContext.FilePermissions.AddAsync(file);
    }
}
