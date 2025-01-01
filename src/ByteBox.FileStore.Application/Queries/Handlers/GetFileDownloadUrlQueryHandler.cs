using Amazon.S3;
using Amazon.S3.Model;
using ByteBox.FileStore.Application.Abstraction;
using ByteBox.FileStore.Application.Extensions;
using ByteBox.FileStore.Application.Responses;
using ByteBox.FileStore.Domain.Constants;
using ByteBox.FileStore.Domain.Repositories;
using ByteBox.FileStore.Domain.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ByteBox.FileStore.Application.Queries.Handlers;

public class GetFileDownloadUrlQueryHandler : IQueryHandler<GetFileDownloadUrlQuery, GetFileDownloadUrlResponse>
{
    private readonly IAmazonS3 _s3Client;
    private readonly S3Settings _s3Settings;
    private readonly IFilePermissionRepository _filePermissionRepository;
    private ILogger<GetFileDownloadUrlQueryHandler> _logger;

    public GetFileDownloadUrlQueryHandler(
        IAmazonS3 s3Client,
        IOptions<S3Settings> s3Settings,
        IFilePermissionRepository filePermissionRepository,
        ILogger<GetFileDownloadUrlQueryHandler> logger)
    {
        _s3Client = s3Client;
        _s3Settings = s3Settings.Value;
        _filePermissionRepository = filePermissionRepository;
        _logger = logger;
    }

    public async Task<GetFileDownloadUrlResponse> Handle(GetFileDownloadUrlQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var filePermission = await _filePermissionRepository.GetAsync(request.FileId, Default.User.UserId); // TODO:: Will replace with real UserId;

            if (filePermission is null)
            {
                throw new Exception("File not found");
            }

            var getPreSignedUrlRequest = new GetPreSignedUrlRequest
            {
                BucketName = _s3Settings.BucketName,
                Key = request.FileId.GenerateFileKey(),
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddMinutes(15)    // TODO:: Will read it from config
            };

            var preSignedUrl = await _s3Client.GetPreSignedURLAsync(getPreSignedUrlRequest);

            return new GetFileDownloadUrlResponse
            {
                FileId = request.FileId,
                DownloadUrl = preSignedUrl
            };
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError(ex, "S3 error occurred while generating pre-signed URL for downloading file with FileId: {FileId} ErrorMessage: {ErrorMessage}", request.FileId, ex.Message);
            throw;
        }
    }
}
