using Microsoft.EntityFrameworkCore.Migrations;

namespace Web_API.Migrations
{
    public partial class BoardUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "boardName",
                table: "leagues",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "jobTitle",
                table: "Job",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "boardName",
                table: "leagues");

            migrationBuilder.DropColumn(
                name: "jobTitle",
                table: "Job");
        }
    }
}
