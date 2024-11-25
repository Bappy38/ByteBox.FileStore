using ByteBox.FileStore.Domain.Interfaces;

namespace ByteBox.FileStore.Domain.Entities;

// TODO: Whenever a user initiate file upload (requests for generating pre-signed url), we will check if the remaining storage is enough to upload the file he is trying to upload.
// TODO: Whenever a file uploaded successfully, we will publish a FileUploadedEvent to update all of its anchestor folders size, propagate FolderPermission to that file
public class File : IAuditable, ISoftDeletable
{
    public Guid FileId { get; set; }
    public string FileName { get; set; }
    public double FileSizeInMb { get; set; }
    public string FileType { get; set; }
    public Guid FolderId { get; set; }
    public Folder Folder { get; set; }

    public DateTime CreatedAtUtc { get; set; }
    public Guid CreatedByUserId { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid UpdatedByUserId { get; set; }
    public User CreatedBy { get; set; }
    public User UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }
}
