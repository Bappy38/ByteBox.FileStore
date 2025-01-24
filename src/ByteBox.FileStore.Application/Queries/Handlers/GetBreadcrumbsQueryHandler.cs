
using ByteBox.FileStore.Application.Abstraction;
using ByteBox.FileStore.Domain.Constants;
using ByteBox.FileStore.Domain.DTOs;
using ByteBox.FileStore.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace ByteBox.FileStore.Application.Queries.Handlers;

public class GetBreadcrumbsQueryHandler : IQueryHandler<GetBreadcrumbsQuery, List<BreadcrumbDto>>
{
    private readonly IFolderRepository _folderRepository;
    private readonly IFolderPermissionRepository _folderPermissionRepository;
    private readonly ILogger<GetBreadcrumbsQueryHandler> _logger;

    public GetBreadcrumbsQueryHandler(
        IFolderRepository folderRepository,
        IFolderPermissionRepository folderPermissionRepository,
        ILogger<GetBreadcrumbsQueryHandler> logger)
    {
        _folderRepository = folderRepository;
        _folderPermissionRepository = folderPermissionRepository;
        _logger = logger;
    }

    public async Task<List<BreadcrumbDto>> Handle(GetBreadcrumbsQuery request, CancellationToken cancellationToken)
    {
        var folder = await _folderRepository.GetFolderPathByIdAsync(request.FolderId);
        if (folder is null)
        {
            _logger.LogInformation("Folder with ID {FolderId} not found", request.FolderId);
            throw new Exception("Folder not found");
        }

        var pathFolderIds = folder.GetFolderPath();
        var pathFolders = await _folderRepository.GetFoldersPathByIdsAsync(pathFolderIds);
        var pathFoldersPermission = await _folderPermissionRepository.GetFolderPermissionsAsync(Default.User.UserId, pathFolderIds);  // TODO:: Replace with real user id

        var folderLookup = pathFolders.ToDictionary(f => f.FolderId, f => f);
        var permittedFolders = pathFoldersPermission.Select(fp => fp.FolderId).ToHashSet();

        if (!permittedFolders.Contains(folder.FolderId))
        {
            _logger.LogInformation("User with ID {UserId} doesn't have permission to access folder with ID {FolderId}", Default.User.UserId, folder.FolderId);
            throw new Exception("Folder not found");
        }


        var breadcrumbs = new List<BreadcrumbDto>();
        foreach (var pathFolderId in pathFolderIds)
        {
            if (!permittedFolders.Contains(pathFolderId))
            {
                continue;
            }
            var pathFolder = folderLookup[pathFolderId];

            breadcrumbs.Add(new BreadcrumbDto
            {
                FolderId = pathFolder.FolderId,
                FolderName = pathFolder.FolderName
            });
        }
        return breadcrumbs;
    }
}
