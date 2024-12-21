using ByteBox.FileStore.Application.Queries;
using ByteBox.FileStore.Application.Queries.Handlers;
using ByteBox.FileStore.Domain.Constants;
using ByteBox.FileStore.Domain.Entities;

namespace ByteBox.FileStore.UnitTests.Folders;

public sealed class GetFolderQueryHandlerTests : TestBase
{
    private readonly GetFolderQuery Query = new GetFolderQuery
    {
        FolderId = Default.Folder.RootFolderId
    };

    private readonly GetFolderQueryHandler _handler;
    public GetFolderQueryHandlerTests()
    {
        _handler = new GetFolderQueryHandler(_folderRepository, _folderPermissionRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFoundException_WhenFolderDoesNotExist()
    {
        // Arrange
        var query = Query with { FolderId = Guid.NewGuid() };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(query, default));

        Assert.Equal("Folder not found", exception.Message);
    }

    [Fact]
    public async Task Handle_Should_ReturnNotFoundException_WhenDoesNotHavePermission()
    {
        // TODO:: Will implement it after implementing authorization.
    }

    [Fact]
    public async Task Handle_ShouldReturnFolderDto_WhenFolderExist()
    {
        // Arrange
        var folder = new Folder
        {
            FolderId = Guid.NewGuid(),
            FolderName = "Test Folder",
            ParentFolderId = Default.Folder.RootFolderId,
            CreatedAtUtc = DateTime.UtcNow,
            CreatedByUserId = Default.User.UserId
        };

        _dbContext.Folders.Add(folder);
        _dbContext.SaveChanges();

        // Act
        var result = await _handler.Handle(Query, default);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(Default.Folder.RootFolderId, result.FolderId);
        Assert.Equal(Default.Folder.RootFolderName, result.FolderName);

        Assert.Single(result.SubFolders);
        Assert.Equal(folder.FolderId, result.SubFolders[0].FolderId);
        Assert.Equal(folder.FolderName, result.SubFolders[0].FolderName);

        Assert.Empty(result.Files);
    }
}
