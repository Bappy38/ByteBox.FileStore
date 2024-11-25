using ByteBox.FileStore.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using File = ByteBox.FileStore.Domain.Entities.File;

namespace ByteBox.FileStore.Infrastructure.Data.EntityConfigurations;

public class FileConfiguration : IEntityTypeConfiguration<File>
{
    public void Configure(EntityTypeBuilder<File> builder)
    {
        builder.ToTable(Tables.Files);
        builder.HasKey(file => file.FileId);

        builder
            .HasOne(file => file.CreatedBy)
            .WithOne()
            .HasForeignKey<File>(folder => folder.CreatedByUserId);

        builder
            .HasOne(file => file.UpdatedBy)
            .WithOne()
            .HasForeignKey<File>(folder => folder.UpdatedByUserId);
    }
}
