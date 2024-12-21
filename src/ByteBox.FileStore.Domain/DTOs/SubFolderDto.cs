namespace ByteBox.FileStore.Domain.DTOs;

public sealed record SubFolderDto
{
    public Guid FolderId { get; set; }
    public string FolderName { get; set; }
}
