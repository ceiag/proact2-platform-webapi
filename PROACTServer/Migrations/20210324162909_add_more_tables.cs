using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class add_more_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DrugTargets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugTargets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DrugTargets_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MedicalTeams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressLine1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressLine2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StateOrProvince = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegionCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeZone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalTeams_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicAdmins",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicalTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicAdmins", x => new { x.UserId, x.MedicalTeamId });
                    table.ForeignKey(
                        name: "FK_MedicAdmins_MedicalTeams_MedicalTeamId",
                        column: x => x.MedicalTeamId,
                        principalTable: "MedicalTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicAdmins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Medics",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicalTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medics", x => new { x.UserId, x.MedicalTeamId });
                    table.ForeignKey(
                        name: "FK_Medics_MedicalTeams_MedicalTeamId",
                        column: x => x.MedicalTeamId,
                        principalTable: "MedicalTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Medics_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicalTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ECode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowBroadcastMessages = table.Column<bool>(type: "bit", nullable: false),
                    PatientDataAssetId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthYear = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TreatmentStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => new { x.UserId, x.MedicalTeamId });
                    table.ForeignKey(
                        name: "FK_Patients_MedicalTeams_MedicalTeamId",
                        column: x => x.MedicalTeamId,
                        principalTable: "MedicalTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Patients_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DrugTargetPatient",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicalTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrugTargetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugTargetPatient", x => new { x.UserId, x.MedicalTeamId, x.DrugTargetId });
                    table.ForeignKey(
                        name: "FK_DrugTargetPatient_DrugTargets_DrugTargetId",
                        column: x => x.DrugTargetId,
                        principalTable: "DrugTargets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DrugTargetPatient_Patients_UserId_MedicalTeamId",
                        columns: x => new { x.UserId, x.MedicalTeamId },
                        principalTable: "Patients",
                        principalColumns: new[] { "UserId", "MedicalTeamId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DrugTargetPatient_DrugTargetId",
                table: "DrugTargetPatient",
                column: "DrugTargetId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugTargets_ProjectId",
                table: "DrugTargets",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicAdmins_MedicalTeamId",
                table: "MedicAdmins",
                column: "MedicalTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalTeams_ProjectId",
                table: "MedicalTeams",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Medics_MedicalTeamId",
                table: "Medics",
                column: "MedicalTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_MedicalTeamId",
                table: "Patients",
                column: "MedicalTeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrugTargetPatient");

            migrationBuilder.DropTable(
                name: "MedicAdmins");

            migrationBuilder.DropTable(
                name: "Medics");

            migrationBuilder.DropTable(
                name: "DrugTargets");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "MedicalTeams");
        }
    }
}
