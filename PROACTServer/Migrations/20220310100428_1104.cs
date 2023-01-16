using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class _1104 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medics_MedicalTeams_MedicalTeamId",
                table: "Medics");

            migrationBuilder.DropForeignKey(
                name: "FK_Nurses_MedicalTeams_MedicalTeamId",
                table: "Nurses");

            migrationBuilder.DropTable(
                name: "DrugTargetsPatient");

            migrationBuilder.DropTable(
                name: "WelcomeMessages");

            migrationBuilder.DropTable(
                name: "DrugTargets");

            migrationBuilder.DropIndex(
                name: "IX_Nurses_MedicalTeamId",
                table: "Nurses");

            migrationBuilder.DropIndex(
                name: "IX_Medics_MedicalTeamId",
                table: "Medics");

            migrationBuilder.DropColumn(
                name: "ShowBroadcastMessages",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "MedicalTeamId",
                table: "Nurses");

            migrationBuilder.DropColumn(
                name: "MedicalTeamId",
                table: "Medics");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectPropertiesId",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RegionCode",
                table: "MedicalTeams",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "MedicalTeams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Analysis",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analysis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Analysis_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Analysis_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lexicons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lexicons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicsMedicalTeamRelations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicalTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicsMedicalTeamRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicsMedicalTeamRelations_MedicalTeams_MedicalTeamId",
                        column: x => x.MedicalTeamId,
                        principalTable: "MedicalTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicsMedicalTeamRelations_Medics_MedicId",
                        column: x => x.MedicId,
                        principalTable: "Medics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NursesMedicalTeamRelations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NurseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicalTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NursesMedicalTeamRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NursesMedicalTeamRelations_MedicalTeams_MedicalTeamId",
                        column: x => x.MedicalTeamId,
                        principalTable: "MedicalTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NursesMedicalTeamRelations_Nurses_NurseId",
                        column: x => x.NurseId,
                        principalTable: "Nurses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LexiconCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MultipleSelection = table.Column<bool>(type: "bit", nullable: false),
                    LexiconId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LexiconCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LexiconCategories_Lexicons_LexiconId",
                        column: x => x.LexiconId,
                        principalTable: "Lexicons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectProperties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicsCanSeeOtherAnalisys = table.Column<bool>(type: "bit", nullable: false),
                    MessageCanNotBeDeletedAfterMinutes = table.Column<int>(type: "int", nullable: false),
                    MessageCanBeAnalizedAfterMinutes = table.Column<int>(type: "int", nullable: false),
                    LexiconId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectProperties_Lexicons_LexiconId",
                        column: x => x.LexiconId,
                        principalTable: "Lexicons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectProperties_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LexiconLabels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LexiconCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LexiconLabels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LexiconLabels_LexiconCategories_LexiconCategoryId",
                        column: x => x.LexiconCategoryId,
                        principalTable: "LexiconCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnalysisResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LexiconLabelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalysisResults_Analysis_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Analysis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnalysisResults_LexiconLabels_LexiconLabelId",
                        column: x => x.LexiconLabelId,
                        principalTable: "LexiconLabels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Analysis_MessageId",
                table: "Analysis",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Analysis_UserId",
                table: "Analysis",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisResults_LexiconLabelId",
                table: "AnalysisResults",
                column: "LexiconLabelId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalysisResults_ReviewId",
                table: "AnalysisResults",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_LexiconCategories_LexiconId",
                table: "LexiconCategories",
                column: "LexiconId");

            migrationBuilder.CreateIndex(
                name: "IX_LexiconLabels_LexiconCategoryId",
                table: "LexiconLabels",
                column: "LexiconCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicsMedicalTeamRelations_MedicalTeamId",
                table: "MedicsMedicalTeamRelations",
                column: "MedicalTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicsMedicalTeamRelations_MedicId",
                table: "MedicsMedicalTeamRelations",
                column: "MedicId");

            migrationBuilder.CreateIndex(
                name: "IX_NursesMedicalTeamRelations_MedicalTeamId",
                table: "NursesMedicalTeamRelations",
                column: "MedicalTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_NursesMedicalTeamRelations_NurseId",
                table: "NursesMedicalTeamRelations",
                column: "NurseId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectProperties_LexiconId",
                table: "ProjectProperties",
                column: "LexiconId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectProperties_ProjectId",
                table: "ProjectProperties",
                column: "ProjectId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnalysisResults");

            migrationBuilder.DropTable(
                name: "MedicsMedicalTeamRelations");

            migrationBuilder.DropTable(
                name: "NursesMedicalTeamRelations");

            migrationBuilder.DropTable(
                name: "ProjectProperties");

            migrationBuilder.DropTable(
                name: "Analysis");

            migrationBuilder.DropTable(
                name: "LexiconLabels");

            migrationBuilder.DropTable(
                name: "LexiconCategories");

            migrationBuilder.DropTable(
                name: "Lexicons");

            migrationBuilder.DropColumn(
                name: "ProjectPropertiesId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "State",
                table: "MedicalTeams");

            migrationBuilder.AddColumn<bool>(
                name: "ShowBroadcastMessages",
                table: "Patients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "MedicalTeamId",
                table: "Nurses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MedicalTeamId",
                table: "Medics",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RegionCode",
                table: "MedicalTeams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "DrugTargets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugTargets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DrugTargets_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WelcomeMessages",
                columns: table => new
                {
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WelcomeMessages", x => new { x.MessageId, x.EntityId });
                    table.ForeignKey(
                        name: "FK_WelcomeMessages_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DrugTargetsPatient",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicalTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrugTargetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugTargetsPatient", x => new { x.UserId, x.MedicalTeamId, x.DrugTargetId });
                    table.ForeignKey(
                        name: "FK_DrugTargetsPatient_DrugTargets_DrugTargetId",
                        column: x => x.DrugTargetId,
                        principalTable: "DrugTargets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DrugTargetsPatient_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Nurses_MedicalTeamId",
                table: "Nurses",
                column: "MedicalTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Medics_MedicalTeamId",
                table: "Medics",
                column: "MedicalTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugTargets_ProjectId",
                table: "DrugTargets",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugTargetsPatient_DrugTargetId",
                table: "DrugTargetsPatient",
                column: "DrugTargetId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugTargetsPatient_PatientId",
                table: "DrugTargetsPatient",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medics_MedicalTeams_MedicalTeamId",
                table: "Medics",
                column: "MedicalTeamId",
                principalTable: "MedicalTeams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Nurses_MedicalTeams_MedicalTeamId",
                table: "Nurses",
                column: "MedicalTeamId",
                principalTable: "MedicalTeams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
