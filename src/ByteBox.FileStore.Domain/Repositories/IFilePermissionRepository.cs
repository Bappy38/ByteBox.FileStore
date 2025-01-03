﻿using ByteBox.FileStore.Domain.Entities;

namespace ByteBox.FileStore.Domain.Repositories;

public interface IFilePermissionRepository
{
    Task AddAsync(FilePermission file);
    Task<FilePermission?> GetAsync(Guid fileId, Guid userId);
}
