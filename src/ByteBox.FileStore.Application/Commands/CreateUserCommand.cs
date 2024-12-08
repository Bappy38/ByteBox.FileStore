using ByteBox.FileStore.Application.Abstraction;

namespace ByteBox.FileStore.Application.Commands;

public sealed record CreateUserCommand(
    string UserName,
    string Email,
    string? ProfilePictureUrl
) : ICommand<Guid>;