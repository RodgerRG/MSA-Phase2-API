using Microsoft.EntityFrameworkCore.Migrations;

namespace Web_API.Migrations
{
    public partial class Jobs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userLeague_leagues_leagueId",
                table: "userLeague");

            migrationBuilder.DropPrimaryKey(
                name: "PK_userLeague",
                table: "userLeague");

            migrationBuilder.DropIndex(
                name: "IX_userLeague_leagueId",
                table: "userLeague");

            migrationBuilder.DropPrimaryKey(
                name: "PK_leagues",
                table: "leagues");

            migrationBuilder.DropColumn(
                name: "leagueId",
                table: "userLeague");

            migrationBuilder.DropColumn(
                name: "leagueId",
                table: "leagues");

            migrationBuilder.AddColumn<int>(
                name: "membershipId",
                table: "userLeague",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "boardId",
                table: "userLeague",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rep",
                table: "userLeague",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "boardId",
                table: "leagues",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "location",
                table: "leagues",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_userLeague",
                table: "userLeague",
                column: "membershipId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_leagues",
                table: "leagues",
                column: "boardId");

            migrationBuilder.CreateTable(
                name: "Job",
                columns: table => new
                {
                    jobId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    posterId = table.Column<int>(nullable: false),
                    mediaURI = table.Column<string>(nullable: true),
                    jobDescription = table.Column<string>(nullable: false),
                    location = table.Column<string>(nullable: false),
                    isCompleted = table.Column<bool>(nullable: false),
                    boardId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job", x => x.jobId);
                    table.ForeignKey(
                        name: "FK_Job_leagues_boardId",
                        column: x => x.boardId,
                        principalTable: "leagues",
                        principalColumn: "boardId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Job_userLeague_posterId",
                        column: x => x.posterId,
                        principalTable: "userLeague",
                        principalColumn: "membershipId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_userLeague_boardId",
                table: "userLeague",
                column: "boardId");

            migrationBuilder.CreateIndex(
                name: "IX_userLeague_userId",
                table: "userLeague",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Job_boardId",
                table: "Job",
                column: "boardId");

            migrationBuilder.CreateIndex(
                name: "IX_Job_posterId",
                table: "Job",
                column: "posterId");

            migrationBuilder.AddForeignKey(
                name: "FK_userLeague_leagues_boardId",
                table: "userLeague",
                column: "boardId",
                principalTable: "leagues",
                principalColumn: "boardId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userLeague_leagues_boardId",
                table: "userLeague");

            migrationBuilder.DropTable(
                name: "Job");

            migrationBuilder.DropPrimaryKey(
                name: "PK_userLeague",
                table: "userLeague");

            migrationBuilder.DropIndex(
                name: "IX_userLeague_boardId",
                table: "userLeague");

            migrationBuilder.DropIndex(
                name: "IX_userLeague_userId",
                table: "userLeague");

            migrationBuilder.DropPrimaryKey(
                name: "PK_leagues",
                table: "leagues");

            migrationBuilder.DropColumn(
                name: "membershipId",
                table: "userLeague");

            migrationBuilder.DropColumn(
                name: "boardId",
                table: "userLeague");

            migrationBuilder.DropColumn(
                name: "rep",
                table: "userLeague");

            migrationBuilder.DropColumn(
                name: "boardId",
                table: "leagues");

            migrationBuilder.DropColumn(
                name: "location",
                table: "leagues");

            migrationBuilder.AddColumn<int>(
                name: "leagueId",
                table: "userLeague",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "leagueId",
                table: "leagues",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_userLeague",
                table: "userLeague",
                columns: new[] { "userId", "leagueId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_leagues",
                table: "leagues",
                column: "leagueId");

            migrationBuilder.CreateIndex(
                name: "IX_userLeague_leagueId",
                table: "userLeague",
                column: "leagueId");

            migrationBuilder.AddForeignKey(
                name: "FK_userLeague_leagues_leagueId",
                table: "userLeague",
                column: "leagueId",
                principalTable: "leagues",
                principalColumn: "leagueId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
