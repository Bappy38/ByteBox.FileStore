using NexaWrap.SQS.NET.Models;

namespace ByteBox.FileStore.Infrastructure.Messages;

public record RefreshFolderMessage : IMessage
{
    public Guid FolderId { get; set; }

    public string MessageTypeName => nameof(RefreshFolderMessage);

    public string? CorrelationId { get; set; }
}
