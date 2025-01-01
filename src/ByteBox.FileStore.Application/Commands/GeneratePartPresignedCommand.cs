using ByteBox.FileStore.Application.Abstraction;
using ByteBox.FileStore.Application.Responses;

namespace ByteBox.FileStore.Application.Commands;

public record GeneratePartPresignedCommand : ICommand<GeneratePartPresignedResponse>
{
    public Guid FileId { get; init; }
    public string UploadId { get; init; }
    public int PartNumber { get; init; }
}
