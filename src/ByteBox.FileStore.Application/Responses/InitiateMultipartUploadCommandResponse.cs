namespace ByteBox.FileStore.Application.Responses;

public record InitiateMultipartUploadCommandResponse
{
    public Guid FileId { get; set; }
    public string UploadId { get; set; }
}
