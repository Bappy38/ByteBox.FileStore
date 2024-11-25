namespace ByteBox.FileStore.Domain.Interfaces;

public interface ISoftDeletable
{
    public bool IsDeleted { get; set; }
}
