using ByteBox.FileStore.Domain.Entities;
using ByteBox.FileStore.Domain.Repositories;
using ByteBox.FileStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ByteBox.FileStore.Infrastructure.Repositories;

public class DriveRepository : IDriveRepository
{
    private readonly ApplicationDbContext _dbContext;

    public DriveRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Drive drive)
    {
        await _dbContext.Drives.AddAsync(drive);
    }

    public async Task<Drive?> GetByIdAsync(Guid driveId)
    {
        return await _dbContext.Drives.FirstOrDefaultAsync(d => d.DriveId == driveId);
    }
}
