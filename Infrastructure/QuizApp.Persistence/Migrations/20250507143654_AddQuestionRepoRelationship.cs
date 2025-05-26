using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddQuestionRepoRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionRepos_QuestionRepoId",
                table: "Questions");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionRepos_QuestionRepoId",
                table: "Questions",
                column: "QuestionRepoId",
                principalTable: "QuestionRepos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionRepos_QuestionRepoId",
                table: "Questions");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionRepos_QuestionRepoId",
                table: "Questions",
                column: "QuestionRepoId",
                principalTable: "QuestionRepos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
