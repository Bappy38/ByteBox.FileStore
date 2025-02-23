using ByteBox.FileStore.Application.Abstraction;
using ByteBox.FileStore.Domain.Repositories;
using ByteBox.FileStore.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace ByteBox.FileStore.Application.Commands.Handlers;

public class RefreshThumbnailPresignedCommandHandler : ICommandHandler<RefreshThumbnailPresignedCommand>
{
    private readonly IFileRepository _fileRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RefreshThumbnailPresignedCommandHandler> _logger;

    public RefreshThumbnailPresignedCommandHandler(IFileRepository fileRepository,
        IUnitOfWork unitOfWork,
        ILogger<RefreshThumbnailPresignedCommandHandler> logger)
    {
        _fileRepository = fileRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(RefreshThumbnailPresignedCommand request, CancellationToken cancellationToken)
    {
        var file = await _fileRepository.GetByIdAsync(request.FileId);

        if (file == null)
        {
            _logger.LogError("File with ID {FileId} not found", request.FileId);
            throw new Exception($"File with ID {request.FileId} not found");
        }

        if (!string.IsNullOrEmpty(request.ThumbnailPresignedUrl))
        {
            file.ThumbnailPresignedUrl = request.ThumbnailPresignedUrl;
            file.ThumbnailPresignedGeneratedAt = DateTime.UtcNow;
            await _fileRepository.UpdateAsync(file);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Thumbnail PreSignedUrl Updated for File ID: {FileId}", file.FileId);
            return;
        }

        // TODO:: Generate presigned url every 24 hour by a scheduled job.
    }
}
