namespace ByteBox.FileStore.Application.Responses;

public record GeneratePartPresignedResponse
{
    public Guid FileId { get; set; }
    public string PreSignedUrl { get; set; }
}
