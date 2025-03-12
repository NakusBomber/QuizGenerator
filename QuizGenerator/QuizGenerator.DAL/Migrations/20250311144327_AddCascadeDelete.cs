using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizGenerator.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			// SQLite does not support executing certain operations,
            // like PRAGMA foreign_keys = 0, within a transaction
			migrationBuilder.Sql("PRAGMA foreign_keys = 0", suppressTransaction: true);

            migrationBuilder.DropForeignKey(
                name: "FK_AnswerDetails_QuestionDetails_QuestionDetailId",
                table: "AnswerDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionDetails_Questions_QuestionId",
                table: "QuestionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Quizes_QuizId",
                table: "Questions");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerDetails_QuestionDetails_QuestionDetailId",
                table: "AnswerDetails",
                column: "QuestionDetailId",
                principalTable: "QuestionDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionDetails_Questions_QuestionId",
                table: "QuestionDetails",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Quizes_QuizId",
                table: "Questions",
                column: "QuizId",
                principalTable: "Quizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerDetails_QuestionDetails_QuestionDetailId",
                table: "AnswerDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionDetails_Questions_QuestionId",
                table: "QuestionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Quizes_QuizId",
                table: "Questions");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerDetails_QuestionDetails_QuestionDetailId",
                table: "AnswerDetails",
                column: "QuestionDetailId",
                principalTable: "QuestionDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionDetails_Questions_QuestionId",
                table: "QuestionDetails",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Quizes_QuizId",
                table: "Questions",
                column: "QuizId",
                principalTable: "Quizes",
                principalColumn: "Id");
        }
    }
}
