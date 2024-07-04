using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineEducationWebApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class lesson_finish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FinishedAt",
                table: "Lessons",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Lessons",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishedAt",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Lessons");
        }
    }
}
