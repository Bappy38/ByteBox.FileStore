using ByteBox.FileStore.Domain.Entities;

namespace ByteBox.FileStore.Domain.Repositories;

public interface IDriveRepository
{
    Task AddAsync(Drive drive);
    Task<Drive?> GetByIdAsync(Guid driveId);
}
