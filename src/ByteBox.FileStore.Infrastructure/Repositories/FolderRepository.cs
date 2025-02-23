using ByteBox.FileStore.Domain.DTOs;
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

    public async Task<FolderDto?> GetFolderByIdAsync(Guid folderId)
    {
        return await _dbContext.Folders
            .AsNoTracking()
            .Where(f => f.FolderId == folderId)
            .Select(f => new FolderDto
            {
                FolderId = f.FolderId,
                FolderName = f.FolderName,
                FolderSizeInMb = f.FolderSizeInMb,
                Files = f.Files.Where(f => f.TrashedAt == null && f.IsUploadCompleted).Select(file => new FileDto
                {
                    FileId = file.FileId,
                    FileName = file.FileName,
                    FileSizeInMb = file.FileSizeInMb,
                    FileType = file.FileType,
                    ThumbnailUrl = file.ThumbnailPresignedUrl
                }).ToList(),
                SubFolders = f.SubFolders.Select(sf => new SubFolderDto
                {
                    FolderId = sf.FolderId,
                    FolderName = sf.FolderName
                }).ToList(),
            }).FirstOrDefaultAsync();
    }

    public async Task<FolderPathDto?> GetFolderPathByIdAsync(Guid folderId)
    {
        return await _dbContext.Folders
            .AsNoTracking()
            .Where(f => f.FolderId == folderId)
            .Select(f => new FolderPathDto
            {
                FolderId = f.FolderId,
                FolderName = f.FolderName,
                AncestorIds = f.AncestorIds
            }).FirstOrDefaultAsync();
    }

    public async Task<List<FolderPathDto>> GetFoldersPathByIdsAsync(List<Guid> folderIds)
    {
        return await _dbContext.Folders
            .AsNoTracking()
            .Where(f => folderIds.Contains(f.FolderId))
            .Select(f => new FolderPathDto
            {
                FolderId = f.FolderId,
                FolderName = f.FolderName,
                AncestorIds = f.AncestorIds
            }).ToListAsync();
    }

    public async Task<bool> IsUniqueFolderName(string folderName, Guid parentFolderId)
    {
        return !await _dbContext.Folders.AnyAsync(f => f.ParentFolderId == parentFolderId && f.FolderName == folderName);
    }
}
