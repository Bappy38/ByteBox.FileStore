namespace ByteBox.FileStore.Application.Responses;

public record GetFileDownloadUrlResponse
{
    public Guid FileId { get; set; }
    public string DownloadUrl { get; set; }
}
