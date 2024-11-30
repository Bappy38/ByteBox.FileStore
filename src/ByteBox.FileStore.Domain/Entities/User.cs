using ByteBox.FileStore.Domain.Interfaces;

namespace ByteBox.FileStore.Domain.Entities;

public class User : ISoftDeletable
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string ProfilePictureUrl { get; set; }
    public bool IsDeleted { get; set; }
}
