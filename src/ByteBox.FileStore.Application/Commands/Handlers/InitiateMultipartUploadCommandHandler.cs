using Amazon.S3;
using Amazon.S3.Model;
using ByteBox.FileStore.Application.Abstraction;
using ByteBox.FileStore.Application.Extensions;
using ByteBox.FileStore.Application.Responses;
using ByteBox.FileStore.Domain.Constants;
using ByteBox.FileStore.Domain.Repositories;
using ByteBox.FileStore.Domain.Utilities;
using ByteBox.FileStore.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ByteBox.FileStore.Application.Commands.Handlers;

public class InitiateMultipartUploadCommandHandler : ICommandHandler<InitiateMultipartUploadCommand, InitiateMultipartUploadCommandResponse>
{
    private readonly IAmazonS3 _s3Client;
    private readonly S3Settings _s3Settings;
    private readonly IDriveRepository _driveRepository;
    private readonly IFileRepository _fileRepository;
    private readonly ILogger<InitiateMultipartUploadCommandHandler> _logger;

    public InitiateMultipartUploadCommandHandler(
        IAmazonS3 s3Client,
        IOptions<S3Settings> s3Settings,
        IDriveRepository driveRepository,
        IFileRepository fileRepository,
        ILogger<InitiateMultipartUploadCommandHandler> logger)
    {
        _s3Client = s3Client;
        _s3Settings = s3Settings.Value;
        _driveRepository = driveRepository;
        _fileRepository = fileRepository;
        _logger = logger;
    }

    public async Task<InitiateMultipartUploadCommandResponse> Handle(InitiateMultipartUploadCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var drive = await _driveRepository.GetByIdAsync(Default.User.UserId);   //TODO:: Replace with real user
            if (!drive!.HaveSpace(request.FileSizeInMb))
            {
                throw new Exception("Storage limit exceed");
            }

            if (!await _fileRepository.IsUniqueFileName(request.FileName, request.FolderId))
            {
                throw new Exception($"File with name '{request.FileName}' already exist");
            }

            var fileId = Guid.NewGuid();
            var uploadRequest = new InitiateMultipartUploadRequest
            {
                BucketName = _s3Settings.BucketName,
                Key = fileId.GenerateFileKey(),
                ContentType = request.ContentType,
                Metadata =
                {
                    ["file-name"] = request.FileName,
                    ["folder-id"] = request.FolderId.ToString()
                }
            };

            var response = await _s3Client.InitiateMultipartUploadAsync(uploadRequest);
            return new InitiateMultipartUploadCommandResponse
            {
                FileId = fileId,
                UploadId = response.UploadId
            };
        }
        catch(AmazonS3Exception ex)
        {
            _logger.LogError(ex, "S3 error occurred while starting multipart upload: {ErrorMessage}", ex.Message);
            throw;
        }
    }
}
