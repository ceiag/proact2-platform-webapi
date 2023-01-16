using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class _301120211243 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyAnswers_SurveyAnswersBlocks_AnswersBlockId",
                table: "SurveyAnswers");

            migrationBuilder.AlterColumn<Guid>(
                name: "AnswersBlockId",
                table: "SurveyAnswers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyAnswers_SurveyAnswersBlocks_AnswersBlockId",
                table: "SurveyAnswers",
                column: "AnswersBlockId",
                principalTable: "SurveyAnswersBlocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyAnswers_SurveyAnswersBlocks_AnswersBlockId",
                table: "SurveyAnswers");

            migrationBuilder.AlterColumn<Guid>(
                name: "AnswersBlockId",
                table: "SurveyAnswers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyAnswers_SurveyAnswersBlocks_AnswersBlockId",
                table: "SurveyAnswers",
                column: "AnswersBlockId",
                principalTable: "SurveyAnswersBlocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
