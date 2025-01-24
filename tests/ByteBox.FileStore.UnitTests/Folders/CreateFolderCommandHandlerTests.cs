using ByteBox.FileStore.Application.Commands;
using ByteBox.FileStore.Application.Commands.Handlers;
using ByteBox.FileStore.Domain.Constants;
using ByteBox.FileStore.Domain.Entities;
using ByteBox.FileStore.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace ByteBox.FileStore.UnitTests.Folders;

public sealed class CreateFolderCommandHandlerTests : TestBase
{
    private readonly CreateFolderCommand Command = new CreateFolderCommand
    {
        FolderName = "Folder A",
        ParentFolderId = Default.Folder.RootFolderId
    };

    private readonly CreateFolderCommandHandler _handler;

    public CreateFolderCommandHandlerTests()
    {
        _handler = new CreateFolderCommandHandler(_folderRepository, _folderPermissionRepository, _unitOfWork, Mock.Of<ILogger<CreateFolderCommandHandler>>());
        SeedData();
    }

    [Fact]
    public async Task Handle_ShouldReturnException_WhenFolderWithSameNameAlreadyExist()
    {
        // Arrange
        var command = Command with { FolderName = "Folder X" };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, default));

        Assert.Equal($"Folder with name {command.FolderName} has already exist", exception.Message);
    }

    [Fact]
    public async Task Handle_ShouldCreateFolderAndPermission_WhenValidCommand()
    {
        // Arrange

        // Act
        var result = await _handler.Handle(Command, default);

        // Assert
        var createdFolder = await _dbContext.Folders.FirstOrDefaultAsync(f => f.FolderId == result);
        Assert.NotNull(createdFolder);

        var folderPermission = await _dbContext.FolderPermissions.FirstOrDefaultAsync(fp => fp.FolderId == createdFolder.FolderId && fp.UserId == Default.User.UserId);
        Assert.NotNull(folderPermission);
        Assert.Equal(AccessLevel.Owner, folderPermission.AccessLevel);
    }

    private void SeedData()
    {
        var folder = new Folder
        {
            FolderId = Guid.NewGuid(),
            FolderName = "Folder X",
            ParentFolderId = Default.Folder.RootFolderId
        };
        _dbContext.Folders.Add(folder);

        var folderPermission = new FolderPermission
        {
            FolderId = folder.FolderId,
            UserId = Default.User.UserId,
            AccessLevel = AccessLevel.Owner,
            GrantedAtUtc = DateTime.UtcNow
        };
        _dbContext.FolderPermissions.Add(folderPermission);

        _dbContext.SaveChanges();
    }
}
