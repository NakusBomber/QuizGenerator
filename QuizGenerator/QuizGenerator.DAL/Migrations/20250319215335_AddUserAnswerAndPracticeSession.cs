using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizGenerator.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAnswerAndPracticeSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PracticeSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    QuizId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PracticeSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PracticeSessions_Quizes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: true),
                    PracticeSessionId = table.Column<Guid>(type: "TEXT", nullable: true),
                    QuestionDetailId = table.Column<Guid>(type: "TEXT", nullable: true),
                    AnswerDetailId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAnswers_AnswerDetails_AnswerDetailId",
                        column: x => x.AnswerDetailId,
                        principalTable: "AnswerDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserAnswers_PracticeSessions_PracticeSessionId",
                        column: x => x.PracticeSessionId,
                        principalTable: "PracticeSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAnswers_QuestionDetails_QuestionDetailId",
                        column: x => x.QuestionDetailId,
                        principalTable: "QuestionDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PracticeSessions_QuizId",
                table: "PracticeSessions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_AnswerDetailId",
                table: "UserAnswers",
                column: "AnswerDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_PracticeSessionId",
                table: "UserAnswers",
                column: "PracticeSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_QuestionDetailId",
                table: "UserAnswers",
                column: "QuestionDetailId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAnswers");

            migrationBuilder.DropTable(
                name: "PracticeSessions");
        }
    }
}
