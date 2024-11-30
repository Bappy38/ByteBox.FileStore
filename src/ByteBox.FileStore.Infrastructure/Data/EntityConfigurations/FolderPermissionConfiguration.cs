using ByteBox.FileStore.Domain.Constants;
using ByteBox.FileStore.Domain.Entities;
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
            .HasForeignKey(fp => fp.FolderId);

        builder
            .HasOne(fp => fp.User)
            .WithMany()
            .HasForeignKey(fp => fp.UserId);

        builder
            .HasOne(fp => fp.CreatedBy)
            .WithMany()
            .HasForeignKey(fp => fp.CreatedByUserId);

        builder
            .HasOne(fp => fp.UpdatedBy)
            .WithMany()
            .HasForeignKey(fp => fp.UpdatedByUserId);
    }
}
