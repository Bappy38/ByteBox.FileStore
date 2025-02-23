using ByteBox.FileStore.Domain.DTOs;
using ByteBox.FileStore.Domain.Entities;
using ByteBox.FileStore.Domain.Repositories;
using ByteBox.FileStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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

    public async Task<FilePermissionDto?> GetAsync(Guid fileId, Guid userId)
    {
        return await _dbContext.FilePermissions
            .Where(fp => fp.FileId == fileId && fp.UserId == userId)
            .Include(fp => fp.File)
            .Select(fp => new FilePermissionDto
            {
                FileId = fp.FileId,
                UserId = fp.UserId,
                AccessLevel = fp.AccessLevel,
                GrantedAtUtc = fp.GrantedAtUtc
            }).FirstOrDefaultAsync();
    }
}
