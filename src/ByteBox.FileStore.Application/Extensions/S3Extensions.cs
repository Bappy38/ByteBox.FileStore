using ByteBox.FileStore.Domain.Constants;

namespace ByteBox.FileStore.Application.Extensions;

public static class S3Extensions
{
    public static string GenerateFileKey(this Guid fileId)
    {
        return $"{Default.User.UserId}/resources/{fileId}";
    }

    public static string GenerateThumbnailKey(this Guid fileId)
    {
        return $"{Default.User.UserId}/thumbnails/{fileId}";
    }
}
