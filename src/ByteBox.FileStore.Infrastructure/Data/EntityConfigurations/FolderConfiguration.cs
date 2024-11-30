﻿using ByteBox.FileStore.Domain.Constants;
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
            .HasForeignKey(f => f.FolderId);

        builder
            .HasMany(folder => folder.SubFolders)
            .WithOne(subFolder => subFolder.ParentFolder)
            .HasForeignKey(subFolder => subFolder.ParentFolderId)
            .IsRequired(false);

        builder
            .HasOne(folder => folder.CreatedBy)
            .WithMany()
            .HasForeignKey(folder => folder.CreatedByUserId);

        builder
            .HasOne(folder => folder.UpdatedBy)
            .WithMany()
            .HasForeignKey(folder => folder.UpdatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Property(folder => folder.IsDeleted)
            .HasDefaultValue(false);
    }
}
