using ByteBox.FileStore.Application.Abstraction;
using ByteBox.FileStore.Domain.Constants;
using ByteBox.FileStore.Domain.Enums;
using ByteBox.FileStore.Domain.Repositories;
using ByteBox.FileStore.Infrastructure.Data;

namespace ByteBox.FileStore.Application.Commands.Handlers;

public class RestoreFileCommandHandler : ICommandHandler<RestoreFileCommand>
{
    private readonly IFileRepository _fileRepository;
    private readonly IFilePermissionRepository _filePermissionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RestoreFileCommandHandler(
        IFileRepository fileRepository,
        IFilePermissionRepository filePermissionRepository,
        IUnitOfWork unitOfWork)
    {
        _fileRepository = fileRepository;
        _filePermissionRepository = filePermissionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RestoreFileCommand request, CancellationToken cancellationToken)
    {
        // TODO:: Replace with real user id
        var filePermission = await _filePermissionRepository.GetAsync(request.FileId, Default.User.UserId);
        if (filePermission is null || filePermission.AccessLevel != AccessLevel.Owner)
        {
            throw new Exception("File not found");
        }

        var file = await _fileRepository.GetTrashedFileByIdAsync(request.FileId);
        if (file is null)
        {
            throw new Exception("File not found");
        }

        file.TrashedAt = null;
        await _fileRepository.UpdateAsync(file);
        await _unitOfWork.SaveChangesAsync();
    }
}
