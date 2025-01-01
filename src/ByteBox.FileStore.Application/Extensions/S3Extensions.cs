using ByteBox.FileStore.Domain.Constants;

namespace ByteBox.FileStore.Application.Extensions;

public static class S3Extensions
{
    public static string GenerateFileKey(this Guid fileId)
    {
        return $"resources/{Default.User.UserId}/{fileId}";
    }
}
