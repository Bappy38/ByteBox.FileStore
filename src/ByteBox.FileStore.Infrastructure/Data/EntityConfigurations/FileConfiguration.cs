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
            .WithMany()
            .HasForeignKey(file => file.CreatedByUserId);

        builder
            .HasOne(file => file.UpdatedBy)
            .WithMany()
            .HasForeignKey(file => file.UpdatedByUserId);
    }
}
