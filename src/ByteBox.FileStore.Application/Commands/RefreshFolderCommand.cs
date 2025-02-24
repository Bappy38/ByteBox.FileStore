using ByteBox.FileStore.Application.Abstraction;

namespace ByteBox.FileStore.Application.Commands;

public class RefreshFolderCommand : ICommand
{
    public Guid FolderId { get; set; }
}
