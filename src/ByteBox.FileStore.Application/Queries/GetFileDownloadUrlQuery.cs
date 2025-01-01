using ByteBox.FileStore.Application.Abstraction;
using ByteBox.FileStore.Application.Responses;

namespace ByteBox.FileStore.Application.Queries;

public sealed record GetFileDownloadUrlQuery : IQuery<GetFileDownloadUrlResponse>
{
    public Guid FileId { get; set; }
}
