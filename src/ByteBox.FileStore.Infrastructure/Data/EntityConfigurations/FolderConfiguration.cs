using ByteBox.FileStore.Domain.Constants;
using ByteBox.FileStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByteBox.FileStore.Infrastructure.Data.EntityConfigurations;

public class FolderConfiguration : IEntityTypeConfiguration<Folder>
{
    public void Configure(EntityTypeBuilder<Folder> builder)
    {
        builder
            .ToTable(Tables.Folders)
            .HasKey(folder => folder.FolderId);

        builder
            .HasMany(folder => folder.Files)
            .WithOne(file => file.Folder)
            .HasForeignKey(file => file.FolderId);

        builder
            .HasMany(folder => folder.SubFolders)
            .WithOne(subFolder => subFolder.ParentFolder)
            .HasForeignKey(subFolder => subFolder.ParentFolderId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(folder => folder.CreatedBy)
            .WithMany()
            .HasForeignKey(folder => folder.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(folder => folder.UpdatedBy)
            .WithMany()
            .HasForeignKey(folder => folder.UpdatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Property(folder => folder.IsDeleted)
            .HasDefaultValue(false);

        builder
            .Property(folder => folder.FolderSizeInMb)
            .HasDefaultValue(0.0);

        builder
            .HasQueryFilter(folder => !folder.IsDeleted);

        builder
            .HasData(GetSeedFolders());
    }

    private static List<Folder> GetSeedFolders()
    {
        return new List<Folder>
        {
            new Folder
            {
                FolderId = Default.User.UserId,
                FolderName = Default.Folder.RootFolderName,
                CreatedAtUtc = Default.DateTime,
                CreatedByUserId = Default.User.UserId,
                IsDeleted = false
            }
        };
    }
}
