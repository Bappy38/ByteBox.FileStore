using Amazon.S3;
using Amazon.S3.Model;
using ByteBox.FileStore.Application.Abstraction;
using ByteBox.FileStore.Application.Extensions;
using ByteBox.FileStore.Application.Responses;
using ByteBox.FileStore.Domain.Constants;
using ByteBox.FileStore.Domain.Entities;
using ByteBox.FileStore.Domain.Enums;
using ByteBox.FileStore.Domain.Repositories;
using ByteBox.FileStore.Domain.Utilities;
using ByteBox.FileStore.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using File = ByteBox.FileStore.Domain.Entities.File;

namespace ByteBox.FileStore.Application.Commands.Handlers;

public class CompleteMultipartUploadCommandHandler : ICommandHandler<CompleteMultipartUploadCommand, CompleteMultipartUploadCommandResponse>
{
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

    public async Task<CompleteMultipartUploadCommandResponse> Handle(CompleteMultipartUploadCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var completeUploadRequest = new CompleteMultipartUploadRequest
            {
                BucketName = _s3Settings.BucketName,
                Key = request.FileId.GenerateFileKey(),
                UploadId = request.UploadId,
                PartETags = request.Parts.Select(p => new PartETag(p.PartNumber, p.ETag)).ToList()
            };

            var response = await _s3Client.CompleteMultipartUploadAsync(completeUploadRequest, cancellationToken);

            var file = new File
            {
                FileId = request.FileId,
                FileName = request.FileName,
                FileSizeInMb = request.FileSizeInMb,
                FileType = request.ContentType,
                FileLocation = response.Location,
                FolderId = request.FolderId
            };
            await _fileRepository.AddAsync(file);

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

            return new CompleteMultipartUploadCommandResponse
            {
                FileId = request.FileId,
                Location = response.Location
            };
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError(ex, "S3 error occurred while completing multipart upload: {ErrorMessage}", ex.Message);
            throw;
        }
    }
}
