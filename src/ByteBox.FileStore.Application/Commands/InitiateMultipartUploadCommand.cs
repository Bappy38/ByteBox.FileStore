using ByteBox.FileStore.Application.Abstraction;
using ByteBox.FileStore.Application.Responses;

namespace ByteBox.FileStore.Application.Commands;

public sealed record InitiateMultipartUploadCommand : ICommand<InitiateMultipartUploadResponse>
{
    public Guid FolderId { get; set; }
    public double FileSizeInMb { get; set; }
    public string FileName { get; init; }
    public string ContentType { get; init; }
}
