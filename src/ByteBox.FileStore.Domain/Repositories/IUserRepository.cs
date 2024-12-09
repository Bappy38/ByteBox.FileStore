using ByteBox.FileStore.Domain.Entities;

namespace ByteBox.FileStore.Domain.Repositories;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<bool> IsUniqueEmail(string email);
}
