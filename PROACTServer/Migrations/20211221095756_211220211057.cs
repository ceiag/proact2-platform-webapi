using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class _211220211057 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyUsersQuestionsAnswersRelations_SurveysAssignmentsRelations_AssignmentRelationId",
                table: "SurveyUsersQuestionsAnswersRelations");

            migrationBuilder.DropIndex(
                name: "IX_SurveyUsersQuestionsAnswersRelations_AssignmentRelationId",
                table: "SurveyUsersQuestionsAnswersRelations");

            migrationBuilder.DropColumn(
                name: "AssignmentRelationId",
                table: "SurveyUsersQuestionsAnswersRelations");

            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "SurveysAssignmentsRelations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_SurveyUsersQuestionsAnswersRelations_AssignmentId",
                table: "SurveyUsersQuestionsAnswersRelations",
                column: "AssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyUsersQuestionsAnswersRelations_SurveysAssignmentsRelations_AssignmentId",
                table: "SurveyUsersQuestionsAnswersRelations",
                column: "AssignmentId",
                principalTable: "SurveysAssignmentsRelations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyUsersQuestionsAnswersRelations_SurveysAssignmentsRelations_AssignmentId",
                table: "SurveyUsersQuestionsAnswersRelations");

            migrationBuilder.DropIndex(
                name: "IX_SurveyUsersQuestionsAnswersRelations_AssignmentId",
                table: "SurveyUsersQuestionsAnswersRelations");

            migrationBuilder.DropColumn(
                name: "Completed",
                table: "SurveysAssignmentsRelations");

            migrationBuilder.AddColumn<Guid>(
                name: "AssignmentRelationId",
                table: "SurveyUsersQuestionsAnswersRelations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurveyUsersQuestionsAnswersRelations_AssignmentRelationId",
                table: "SurveyUsersQuestionsAnswersRelations",
                column: "AssignmentRelationId");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyUsersQuestionsAnswersRelations_SurveysAssignmentsRelations_AssignmentRelationId",
                table: "SurveyUsersQuestionsAnswersRelations",
                column: "AssignmentRelationId",
                principalTable: "SurveysAssignmentsRelations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
