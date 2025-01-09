﻿// <auto-generated />
using System;
using ByteBox.FileStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ByteBox.FileStore.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250109130118_TrashedAt column added in File")]
    partial class TrashedAtcolumnaddedinFile
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ByteBox.FileStore.Domain.Entities.Drive", b =>
                {
                    b.Property<Guid>("DriveId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<DateTime?>("NextBillDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("PurchasedStorageInMb")
                        .HasColumnType("float");

                    b.Property<double>("UsedStorageInMb")
                        .HasColumnType("float");

                    b.HasKey("DriveId");

                    b.HasIndex("OwnerId")
                        .IsUnique();

                    b.ToTable("Drives", (string)null);

                    b.HasData(
                        new
                        {
                            DriveId = new Guid("9a18b0b3-c515-412d-bef1-b609450de4c9"),
                            IsDeleted = false,
                            NextBillDate = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            OwnerId = new Guid("9a18b0b3-c515-412d-bef1-b609450de4c9"),
                            PurchasedStorageInMb = 1024.0,
                            UsedStorageInMb = 0.0
                        });
                });

            modelBuilder.Entity("ByteBox.FileStore.Domain.Entities.File", b =>
                {
                    b.Property<Guid>("FileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FileLocation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("FileSizeInMb")
                        .HasColumnType("float");

                    b.Property<string>("FileType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("FolderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<DateTime?>("TrashedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UpdatedAtUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UpdatedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("FileId");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("FolderId");

                    b.HasIndex("UpdatedByUserId");

                    b.ToTable("Files", (string)null);
                });

            modelBuilder.Entity("ByteBox.FileStore.Domain.Entities.FilePermission", b =>
                {
                    b.Property<Guid>("FileId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessLevel")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("GrantedAtUtc")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<DateTime?>("UpdatedAtUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UpdatedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("FileId", "UserId");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("UpdatedByUserId");

                    b.HasIndex("UserId");

                    b.ToTable("FilePermissions", (string)null);
                });

            modelBuilder.Entity("ByteBox.FileStore.Domain.Entities.Folder", b =>
                {
                    b.Property<Guid>("FolderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FolderName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("FolderSizeInMb")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("float")
                        .HasDefaultValue(0.0);

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<Guid?>("ParentFolderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdatedAtUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UpdatedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("FolderId");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("ParentFolderId");

                    b.HasIndex("UpdatedByUserId");

                    b.ToTable("Folders", (string)null);

                    b.HasData(
                        new
                        {
                            FolderId = new Guid("9a18b0b3-c515-412d-bef1-b609450de4c9"),
                            CreatedAtUtc = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            CreatedByUserId = new Guid("9a18b0b3-c515-412d-bef1-b609450de4c9"),
                            FolderName = "Root",
                            FolderSizeInMb = 0.0,
                            IsDeleted = false
                        });
                });

            modelBuilder.Entity("ByteBox.FileStore.Domain.Entities.FolderPermission", b =>
                {
                    b.Property<Guid>("FolderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessLevel")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("GrantedAtUtc")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<DateTime?>("UpdatedAtUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UpdatedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("FolderId", "UserId");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("UpdatedByUserId");

                    b.HasIndex("UserId");

                    b.ToTable("FolderPermissions", (string)null);

                    b.HasData(
                        new
                        {
                            FolderId = new Guid("9a18b0b3-c515-412d-bef1-b609450de4c9"),
                            UserId = new Guid("9a18b0b3-c515-412d-bef1-b609450de4c9"),
                            AccessLevel = 2,
                            CreatedAtUtc = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            CreatedByUserId = new Guid("9a18b0b3-c515-412d-bef1-b609450de4c9"),
                            GrantedAtUtc = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            IsDeleted = false
                        });
                });

            modelBuilder.Entity("ByteBox.FileStore.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("ProfilePictureUrl")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = new Guid("9a18b0b3-c515-412d-bef1-b609450de4c9"),
                            Email = "default.user@bytebox.com",
                            IsDeleted = false,
                            ProfilePictureUrl = "",
                            UserName = "Default User"
                        });
                });

            modelBuilder.Entity("ByteBox.FileStore.Domain.Entities.Drive", b =>
                {
                    b.HasOne("ByteBox.FileStore.Domain.Entities.User", "Owner")
                        .WithOne()
                        .HasForeignKey("ByteBox.FileStore.Domain.Entities.Drive", "OwnerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("ByteBox.FileStore.Domain.Entities.File", b =>
                {
                    b.HasOne("ByteBox.FileStore.Domain.Entities.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ByteBox.FileStore.Domain.Entities.Folder", "Folder")
                        .WithMany("Files")
                        .HasForeignKey("FolderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ByteBox.FileStore.Domain.Entities.User", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("CreatedBy");

                    b.Navigation("Folder");

                    b.Navigation("UpdatedBy");
                });

            modelBuilder.Entity("ByteBox.FileStore.Domain.Entities.FilePermission", b =>
                {
                    b.HasOne("ByteBox.FileStore.Domain.Entities.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ByteBox.FileStore.Domain.Entities.File", "File")
                        .WithMany()
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ByteBox.FileStore.Domain.Entities.User", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ByteBox.FileStore.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CreatedBy");

                    b.Navigation("File");

                    b.Navigation("UpdatedBy");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ByteBox.FileStore.Domain.Entities.Folder", b =>
                {
                    b.HasOne("ByteBox.FileStore.Domain.Entities.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ByteBox.FileStore.Domain.Entities.Folder", "ParentFolder")
                        .WithMany("SubFolders")
                        .HasForeignKey("ParentFolderId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ByteBox.FileStore.Domain.Entities.User", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("CreatedBy");

                    b.Navigation("ParentFolder");

                    b.Navigation("UpdatedBy");
                });

            modelBuilder.Entity("ByteBox.FileStore.Domain.Entities.FolderPermission", b =>
                {
                    b.HasOne("ByteBox.FileStore.Domain.Entities.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ByteBox.FileStore.Domain.Entities.Folder", "Folder")
                        .WithMany()
                        .HasForeignKey("FolderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ByteBox.FileStore.Domain.Entities.User", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ByteBox.FileStore.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CreatedBy");

                    b.Navigation("Folder");

                    b.Navigation("UpdatedBy");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ByteBox.FileStore.Domain.Entities.Folder", b =>
                {
                    b.Navigation("Files");

                    b.Navigation("SubFolders");
                });
#pragma warning restore 612, 618
        }
    }
}
