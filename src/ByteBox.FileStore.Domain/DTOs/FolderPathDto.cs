namespace ByteBox.FileStore.Domain.DTOs;

public sealed record FolderPathDto
{
    public Guid FolderId { get; init; }
    public string FolderName { get; set; }
    public string AncestorIds { get; init; }
}
