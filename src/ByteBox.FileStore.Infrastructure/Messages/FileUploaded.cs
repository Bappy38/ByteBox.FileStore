namespace ByteBox.FileStore.Infrastructure.Messages;

/**
 * In the FileUploadedHandler,
 * 1. Give FilePermission to all the user who have access on all of it's anchestor folder
 * 2. Update folder size of all it's anchestor folder
**/
public record FileUploaded
{
    public Guid FileId { get; init; }
    public string FileName { get; init; }
    public double FileSizeInMb { get; init; }
    public string ContentType { get; init; }

    public Guid FolderId { get; init; }
}
