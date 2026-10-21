using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Playtestr.Api.Migrations
{
    /// <inheritdoc />
    public partial class RenameResponseToSubmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Responses_ResponseId",
                table: "Answers");

            migrationBuilder.DropTable(
                name: "Responses");

            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SubmittedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TesterName = table.Column<string>(type: "TEXT", nullable: true),
                    PlaytestId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Submissions_Playtests_PlaytestId",
                        column: x => x.PlaytestId,
                        principalTable: "Playtests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_PlaytestId",
                table: "Submissions",
                column: "PlaytestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Submissions_ResponseId",
                table: "Answers",
                column: "ResponseId",
                principalTable: "Submissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Submissions_ResponseId",
                table: "Answers");

            migrationBuilder.DropTable(
                name: "Submissions");

            migrationBuilder.CreateTable(
                name: "Responses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlaytestId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TesterName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Responses_Playtests_PlaytestId",
                        column: x => x.PlaytestId,
                        principalTable: "Playtests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Responses_PlaytestId",
                table: "Responses",
                column: "PlaytestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Responses_ResponseId",
                table: "Answers",
                column: "ResponseId",
                principalTable: "Responses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
