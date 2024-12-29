namespace ByteBox.FileStore.Application.Responses;

public record CompleteMultipartUploadCommandResponse
{
    public Guid FileId { get; set; }
    public string Location { get; set; }
}
