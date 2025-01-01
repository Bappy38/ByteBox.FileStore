namespace ByteBox.FileStore.Application.Responses;

public record InitiateMultipartUploadResponse
{
    public Guid FileId { get; set; }
    public string UploadId { get; set; }
}
