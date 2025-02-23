namespace ByteBox.FileStore.Domain.Constants;

public class FileMimeTypes
{
    private const string JpegImage = "image/jpeg";
    private const string PngImage = "image/png";

    private const string Pdf = "application/pdf";

    private const string Mp4Video = "video/mp4";

    private static readonly HashSet<string> ImageMimeTypes = [JpegImage, PngImage];
    private static readonly HashSet<string> PdfMimeTypes = [Pdf];
    private static readonly HashSet<string> VideoMimeTypes = [Mp4Video];

    public static string GetActualFileType(string mimeType)
    {
        if (ImageMimeTypes.Contains(mimeType))
        {
            return FileTypes.Image;
        }

        if (PdfMimeTypes.Contains(mimeType))
        {
            return FileTypes.Pdf;
        }

        if (VideoMimeTypes.Contains(mimeType))
        {
            return FileTypes.Video;
        }

        return FileTypes.Unsupported;
    }
}
