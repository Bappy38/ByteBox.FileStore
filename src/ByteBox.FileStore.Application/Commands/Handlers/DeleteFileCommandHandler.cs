using ByteBox.FileStore.Application.Abstraction;
using ByteBox.FileStore.Domain.Repositories;
using ByteBox.FileStore.Infrastructure.Data;

namespace ByteBox.FileStore.Application.Commands.Handlers;

public class DeleteFileCommandHandler : ICommandHandler<DeleteFileCommand>
{
    private readonly IFileRepository _fileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteFileCommandHandler(
        IFileRepository fileRepository,
        IUnitOfWork unitOfWork)
    {
        _fileRepository = fileRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        var file = await _fileRepository.GetByIdAsync(request.FileId);
        if (file is null)
        {
            throw new Exception($"File with ID {request.FileId} not found");
        }

        file.MoveToTrash();
        await _fileRepository.UpdateAsync(file);
        await _unitOfWork.SaveChangesAsync();
    }
}
