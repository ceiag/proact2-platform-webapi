using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class _171220211240 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyUserQuestionAnswers_Surveys_SurveyId",
                table: "SurveyUserQuestionAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyUsersQuestionsAnswersRelations_Surveys_SurveyId",
                table: "SurveyUsersQuestionsAnswersRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyUsersQuestionsAnswersRelations_Users_UserId",
                table: "SurveyUsersQuestionsAnswersRelations");

            migrationBuilder.DropIndex(
                name: "IX_SurveyUsersQuestionsAnswersRelations_SurveyId",
                table: "SurveyUsersQuestionsAnswersRelations");

            migrationBuilder.DropIndex(
                name: "IX_SurveyUsersQuestionsAnswersRelations_UserId",
                table: "SurveyUsersQuestionsAnswersRelations");

            migrationBuilder.DropIndex(
                name: "IX_SurveyUserQuestionAnswers_SurveyId",
                table: "SurveyUserQuestionAnswers");

            migrationBuilder.DropColumn(
                name: "SurveyId",
                table: "SurveyUsersQuestionsAnswersRelations");

            migrationBuilder.DropColumn(
                name: "Reccurence",
                table: "SurveysAssignmentsRelations");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "SurveyUsersQuestionsAnswersRelations",
                newName: "AssignmentId");

            migrationBuilder.RenameColumn(
                name: "SurveyId",
                table: "SurveyUserQuestionAnswers",
                newName: "AssignmentId");

            migrationBuilder.AddColumn<Guid>(
                name: "AssignmentRelationId",
                table: "SurveyUsersQuestionsAnswersRelations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AssignmentRelationId",
                table: "SurveyUserQuestionAnswers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurveyUsersQuestionsAnswersRelations_AssignmentRelationId",
                table: "SurveyUsersQuestionsAnswersRelations",
                column: "AssignmentRelationId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyUserQuestionAnswers_AssignmentRelationId",
                table: "SurveyUserQuestionAnswers",
                column: "AssignmentRelationId");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyUserQuestionAnswers_SurveysAssignmentsRelations_AssignmentRelationId",
                table: "SurveyUserQuestionAnswers",
                column: "AssignmentRelationId",
                principalTable: "SurveysAssignmentsRelations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyUsersQuestionsAnswersRelations_SurveysAssignmentsRelations_AssignmentRelationId",
                table: "SurveyUsersQuestionsAnswersRelations",
                column: "AssignmentRelationId",
                principalTable: "SurveysAssignmentsRelations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyUserQuestionAnswers_SurveysAssignmentsRelations_AssignmentRelationId",
                table: "SurveyUserQuestionAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyUsersQuestionsAnswersRelations_SurveysAssignmentsRelations_AssignmentRelationId",
                table: "SurveyUsersQuestionsAnswersRelations");

            migrationBuilder.DropIndex(
                name: "IX_SurveyUsersQuestionsAnswersRelations_AssignmentRelationId",
                table: "SurveyUsersQuestionsAnswersRelations");

            migrationBuilder.DropIndex(
                name: "IX_SurveyUserQuestionAnswers_AssignmentRelationId",
                table: "SurveyUserQuestionAnswers");

            migrationBuilder.DropColumn(
                name: "AssignmentRelationId",
                table: "SurveyUsersQuestionsAnswersRelations");

            migrationBuilder.DropColumn(
                name: "AssignmentRelationId",
                table: "SurveyUserQuestionAnswers");

            migrationBuilder.RenameColumn(
                name: "AssignmentId",
                table: "SurveyUsersQuestionsAnswersRelations",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "AssignmentId",
                table: "SurveyUserQuestionAnswers",
                newName: "SurveyId");

            migrationBuilder.AddColumn<Guid>(
                name: "SurveyId",
                table: "SurveyUsersQuestionsAnswersRelations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Reccurence",
                table: "SurveysAssignmentsRelations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SurveyUsersQuestionsAnswersRelations_SurveyId",
                table: "SurveyUsersQuestionsAnswersRelations",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyUsersQuestionsAnswersRelations_UserId",
                table: "SurveyUsersQuestionsAnswersRelations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyUserQuestionAnswers_SurveyId",
                table: "SurveyUserQuestionAnswers",
                column: "SurveyId");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyUserQuestionAnswers_Surveys_SurveyId",
                table: "SurveyUserQuestionAnswers",
                column: "SurveyId",
                principalTable: "Surveys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyUsersQuestionsAnswersRelations_Surveys_SurveyId",
                table: "SurveyUsersQuestionsAnswersRelations",
                column: "SurveyId",
                principalTable: "Surveys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyUsersQuestionsAnswersRelations_Users_UserId",
                table: "SurveyUsersQuestionsAnswersRelations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
