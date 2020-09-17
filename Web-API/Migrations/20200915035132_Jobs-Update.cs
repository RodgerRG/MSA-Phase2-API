using Microsoft.EntityFrameworkCore.Migrations;

namespace Web_API.Migrations
{
    public partial class JobsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Job_userLeague_posterId",
                table: "Job");

            migrationBuilder.DropForeignKey(
                name: "FK_userLeague_leagues_boardId",
                table: "userLeague");

            migrationBuilder.DropForeignKey(
                name: "FK_userLeague_AspNetUsers_userId",
                table: "userLeague");

            migrationBuilder.DropPrimaryKey(
                name: "PK_userLeague",
                table: "userLeague");

            migrationBuilder.RenameTable(
                name: "userLeague",
                newName: "userBoard");

            migrationBuilder.RenameIndex(
                name: "IX_userLeague_userId",
                table: "userBoard",
                newName: "IX_userBoard_userId");

            migrationBuilder.RenameIndex(
                name: "IX_userLeague_boardId",
                table: "userBoard",
                newName: "IX_userBoard_boardId");

            migrationBuilder.AddColumn<bool>(
                name: "isTaken",
                table: "Job",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "hasJob",
                table: "userBoard",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_userBoard",
                table: "userBoard",
                column: "membershipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Job_userBoard_posterId",
                table: "Job",
                column: "posterId",
                principalTable: "userBoard",
                principalColumn: "membershipId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_userBoard_leagues_boardId",
                table: "userBoard",
                column: "boardId",
                principalTable: "leagues",
                principalColumn: "boardId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_userBoard_AspNetUsers_userId",
                table: "userBoard",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Job_userBoard_posterId",
                table: "Job");

            migrationBuilder.DropForeignKey(
                name: "FK_userBoard_leagues_boardId",
                table: "userBoard");

            migrationBuilder.DropForeignKey(
                name: "FK_userBoard_AspNetUsers_userId",
                table: "userBoard");

            migrationBuilder.DropPrimaryKey(
                name: "PK_userBoard",
                table: "userBoard");

            migrationBuilder.DropColumn(
                name: "isTaken",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "hasJob",
                table: "userBoard");

            migrationBuilder.RenameTable(
                name: "userBoard",
                newName: "userLeague");

            migrationBuilder.RenameIndex(
                name: "IX_userBoard_userId",
                table: "userLeague",
                newName: "IX_userLeague_userId");

            migrationBuilder.RenameIndex(
                name: "IX_userBoard_boardId",
                table: "userLeague",
                newName: "IX_userLeague_boardId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_userLeague",
                table: "userLeague",
                column: "membershipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Job_userLeague_posterId",
                table: "Job",
                column: "posterId",
                principalTable: "userLeague",
                principalColumn: "membershipId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_userLeague_leagues_boardId",
                table: "userLeague",
                column: "boardId",
                principalTable: "leagues",
                principalColumn: "boardId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_userLeague_AspNetUsers_userId",
                table: "userLeague",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
