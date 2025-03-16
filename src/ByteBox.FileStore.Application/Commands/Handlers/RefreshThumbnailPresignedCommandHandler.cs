using Amazon.S3;
using Amazon.S3.Model;
using ByteBox.FileStore.Application.Abstraction;
using ByteBox.FileStore.Application.Extensions;
using ByteBox.FileStore.Domain.Repositories;
using ByteBox.FileStore.Domain.Utilities;
using ByteBox.FileStore.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ByteBox.FileStore.Application.Commands.Handlers;

public class RefreshThumbnailPresignedCommandHandler : ICommandHandler<RefreshThumbnailPresignedCommand>
{
    private readonly IAmazonS3 _s3Client;
    private readonly S3Settings _s3Settings;
    private readonly IFileRepository _fileRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RefreshThumbnailPresignedCommandHandler> _logger;

    public RefreshThumbnailPresignedCommandHandler(
        IAmazonS3 s3Client,
        IOptions<S3Settings> s3Settings,
        IFileRepository fileRepository,
        IUnitOfWork unitOfWork,
        ILogger<RefreshThumbnailPresignedCommandHandler> logger)
    {
        _s3Client = s3Client;
        _s3Settings = s3Settings.Value;
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

        var getPreSignedUrlRequest = new GetPreSignedUrlRequest
        {
            BucketName = _s3Settings.BucketName,
            Key = file.GenerateThumbnailKey(),
            Verb = HttpVerb.GET,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        var preSignedUrl = await _s3Client.GetPreSignedURLAsync(getPreSignedUrlRequest);

        file.ThumbnailPresignedUrl = preSignedUrl;
        file.ThumbnailPresignedGeneratedAt = DateTime.UtcNow;

        await _fileRepository.UpdateAsync(file);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Thumbnail PreSignedUrl Refreshed for File ID: {FileId}", file.FileId);
    }
}
