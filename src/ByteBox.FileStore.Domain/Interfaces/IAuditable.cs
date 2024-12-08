using ByteBox.FileStore.Domain.Entities;

namespace ByteBox.FileStore.Domain.Interfaces;

public interface IAuditable
{
    public DateTime CreatedAtUtc { get; set; }
    public Guid CreatedByUserId { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public Guid? UpdatedByUserId { get; set; }

    public User CreatedBy { get; set; }
    public User? UpdatedBy { get; set; }
}
