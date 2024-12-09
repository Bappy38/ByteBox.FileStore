using Azure.Core;
using ByteBox.FileStore.Application.Commands;
using ByteBox.FileStore.Application.Commands.Handlers;
using ByteBox.FileStore.Domain.Constants;
using ByteBox.FileStore.Domain.Entities;
using ByteBox.FileStore.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ByteBox.FileStore.UnitTests.Users;

public class CreateUserCommandHandlerTests : TestBase
{
    private readonly CreateUserCommand Command = new CreateUserCommand
    {
        UserName = "Bappy38",
        Email = "bappy@gmail.com"
    };

    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _handler = new CreateUserCommandHandler(_userRepository, _driveRepository, _folderRepository, _folderPermissionRepository, _unitOfWork);

        SeedUsers();
    }

    [Fact]
    public async Task Handle_ShouldReturnException_WhenEmailIsNotUnique()
    {
        // Arrange
        var command = Command with { Email = DefaultUser.Email };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, default));

        Assert.Equal($"User with email {command.Email} has already exist", exception.Message);
    }

    [Fact]
    public async Task Handle_ShouldCreateUserDriveFolder_WhenEmailIsUnique()
    {
        // Arrange

        // Act
        var result = await _handler.Handle(Command, default);

        // Assert
        var createdUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == result);
        Assert.NotNull(createdUser);

        var createdDrive = await _dbContext.Drives.FirstOrDefaultAsync(d => d.DriveId == result);
        Assert.NotNull(createdDrive);

        var createdFolder = await _dbContext.Folders.FirstOrDefaultAsync(f => f.FolderId == result);
        Assert.NotNull(createdFolder);

        var folderPermission = await _dbContext.FolderPermissions.FirstOrDefaultAsync(fp => fp.FolderId == createdFolder.FolderId && fp.UserId == createdUser.UserId);
        Assert.NotNull(folderPermission);
        Assert.Equal(AccessLevel.Owner, folderPermission.AccessLevel);
    }

    private void SeedUsers()
    {
        var user = new User
        {
            UserId = DefaultUser.UserId,
            UserName = DefaultUser.UserName,
            Email = DefaultUser.Email
        };

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
    }
}
