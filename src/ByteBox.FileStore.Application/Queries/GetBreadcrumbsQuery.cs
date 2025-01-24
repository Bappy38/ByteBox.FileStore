using ByteBox.FileStore.Application.Abstraction;
using ByteBox.FileStore.Domain.DTOs;

namespace ByteBox.FileStore.Application.Queries;

public sealed record GetBreadcrumbsQuery : IQuery<List<BreadcrumbDto>>
{
    public Guid FolderId { get; init; }
}
