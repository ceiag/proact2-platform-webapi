using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class _100120221049 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrugTargets_Projects_ProjectId",
                table: "DrugTargets");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyQuestions_SurveyQuestionsSets_QuestionSetId",
                table: "SurveyQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_NotificationSettings_NotificationSettingsId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_NotificationSettingsId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_SurveyQuestions_QuestionSetId",
                table: "SurveyQuestions");

            migrationBuilder.DropColumn(
                name: "NotificationSettingsId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "QuestionSetId",
                table: "SurveyQuestions");

            migrationBuilder.AddColumn<Guid>(
                name: "AdminUserId",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "NotificationSettings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "ProjectId",
                table: "DrugTargets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestions_QuestionsSetId",
                table: "SurveyQuestions",
                column: "QuestionsSetId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationSettings_UserId",
                table: "NotificationSettings",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DrugTargets_Projects_ProjectId",
                table: "DrugTargets",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationSettings_Users_UserId",
                table: "NotificationSettings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyQuestions_SurveyQuestionsSets_QuestionsSetId",
                table: "SurveyQuestions",
                column: "QuestionsSetId",
                principalTable: "SurveyQuestionsSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrugTargets_Projects_ProjectId",
                table: "DrugTargets");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationSettings_Users_UserId",
                table: "NotificationSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyQuestions_SurveyQuestionsSets_QuestionsSetId",
                table: "SurveyQuestions");

            migrationBuilder.DropIndex(
                name: "IX_SurveyQuestions_QuestionsSetId",
                table: "SurveyQuestions");

            migrationBuilder.DropIndex(
                name: "IX_NotificationSettings_UserId",
                table: "NotificationSettings");

            migrationBuilder.DropColumn(
                name: "AdminUserId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "NotificationSettings");

            migrationBuilder.AddColumn<Guid>(
                name: "NotificationSettingsId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionSetId",
                table: "SurveyQuestions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ProjectId",
                table: "DrugTargets",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Users_NotificationSettingsId",
                table: "Users",
                column: "NotificationSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestions_QuestionSetId",
                table: "SurveyQuestions",
                column: "QuestionSetId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrugTargets_Projects_ProjectId",
                table: "DrugTargets",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyQuestions_SurveyQuestionsSets_QuestionSetId",
                table: "SurveyQuestions",
                column: "QuestionSetId",
                principalTable: "SurveyQuestionsSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_NotificationSettings_NotificationSettingsId",
                table: "Users",
                column: "NotificationSettingsId",
                principalTable: "NotificationSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
