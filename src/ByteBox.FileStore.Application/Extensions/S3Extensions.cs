using ByteBox.FileStore.Domain.Constants;
using ByteBox.FileStore.Domain.Entities;
using File = ByteBox.FileStore.Domain.Entities.File;

namespace ByteBox.FileStore.Application.Extensions;

public static class S3Extensions
{
    public static string GenerateFileKey(this Guid fileId, string mimeType)
    {
        var actualFileType = FileMimeTypes.GetActualFileType(mimeType);

        if (actualFileType == FileTypes.Unsupported)
        {
            throw new Exception("Unsupported file format");
        }

        return $"resources/{actualFileType}/{Default.User.UserId}/{fileId}";
    }

    public static string GetThumbnailLocation(this File file)
    {
        return file.FileLocation.Replace("resources", "thumbnails");
    }
}
