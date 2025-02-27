using ByteBox.FileStore.Application.Abstraction;

namespace ByteBox.FileStore.Application.Commands;

public record DeleteFolderCommand : ICommand
{
    public Guid FolderId { get; init; }
}
