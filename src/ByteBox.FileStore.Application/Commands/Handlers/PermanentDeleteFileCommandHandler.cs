using ByteBox.FileStore.Application.Abstraction;
using ByteBox.FileStore.Domain.Constants;
using ByteBox.FileStore.Domain.Repositories;
using ByteBox.FileStore.Infrastructure.Data;

namespace ByteBox.FileStore.Application.Commands.Handlers;

public class PermanentDeleteFileCommandHandler : ICommandHandler<PermanentDeleteFileCommand>
{
    private readonly IFileRepository _fileRepository;
    private readonly IFilePermissionRepository _filePermissionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PermanentDeleteFileCommandHandler(
        IFileRepository fileRepository,
        IFilePermissionRepository filePermissionRepository,
        IUnitOfWork unitOfWork)
    {
        _fileRepository = fileRepository;
        _filePermissionRepository = filePermissionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(PermanentDeleteFileCommand request, CancellationToken cancellationToken)
    {
        // TODO:: Replace with real userId
        var filePermission = await _filePermissionRepository.GetAsync(request.FileId, Default.User.UserId);
        if (filePermission is null)
        {
            throw new Exception("File not found");
        }

        var file = await _fileRepository.GetTrashedFileByIdAsync(request.FileId);
        if (file is null)
        {
            throw new Exception("File not found");
        }

        await _fileRepository.RemoveAsync(file);
        await _unitOfWork.SaveChangesAsync();
    }
}
