using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteBox.FileStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FileLocationcolumnaddedinFileentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileLocation",
                table: "Files",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileLocation",
                table: "Files");
        }
    }
}
