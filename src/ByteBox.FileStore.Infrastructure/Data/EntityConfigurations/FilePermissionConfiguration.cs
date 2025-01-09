using ByteBox.FileStore.Domain.Constants;
using ByteBox.FileStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByteBox.FileStore.Infrastructure.Data.EntityConfigurations;

public class FilePermissionConfiguration : IEntityTypeConfiguration<FilePermission>
{
    public void Configure(EntityTypeBuilder<FilePermission> builder)
    {
        builder
            .ToTable(Tables.FilePermissions)
            .HasKey(fp => new { fp.FileId, fp.UserId });

        builder
            .HasOne(fp => fp.File)
            .WithMany()
            .HasForeignKey(fp => fp.FileId)
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
    }
}
