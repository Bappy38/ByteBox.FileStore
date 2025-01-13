using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteBox.FileStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CascadingdeletebehaviordefinedbetweenFileandFilePermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilePermissions_Files_FileId",
                table: "FilePermissions");

            migrationBuilder.AddForeignKey(
                name: "FK_FilePermissions_Files_FileId",
                table: "FilePermissions",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "FileId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilePermissions_Files_FileId",
                table: "FilePermissions");

            migrationBuilder.AddForeignKey(
                name: "FK_FilePermissions_Files_FileId",
                table: "FilePermissions",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "FileId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
