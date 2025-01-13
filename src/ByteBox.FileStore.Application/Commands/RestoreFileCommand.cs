using ByteBox.FileStore.Application.Abstraction;

namespace ByteBox.FileStore.Application.Commands;

public record RestoreFileCommand : ICommand
{
    public Guid FileId { get; set; }
}
