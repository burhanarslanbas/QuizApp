using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixOptionRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_Options_OptionId1",
                table: "UserAnswers");

            migrationBuilder.DropIndex(
                name: "IX_UserAnswers_OptionId1",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "OptionId1",
                table: "UserAnswers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OptionId1",
                table: "UserAnswers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_OptionId1",
                table: "UserAnswers",
                column: "OptionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_Options_OptionId1",
                table: "UserAnswers",
                column: "OptionId1",
                principalTable: "Options",
                principalColumn: "Id");
        }
    }
}
