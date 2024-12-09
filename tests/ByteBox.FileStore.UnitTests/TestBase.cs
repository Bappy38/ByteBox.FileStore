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
    }
}
