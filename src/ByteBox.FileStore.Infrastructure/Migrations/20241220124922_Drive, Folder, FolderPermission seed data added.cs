using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteBox.FileStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DriveFolderFolderPermissionseeddataadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Drives",
                columns: new[] { "DriveId", "NextBillDate", "OwnerId", "PurchasedStorageInMb", "UsedStorageInMb" },
                values: new object[] { new Guid("9a18b0b3-c515-412d-bef1-b609450de4c9"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("9a18b0b3-c515-412d-bef1-b609450de4c9"), 1024.0, 0.0 });

            migrationBuilder.InsertData(
                table: "Folders",
                columns: new[] { "FolderId", "CreatedAtUtc", "CreatedByUserId", "FolderName", "ParentFolderId", "UpdatedAtUtc", "UpdatedByUserId" },
                values: new object[] { new Guid("9a18b0b3-c515-412d-bef1-b609450de4c9"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), "Root", null, null, null });

            migrationBuilder.InsertData(
                table: "FolderPermissions",
                columns: new[] { "FolderId", "UserId", "AccessLevel", "CreatedAtUtc", "CreatedByUserId", "GrantedAtUtc", "UpdatedAtUtc", "UpdatedByUserId" },
                values: new object[] { new Guid("9a18b0b3-c515-412d-bef1-b609450de4c9"), new Guid("9a18b0b3-c515-412d-bef1-b609450de4c9"), 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Drives",
                keyColumn: "DriveId",
                keyValue: new Guid("9a18b0b3-c515-412d-bef1-b609450de4c9"));

            migrationBuilder.DeleteData(
                table: "FolderPermissions",
                keyColumns: new[] { "FolderId", "UserId" },
                keyValues: new object[] { new Guid("9a18b0b3-c515-412d-bef1-b609450de4c9"), new Guid("9a18b0b3-c515-412d-bef1-b609450de4c9") });

            migrationBuilder.DeleteData(
                table: "Folders",
                keyColumn: "FolderId",
                keyValue: new Guid("9a18b0b3-c515-412d-bef1-b609450de4c9"));
        }
    }
}
