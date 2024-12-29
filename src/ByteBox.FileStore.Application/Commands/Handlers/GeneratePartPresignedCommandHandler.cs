using Amazon.S3;
using Amazon.S3.Model;
using ByteBox.FileStore.Application.Abstraction;
using ByteBox.FileStore.Application.Extensions;
using ByteBox.FileStore.Application.Responses;
using ByteBox.FileStore.Domain.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ByteBox.FileStore.Application.Commands.Handlers;

public class GeneratePartPresignedCommandHandler : ICommandHandler<GeneratePartPresignedCommand, GeneratePartPresignedCommandResponse>
{
    private readonly IAmazonS3 _s3Client;
    private readonly S3Settings _s3Settings;
    private ILogger<GeneratePartPresignedCommandHandler> _logger;

    public GeneratePartPresignedCommandHandler(
        IAmazonS3 s3Client,
        IOptions<S3Settings> s3Settings,
        ILogger<GeneratePartPresignedCommandHandler> logger)
    {
        _s3Client = s3Client;
        _s3Settings = s3Settings.Value;
        _logger = logger;
    }

    public async Task<GeneratePartPresignedCommandResponse> Handle(GeneratePartPresignedCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var getPreSignedUrlRequest = new GetPreSignedUrlRequest
            {
                BucketName = _s3Settings.BucketName,
                Key = request.FileId.GenerateFileKey(),
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddMinutes(15),   // TODO:: Will read it from config
                UploadId = request.UploadId,
                PartNumber = request.PartNumber
            };

            var preSignedUrl = await _s3Client.GetPreSignedURLAsync(getPreSignedUrlRequest);

            return new GeneratePartPresignedCommandResponse
            {
                FileId = request.FileId,
                PreSignedUrl = preSignedUrl
            };
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError(ex, "S3 error occurred while generating pre-signed URL for part: {ErrorMessage}", ex.Message);
            throw;
        }
    }
}
