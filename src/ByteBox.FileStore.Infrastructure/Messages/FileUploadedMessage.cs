using NexaWrap.SQS.NET.Models;

namespace ByteBox.FileStore.Infrastructure.Messages;

public record FileUploadedMessage : IMessage
{
    public Guid FileId { get; init; }
    public double FileSizeInMb { get; init; }

    public Guid FolderId { get; init; }

    public string MessageTypeName => nameof(FileUploadedMessage);

    public string? CorrelationId { get; set; }
}
