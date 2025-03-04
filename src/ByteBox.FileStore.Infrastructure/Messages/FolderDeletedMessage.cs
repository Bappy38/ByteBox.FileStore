using NexaWrap.SQS.NET.Models;

namespace ByteBox.FileStore.Infrastructure.Messages;

public record FolderDeletedMessage : IMessage
{
    public Guid FolderId { get; set; }

    public string MessageTypeName => nameof(FolderDeletedMessage);

    public string? CorrelationId { get; set; }
}
