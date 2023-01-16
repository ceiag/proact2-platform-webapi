using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class _1451211220211452 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveysAssignmentsRelations_Surveys_SurveyId",
                table: "SurveysAssignmentsRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveysAssignmentsRelations_Users_UserId",
                table: "SurveysAssignmentsRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyUserQuestionAnswers_SurveysAssignmentsRelations_AssignmentRelationId",
                table: "SurveyUserQuestionAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyUsersQuestionsAnswersRelations_SurveysAssignmentsRelations_AssignmentId",
                table: "SurveyUsersQuestionsAnswersRelations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SurveysAssignmentsRelations",
                table: "SurveysAssignmentsRelations");

            migrationBuilder.RenameTable(
                name: "SurveysAssignmentsRelations",
                newName: "SurveysAssignationsRelations");

            migrationBuilder.RenameIndex(
                name: "IX_SurveysAssignmentsRelations_UserId",
                table: "SurveysAssignationsRelations",
                newName: "IX_SurveysAssignationsRelations_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_SurveysAssignmentsRelations_SurveyId",
                table: "SurveysAssignationsRelations",
                newName: "IX_SurveysAssignationsRelations_SurveyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurveysAssignationsRelations",
                table: "SurveysAssignationsRelations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveysAssignationsRelations_Surveys_SurveyId",
                table: "SurveysAssignationsRelations",
                column: "SurveyId",
                principalTable: "Surveys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveysAssignationsRelations_Users_UserId",
                table: "SurveysAssignationsRelations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyUserQuestionAnswers_SurveysAssignationsRelations_AssignmentRelationId",
                table: "SurveyUserQuestionAnswers",
                column: "AssignmentRelationId",
                principalTable: "SurveysAssignationsRelations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyUsersQuestionsAnswersRelations_SurveysAssignationsRelations_AssignmentId",
                table: "SurveyUsersQuestionsAnswersRelations",
                column: "AssignmentId",
                principalTable: "SurveysAssignationsRelations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveysAssignationsRelations_Surveys_SurveyId",
                table: "SurveysAssignationsRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveysAssignationsRelations_Users_UserId",
                table: "SurveysAssignationsRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyUserQuestionAnswers_SurveysAssignationsRelations_AssignmentRelationId",
                table: "SurveyUserQuestionAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyUsersQuestionsAnswersRelations_SurveysAssignationsRelations_AssignmentId",
                table: "SurveyUsersQuestionsAnswersRelations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SurveysAssignationsRelations",
                table: "SurveysAssignationsRelations");

            migrationBuilder.RenameTable(
                name: "SurveysAssignationsRelations",
                newName: "SurveysAssignmentsRelations");

            migrationBuilder.RenameIndex(
                name: "IX_SurveysAssignationsRelations_UserId",
                table: "SurveysAssignmentsRelations",
                newName: "IX_SurveysAssignmentsRelations_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_SurveysAssignationsRelations_SurveyId",
                table: "SurveysAssignmentsRelations",
                newName: "IX_SurveysAssignmentsRelations_SurveyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurveysAssignmentsRelations",
                table: "SurveysAssignmentsRelations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveysAssignmentsRelations_Surveys_SurveyId",
                table: "SurveysAssignmentsRelations",
                column: "SurveyId",
                principalTable: "Surveys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveysAssignmentsRelations_Users_UserId",
                table: "SurveysAssignmentsRelations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyUserQuestionAnswers_SurveysAssignmentsRelations_AssignmentRelationId",
                table: "SurveyUserQuestionAnswers",
                column: "AssignmentRelationId",
                principalTable: "SurveysAssignmentsRelations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyUsersQuestionsAnswersRelations_SurveysAssignmentsRelations_AssignmentId",
                table: "SurveyUsersQuestionsAnswersRelations",
                column: "AssignmentId",
                principalTable: "SurveysAssignmentsRelations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
