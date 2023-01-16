using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class _261120211650 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AnswersBlockId",
                table: "SurveyAnswers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "SurveyAnswersBlocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyAnswersBlocks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SurveyAnswers_AnswersBlockId",
                table: "SurveyAnswers",
                column: "AnswersBlockId");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyAnswers_SurveyAnswersBlocks_AnswersBlockId",
                table: "SurveyAnswers",
                column: "AnswersBlockId",
                principalTable: "SurveyAnswersBlocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyAnswers_SurveyAnswersBlocks_AnswersBlockId",
                table: "SurveyAnswers");

            migrationBuilder.DropTable(
                name: "SurveyAnswersBlocks");

            migrationBuilder.DropIndex(
                name: "IX_SurveyAnswers_AnswersBlockId",
                table: "SurveyAnswers");

            migrationBuilder.DropColumn(
                name: "AnswersBlockId",
                table: "SurveyAnswers");
        }
    }
}
