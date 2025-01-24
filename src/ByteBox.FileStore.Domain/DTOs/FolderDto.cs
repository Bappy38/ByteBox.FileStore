namespace ByteBox.FileStore.Domain.DTOs;

public sealed record FolderDto
{
    public Guid FolderId { get; init; }
    public string FolderName { get; init; }
    public double FolderSizeInMb { get; init; }
    public List<FileDto> Files { get; init; } = new List<FileDto>();
    public List<SubFolderDto> SubFolders { get; init; } = new List<SubFolderDto>();
}
