using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteBox.FileStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TrashedAtcolumnaddedinfoldertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TrashedAt",
                table: "Folders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Folders",
                keyColumn: "FolderId",
                keyValue: new Guid("9a18b0b3-c515-412d-bef1-b609450de4c9"),
                column: "TrashedAt",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrashedAt",
                table: "Folders");
        }
    }
}
