using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteBox.FileStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AncestorIdscolumnaddedinFolder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AncestorIds",
                table: "Folders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Folders",
                keyColumn: "FolderId",
                keyValue: new Guid("9a18b0b3-c515-412d-bef1-b609450de4c9"),
                column: "AncestorIds",
                value: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AncestorIds",
                table: "Folders");
        }
    }
}
