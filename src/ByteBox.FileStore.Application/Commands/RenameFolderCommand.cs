using ByteBox.FileStore.Application.Abstraction;

namespace ByteBox.FileStore.Application.Commands;

public record RenameFolderCommand : ICommand
{
    public Guid FolderId { get; init; }
    public string FolderName { get; init; }
}
