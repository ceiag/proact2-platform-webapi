using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class _221220211232 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SchedulerId",
                table: "SurveysAssignationsRelations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SurveyScheduler",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpireTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastSubmission = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DaysInterval = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurveyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyScheduler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyScheduler_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SurveyScheduler_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SurveysAssignationsRelations_SchedulerId",
                table: "SurveysAssignationsRelations",
                column: "SchedulerId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyScheduler_SurveyId",
                table: "SurveyScheduler",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyScheduler_UserId",
                table: "SurveyScheduler",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveysAssignationsRelations_SurveyScheduler_SchedulerId",
                table: "SurveysAssignationsRelations",
                column: "SchedulerId",
                principalTable: "SurveyScheduler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveysAssignationsRelations_SurveyScheduler_SchedulerId",
                table: "SurveysAssignationsRelations");

            migrationBuilder.DropTable(
                name: "SurveyScheduler");

            migrationBuilder.DropIndex(
                name: "IX_SurveysAssignationsRelations_SchedulerId",
                table: "SurveysAssignationsRelations");

            migrationBuilder.DropColumn(
                name: "SchedulerId",
                table: "SurveysAssignationsRelations");
        }
    }
}
