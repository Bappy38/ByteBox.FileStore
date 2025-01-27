namespace ByteBox.FileStore.Application.Responses;

public record CompleteMultipartUploadResponse
{
    public Guid FileId { get; init; }
    public string FileName { get; init; }
    public double FileSizeInMb { get; init; }
    public string FileType { get; init; }
    public string ThumbnailUrl { get; init; }
}
