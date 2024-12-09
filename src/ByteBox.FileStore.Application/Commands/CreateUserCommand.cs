using ByteBox.FileStore.Application.Abstraction;

namespace ByteBox.FileStore.Application.Commands;

public sealed record CreateUserCommand : ICommand<Guid>
{
    public string UserName { get; init; }
    public string Email { get; init; }
    public string ProfilePictureUrl { get; init; }
}