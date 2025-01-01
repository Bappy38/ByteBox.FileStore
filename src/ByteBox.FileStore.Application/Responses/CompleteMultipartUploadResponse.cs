namespace ByteBox.FileStore.Application.Responses;

public record CompleteMultipartUploadResponse
{
    public Guid FileId { get; set; }
    public string Location { get; set; }
}
