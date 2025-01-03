﻿using File = ByteBox.FileStore.Domain.Entities.File;

namespace ByteBox.FileStore.Domain.Repositories;

public interface IFileRepository
{
    Task AddAsync(File file);
    Task<bool> IsUniqueFileName(string fileName, Guid folderId);
}
