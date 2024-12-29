using ByteBox.FileStore.Application.Abstraction;
using ByteBox.FileStore.Application.Responses;

namespace ByteBox.FileStore.Application.Commands;

public class CompleteMultipartUploadCommand : ICommand<CompleteMultipartUploadCommandResponse>
{
    public Guid FileId { get; init; }
    public string FileName { get; init; }
    public double FileSizeInMb { get; init; }
    public string ContentType { get; init; }

    public Guid FolderId { get; init; }

    public string UploadId { get; init; }
    public List<PartETagInfo> Parts { get; init; }
}

public class PartETagInfo
{
    public int PartNumber { get; set; }
    public string ETag { get; set; }
}