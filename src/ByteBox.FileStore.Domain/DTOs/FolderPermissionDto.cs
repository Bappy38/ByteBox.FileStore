using ByteBox.FileStore.Domain.Enums;

namespace ByteBox.FileStore.Domain.DTOs;

public sealed record FolderPermissionDto
{
    public Guid FolderId { get; set; }
    public Guid UserId { get; set; }
    public AccessLevel AccessLevel { get; set; }
    public DateTime GrantedAtUtc { get; set; }
}
