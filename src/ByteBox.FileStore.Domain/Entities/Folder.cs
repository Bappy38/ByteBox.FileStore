﻿using ByteBox.FileStore.Domain.Interfaces;

namespace ByteBox.FileStore.Domain.Entities;

public class Folder : IAuditable, ISoftDeletable
{
    public Guid FolderId { get; set; }
    public string FolderName { get; set; }
    public double FolderSizeInMb { get; set; }

    public Guid? ParentFolderId { get; set; }
    public Folder? ParentFolder { get; set; }

    public virtual ICollection<File> Files { get; set; }
    public virtual ICollection<Folder> SubFolders { get; set; }

    public DateTime CreatedAtUtc { get; set; }
    public Guid CreatedByUserId { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid UpdatedByUserId { get; set; }
    public User CreatedBy { get; set; }
    public User UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }
}
