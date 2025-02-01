using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteBox.FileStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ThumbnailPresignedUrlVersionIdIsUploadCompletedpropsaddedatFileEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUploadCompleted",
                table: "Files",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailPresignedUrl",
                table: "Files",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "VersionId",
                table: "Files",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUploadCompleted",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ThumbnailPresignedUrl",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "VersionId",
                table: "Files");
        }
    }
}
