using ByteBox.FileStore.Domain.Constants;
using ByteBox.FileStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByteBox.FileStore.Infrastructure.Data.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .ToTable(Tables.Users)
            .HasKey(u => u.UserId);

        builder
            .Property(u => u.IsDeleted)
            .HasDefaultValue(false);

        builder
            .Property(u => u.ProfilePictureUrl)
            .HasDefaultValue(string.Empty);

        builder
            .HasData(GetSeedUsers());
    }

    private static List<User> GetSeedUsers()
    {
        return new List<User>
        {
            new User
            {
                UserId = Default.User.UserId,
                UserName = Default.User.UserName,
                Email = Default.User.Email
            }
        };
    }
}
