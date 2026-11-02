using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Playtestr.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddDatabaseConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Submissions_PlaytestId",
                table: "Submissions");

            migrationBuilder.DropIndex(
                name: "IX_Answers_SubmissionId",
                table: "Answers");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:citext", ",,");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "citext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Playtests",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Playtests",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Games",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Games",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Answers",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_PlaytestId_UserId",
                table: "Submissions",
                columns: new[] { "PlaytestId", "UserId" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Questions_Type",
                table: "Questions",
                sql: "\"Type\" IN (0, 1)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Games_ReleaseYear",
                table: "Games",
                sql: "\"ReleaseYear\" BETWEEN 1950 AND 2100");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_SubmissionId_QuestionId",
                table: "Answers",
                columns: new[] { "SubmissionId", "QuestionId" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Answers_Rating",
                table: "Answers",
                sql: "\"Rating\" BETWEEN 1 AND 5");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Answers_RatingOrText",
                table: "Answers",
                sql: "(\"Rating\" IS NULL) <> (\"Text\" IS NULL)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Submissions_PlaytestId_UserId",
                table: "Submissions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Questions_Type",
                table: "Questions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Games_ReleaseYear",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Answers_SubmissionId_QuestionId",
                table: "Answers");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Answers_Rating",
                table: "Answers");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Answers_RatingOrText",
                table: "Answers");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:citext", ",,");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "citext");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Playtests",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Playtests",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Games",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Games",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Answers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_PlaytestId",
                table: "Submissions",
                column: "PlaytestId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_SubmissionId",
                table: "Answers",
                column: "SubmissionId");
        }
    }
}
