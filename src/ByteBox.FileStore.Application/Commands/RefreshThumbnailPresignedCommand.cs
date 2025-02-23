using ByteBox.FileStore.Application.Abstraction;

namespace ByteBox.FileStore.Application.Commands;

public class RefreshThumbnailPresignedCommand : ICommand
{
    public Guid FileId { get; set; }
    public string ThumbnailKey { get; set; }
    public string ThumbnailPresignedUrl { get; set; }
    public DateTime ThumbnailPresignedGeneratedAt { get; set; }
}
