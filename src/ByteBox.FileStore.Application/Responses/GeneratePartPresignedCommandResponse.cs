namespace ByteBox.FileStore.Application.Responses;

public record GeneratePartPresignedCommandResponse
{
    public Guid FileId { get; set; }
    public string PreSignedUrl { get; set; }
}
