using ByteBox.FileStore.Domain.Constants;
using ByteBox.FileStore.Domain.Entities;
using ByteBox.FileStore.Domain.Enums;
using ByteBox.FileStore.Domain.Repositories;
using ByteBox.FileStore.Infrastructure.Data;
using ByteBox.FileStore.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ByteBox.FileStore.UnitTests;

public class TestBase
{
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

    protected readonly ApplicationDbContext _dbContext;
    protected readonly IUnitOfWork _unitOfWork;

    protected readonly IUserRepository _userRepository;
    protected readonly IDriveRepository _driveRepository;
    protected readonly IFolderRepository _folderRepository;
    protected readonly IFolderPermissionRepository _folderPermissionRepository;

    public TestBase()
    {
        _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ApplicationDbContext(_dbContextOptions);
        _unitOfWork = new UnitOfWork(_dbContext);

        _userRepository = new UserRepository(_dbContext);
        _driveRepository = new DriveRepository(_dbContext);
        _folderRepository = new FolderRepository(_dbContext);
        _folderPermissionRepository = new FolderPermissionRepository(_dbContext);

        SeedData();
    }

    private void SeedData()
    {
        var user = new User
        {
            UserId = Default.User.UserId,
            UserName = Default.User.UserName,
            Email = Default.User.Email
        };
        _userRepository.AddAsync(user);

        var drive = new Drive
        {
            DriveId = Default.User.UserId,
            PurchasedStorageInMb = 1024,
            UsedStorageInMb = 0,
            NextBillDate = Default.DateTime,
            OwnerId = Default.User.UserId,
            IsDeleted = false
        };
        _driveRepository.AddAsync(drive);

        var folder = new Folder
        {
            FolderId = Default.User.UserId,
            FolderName = Default.Folder.RootFolderName,
            CreatedAtUtc = Default.DateTime,
            CreatedByUserId = Default.User.UserId,
            IsDeleted = false
        };
        _folderRepository.AddAsync(folder);

        var folderPermission = new FolderPermission
        {
            FolderId = Default.User.UserId,
            UserId = Default.User.UserId,
            AccessLevel = AccessLevel.Owner,
            GrantedAtUtc = Default.DateTime,
            CreatedAtUtc = Default.DateTime,
            CreatedByUserId = Default.User.UserId,
            IsDeleted = false
        };
        _folderPermissionRepository.AddAsync(folderPermission);

        _dbContext.SaveChanges();
    }
}
