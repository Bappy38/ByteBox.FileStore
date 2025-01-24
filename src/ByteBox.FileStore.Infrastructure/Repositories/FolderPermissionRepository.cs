using ByteBox.FileStore.Domain.DTOs;
using ByteBox.FileStore.Domain.Entities;
using ByteBox.FileStore.Domain.Repositories;
using ByteBox.FileStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<FolderPermissionDto>> GetFolderPermissionsAsync(Guid userId, List<Guid> folderIds)
    {
        return await _dbContext.FolderPermissions
            .AsNoTracking()
            .Where(fp => fp.UserId == userId && folderIds.Contains(fp.FolderId))
            .Select(fp => new FolderPermissionDto
            {
                FolderId = fp.FolderId,
                UserId = fp.UserId,
                AccessLevel = fp.AccessLevel,
                GrantedAtUtc = fp.GrantedAtUtc
            })
            .ToListAsync();
    }

    public async Task<bool> HasPermission(Guid userId, Guid folderId)
    {
        return await _dbContext.FolderPermissions.AnyAsync(fp => fp.UserId == userId && fp.FolderId == folderId);
    }
}
