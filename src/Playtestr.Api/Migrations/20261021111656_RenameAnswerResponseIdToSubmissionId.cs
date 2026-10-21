using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Playtestr.Api.Migrations
{
    /// <inheritdoc />
    public partial class RenameAnswerResponseIdToSubmissionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Submissions_ResponseId",
                table: "Answers");

            migrationBuilder.RenameColumn(
                name: "ResponseId",
                table: "Answers",
                newName: "SubmissionId");

            migrationBuilder.RenameIndex(
                name: "IX_Answers_ResponseId",
                table: "Answers",
                newName: "IX_Answers_SubmissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Submissions_SubmissionId",
                table: "Answers",
                column: "SubmissionId",
                principalTable: "Submissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Submissions_SubmissionId",
                table: "Answers");

            migrationBuilder.RenameColumn(
                name: "SubmissionId",
                table: "Answers",
                newName: "ResponseId");

            migrationBuilder.RenameIndex(
                name: "IX_Answers_SubmissionId",
                table: "Answers",
                newName: "IX_Answers_ResponseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Submissions_ResponseId",
                table: "Answers",
                column: "ResponseId",
                principalTable: "Submissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
