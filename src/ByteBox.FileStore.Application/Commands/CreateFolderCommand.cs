using ByteBox.FileStore.Application.Abstraction;

namespace ByteBox.FileStore.Application.Commands;

public sealed record CreateFolderCommand : ICommand<Guid>
{
    public string FolderName { get; init; }
    public Guid ParentFolderId { get; init; }
}
