using ByteBox.FileStore.Application.Abstraction;
using ByteBox.FileStore.Domain.Constants;
using ByteBox.FileStore.Domain.Entities;
using ByteBox.FileStore.Domain.Enums;
using ByteBox.FileStore.Domain.Repositories;
using ByteBox.FileStore.Infrastructure.Data;

namespace ByteBox.FileStore.Application.Commands.Handlers;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
{
    // TODO:: Will move this to configuration
    private const int FreeStorageInMb = 1024;

    private readonly IUserRepository _userRepository;
    private readonly IDriveRepository _driveRepository;
    private readonly IFolderRepository _folderRepository;
    private readonly IFolderPermissionRepository _folderPermissionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IDriveRepository driveRepository,
        IFolderRepository folderRepository,
        IFolderPermissionRepository folderPermissionRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _driveRepository = driveRepository;
        _folderRepository = folderRepository;
        _folderPermissionRepository = folderPermissionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (!await _userRepository.IsUniqueEmail(request.Email))
        {
            throw new Exception($"User with email {request.Email} has already exist");
        }

        var user = new User
        {
            UserId = Guid.NewGuid(),
            UserName = request.UserName,
            Email = request.Email,
            ProfilePictureUrl = request.ProfilePictureUrl ?? string.Empty
        };
        await _userRepository.AddAsync(user);

        var drive = new Drive
        {
            DriveId = user.UserId,
            PurchasedStorageInMb = FreeStorageInMb,
            UsedStorageInMb = 0,
            NextBillDate = DateTime.UtcNow,
            OwnerId = user.UserId
        };
        await _driveRepository.AddAsync(drive);

        var folder = new Folder
        {
            FolderId = drive.DriveId,
            FolderName = "Root"
        };
        await _folderRepository.AddAsync(folder);

        var folderPermission = new FolderPermission
        {
            FolderId = folder.FolderId,
            UserId = user.UserId,
            AccessLevel = AccessLevel.Owner,
            GrantedAtUtc = DateTime.UtcNow
        };
        await _folderPermissionRepository.AddAsync(folderPermission);

        await _unitOfWork.SaveChangesAsync();
        return user.UserId;
    }
}
