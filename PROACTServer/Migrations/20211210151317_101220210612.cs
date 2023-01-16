using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class _101220210612 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyUserQuestionAnswers_SurveyAnswers_AnswerId",
                table: "SurveyUserQuestionAnswers");

            migrationBuilder.AlterColumn<Guid>(
                name: "AnswerId",
                table: "SurveyUserQuestionAnswers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "SurveyId",
                table: "SurveyUserQuestionAnswers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SurveyUserQuestionAnswers_SurveyId",
                table: "SurveyUserQuestionAnswers",
                column: "SurveyId");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyUserQuestionAnswers_SurveyAnswers_AnswerId",
                table: "SurveyUserQuestionAnswers",
                column: "AnswerId",
                principalTable: "SurveyAnswers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyUserQuestionAnswers_Surveys_SurveyId",
                table: "SurveyUserQuestionAnswers",
                column: "SurveyId",
                principalTable: "Surveys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyUserQuestionAnswers_SurveyAnswers_AnswerId",
                table: "SurveyUserQuestionAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyUserQuestionAnswers_Surveys_SurveyId",
                table: "SurveyUserQuestionAnswers");

            migrationBuilder.DropIndex(
                name: "IX_SurveyUserQuestionAnswers_SurveyId",
                table: "SurveyUserQuestionAnswers");

            migrationBuilder.DropColumn(
                name: "SurveyId",
                table: "SurveyUserQuestionAnswers");

            migrationBuilder.AlterColumn<Guid>(
                name: "AnswerId",
                table: "SurveyUserQuestionAnswers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyUserQuestionAnswers_SurveyAnswers_AnswerId",
                table: "SurveyUserQuestionAnswers",
                column: "AnswerId",
                principalTable: "SurveyAnswers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
