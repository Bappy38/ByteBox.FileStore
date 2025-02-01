using ByteBox.FileStore.Domain.Enums;

namespace ByteBox.FileStore.Domain.DTOs;

public sealed record FilePermissionDto
{
    public Guid FileId { get; set; }
    public Guid UserId { get; set; }
    public AccessLevel AccessLevel { get; set; }
    public DateTime GrantedAtUtc { get; set; }
}
