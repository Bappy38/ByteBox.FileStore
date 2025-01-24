namespace ByteBox.FileStore.Domain.DTOs;

public sealed record BreadcrumbDto
{
    public Guid FolderId { get; set; }
    public string FolderName { get; set; }
}
