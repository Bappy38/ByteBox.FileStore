using ByteBox.FileStore.Domain.DTOs;
using ByteBox.FileStore.Domain.Entities;

namespace ByteBox.FileStore.Domain.Repositories;

public interface IFilePermissionRepository
{
    Task AddAsync(FilePermission file);
    Task<FilePermissionDto?> GetAsync(Guid fileId, Guid userId);
}
