using ByteBox.FileStore.Application.Abstraction;

namespace ByteBox.FileStore.Application.Commands;

public record PermanentDeleteFileCommand : ICommand
{
    public Guid FileId { get; init; }
}
