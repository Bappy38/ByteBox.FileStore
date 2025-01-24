namespace ByteBox.FileStore.Domain.DTOs;

public sealed class FolderPathDto
{
    public Guid FolderId { get; init; }
    public string FolderName { get; set; }
    public string AncestorIds { get; init; }

    public List<Guid> GetFolderPath()
    {
        var folderIdsInPath = AncestorIds.Split('|').Select(Guid.Parse).ToList();
        folderIdsInPath.Add(FolderId);

        return folderIdsInPath;
    }
}
