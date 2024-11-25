using ByteBox.FileStore.Domain.Constants;
using ByteBox.FileStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByteBox.FileStore.Infrastructure.Data.EntityConfigurations;

public class FolderConfiguration : IEntityTypeConfiguration<Folder>
{
    public void Configure(EntityTypeBuilder<Folder> builder)
    {
        builder.ToTable(Tables.Folders);
        builder.HasKey(folder => folder.FolderId);

        builder
            .HasMany(folder => folder.Files)
            .WithOne(file => file.Folder)
            .HasForeignKey(f => f.FolderId);

        builder
            .HasMany(folder => folder.SubFolders)
            .WithOne(subFolder => subFolder.ParentFolder)
            .HasForeignKey(subFolder => subFolder.ParentFolderId)
            .IsRequired(false);

        builder
            .HasOne(folder => folder.CreatedBy)
            .WithOne()
            .HasForeignKey<Folder>(folder => folder.CreatedByUserId);

        builder
            .HasOne(folder => folder.UpdatedBy)
            .WithOne()
            .HasForeignKey<Folder>(folder => folder.UpdatedByUserId);
    }
}
