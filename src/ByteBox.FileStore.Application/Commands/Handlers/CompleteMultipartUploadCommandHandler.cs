using Amazon.S3;
using Amazon.S3.Model;
using ByteBox.FileStore.Application.Abstraction;
using ByteBox.FileStore.Application.Extensions;
using ByteBox.FileStore.Domain.Constants;
using ByteBox.FileStore.Domain.Entities;
using ByteBox.FileStore.Domain.Enums;
using ByteBox.FileStore.Domain.Repositories;
using ByteBox.FileStore.Domain.Utilities;
using ByteBox.FileStore.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CompleteMultipartUploadResponse = ByteBox.FileStore.Application.Responses.CompleteMultipartUploadResponse;

namespace ByteBox.FileStore.Application.Commands.Handlers;

public class CompleteMultipartUploadCommandHandler : ICommandHandler<CompleteMultipartUploadCommand, Responses.CompleteMultipartUploadResponse>
{
    private const double FileSizeTolerance = 1.0;

    private readonly IAmazonS3 _s3Client;
    private readonly S3Settings _s3Settings;
    private readonly IDriveRepository _driveRepository;
    private readonly IFileRepository _fileRepository;
    private readonly IFilePermissionRepository _filePermissionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private ILogger<CompleteMultipartUploadCommandHandler> _logger;

    public CompleteMultipartUploadCommandHandler(
        IAmazonS3 s3Client,
        IOptions<S3Settings> s3Settings,
        IDriveRepository driveRepository,
        IFileRepository fileRepository,
        IFilePermissionRepository filePermissionRepository,
        IUnitOfWork unitOfWork,
        ILogger<CompleteMultipartUploadCommandHandler> logger)
    {
        _s3Client = s3Client;
        _s3Settings = s3Settings.Value;
        _driveRepository = driveRepository;
        _fileRepository = fileRepository;
        _filePermissionRepository = filePermissionRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<CompleteMultipartUploadResponse> Handle(CompleteMultipartUploadCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var file = await _fileRepository.GetByIdAsync(request.FileId);

            if (file == null)
            {
                throw new Exception("Upload failed");
            }

            await ValidateFileSizeAsync(request);
            var response = await CompleteMultipartUploadAsync(request, cancellationToken);

            file.IsUploadCompleted = true;
            file.FileLocation = response.Location;
            await _fileRepository.UpdateAsync(file);

            var filePermission = new FilePermission
            {
                FileId = file.FileId,
                UserId = Default.User.UserId,   //TODO:: Replace with real user
                AccessLevel = AccessLevel.Owner,
                GrantedAtUtc = DateTime.UtcNow
            };
            await _filePermissionRepository.AddAsync(filePermission);

            var drive = await _driveRepository.GetByIdAsync(Default.User.UserId);   //TODO:: Replace with real user
            drive!.AddFile(file);

            await _unitOfWork.SaveChangesAsync();

            return new CompleteMultipartUploadResponse
            {
                FileId = request.FileId,
                FileName = file.FileName,
                FileSizeInMb = file.FileSizeInMb,
                FileType = file.FileType,
                ThumbnailUrl = string.Empty // TODO:: Replace with default thumbnailUrl until real thumbnail generated
            };
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError(ex, "S3 error occurred while completing multipart upload: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    private async Task ValidateFileSizeAsync(CompleteMultipartUploadCommand request)
    {
        var listPartsRequest = new ListPartsRequest
        {
            BucketName = _s3Settings.BucketName,
            Key = request.FileId.GenerateFileKey(request.ContentType),
            UploadId = request.UploadId
        };
        var response = await _s3Client.ListPartsAsync(listPartsRequest);

        var uploadedFileSizeInByte = response.Parts.Sum(part => part.Size);
        var uploadedFileSizeInMb = uploadedFileSizeInByte / (1024.0 * 1024.0);
        if (Math.Abs(uploadedFileSizeInMb - request.FileSizeInMb) >= FileSizeTolerance)
        {
            throw new Exception("Upload Failed");
        }
    }

    private async Task<Amazon.S3.Model.CompleteMultipartUploadResponse> CompleteMultipartUploadAsync(CompleteMultipartUploadCommand request, CancellationToken ct)
    {
        var completeUploadRequest = new CompleteMultipartUploadRequest
        {
            BucketName = _s3Settings.BucketName,
            Key = request.FileId.GenerateFileKey(),
            UploadId = request.UploadId,
            PartETags = request.Parts.Select(p => new PartETag(p.PartNumber, p.ETag)).ToList()
        };

        return await _s3Client.CompleteMultipartUploadAsync(completeUploadRequest, ct);
    }
}
