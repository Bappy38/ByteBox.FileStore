using ByteBox.FileStore.Application.Abstraction;

namespace ByteBox.FileStore.Application.Commands;

public record DeleteFileCommand : ICommand
{
    public Guid FileId { get; init; }
}
