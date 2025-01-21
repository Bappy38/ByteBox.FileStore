using ByteBox.FileStore.Domain.Constants;
using ByteBox.FileStore.Domain.Entities;
using ByteBox.FileStore.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByteBox.FileStore.Infrastructure.Data.EntityConfigurations;

public class FolderPermissionConfiguration : IEntityTypeConfiguration<FolderPermission>
{
    public void Configure(EntityTypeBuilder<FolderPermission> builder)
    {
        builder
            .ToTable(Tables.FolderPermissions)
            .HasKey(fp => new { fp.FolderId, fp.UserId });

        builder
            .HasOne(fp => fp.Folder)
            .WithMany()
            .HasForeignKey(fp => fp.FolderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(fp => fp.User)
            .WithMany()
            .HasForeignKey(fp => fp.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(fp => fp.CreatedBy)
            .WithMany()
            .HasForeignKey(fp => fp.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(fp => fp.UpdatedBy)
            .WithMany()
            .HasForeignKey(fp => fp.UpdatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Property(fp => fp.IsDeleted)
            .HasDefaultValue(false);

        builder
            .HasQueryFilter(fp => !fp.IsDeleted);

        builder
            .HasData(GetSeedFolderPermission());
    }

    private static List<FolderPermission> GetSeedFolderPermission()
    {
        return new List<FolderPermission>
        {
            new FolderPermission
            {
                FolderId = Default.User.UserId,
                UserId = Default.User.UserId,
                AccessLevel = AccessLevel.Owner,
                GrantedAtUtc = Default.DateTime,
                CreatedAtUtc = Default.DateTime,
                CreatedByUserId = Default.User.UserId,
                IsDeleted = false
            }
        };
    }
}
