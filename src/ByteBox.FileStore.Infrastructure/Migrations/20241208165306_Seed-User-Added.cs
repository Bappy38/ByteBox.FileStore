using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ByteBox.FileStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedUserAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProfilePictureUrl",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "ProfilePictureUrl", "UserName" },
                values: new object[] { new Guid("9a18b0b3-c515-412d-bef1-b609450de4c9"), "default.user@bytebox.com", "", "Default User" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("9a18b0b3-c515-412d-bef1-b609450de4c9"));

            migrationBuilder.AlterColumn<string>(
                name: "ProfilePictureUrl",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "");
        }
    }
}
