using ByteBox.FileStore.Application.Abstraction;
using ByteBox.FileStore.Domain.DTOs;

namespace ByteBox.FileStore.Application.Queries;

public sealed record GetFolderQuery : IQuery<FolderDto>
{
    public Guid FolderId { get; init; }
}
