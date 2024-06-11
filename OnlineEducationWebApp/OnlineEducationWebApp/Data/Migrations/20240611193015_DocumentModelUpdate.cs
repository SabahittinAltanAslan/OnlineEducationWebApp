using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineEducationWebApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class DocumentModelUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Documents",
                newName: "OriginalFileName");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Documents");

            migrationBuilder.RenameColumn(
                name: "OriginalFileName",
                table: "Documents",
                newName: "Name");
        }
    }
}
