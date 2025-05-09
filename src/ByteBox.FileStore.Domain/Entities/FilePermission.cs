﻿using ByteBox.FileStore.Domain.Enums;
using ByteBox.FileStore.Domain.Interfaces;

namespace ByteBox.FileStore.Domain.Entities;

public class FilePermission : IAuditable, ISoftDeletable
{
    public Guid FileId { get; set; }
    public File File { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public AccessLevel AccessLevel { get; set; }
    public DateTime GrantedAtUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; }
    public Guid CreatedByUserId { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public Guid? UpdatedByUserId { get; set; }
    public User CreatedBy { get; set; }
    public User? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }
}
