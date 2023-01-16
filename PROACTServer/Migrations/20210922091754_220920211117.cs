using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class _220920211117 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrugTargetPatient_Patients_UserId_MedicalTeamId",
                table: "DrugTargetPatient");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_MedicalTeams_MedicalTeamId",
                table: "Patients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Patients",
                table: "Patients");

            migrationBuilder.AlterColumn<Guid>(
                name: "MedicalTeamId",
                table: "Patients",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "PatientId",
                table: "DrugTargetPatient",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Patients",
                table: "Patients",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_UserId",
                table: "Patients",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugTargetPatient_PatientId",
                table: "DrugTargetPatient",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrugTargetPatient_Patients_PatientId",
                table: "DrugTargetPatient",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_MedicalTeams_MedicalTeamId",
                table: "Patients",
                column: "MedicalTeamId",
                principalTable: "MedicalTeams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrugTargetPatient_Patients_PatientId",
                table: "DrugTargetPatient");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_MedicalTeams_MedicalTeamId",
                table: "Patients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Patients",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_UserId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_DrugTargetPatient_PatientId",
                table: "DrugTargetPatient");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "DrugTargetPatient");

            migrationBuilder.AlterColumn<Guid>(
                name: "MedicalTeamId",
                table: "Patients",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Patients",
                table: "Patients",
                columns: new[] { "UserId", "MedicalTeamId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DrugTargetPatient_Patients_UserId_MedicalTeamId",
                table: "DrugTargetPatient",
                columns: new[] { "UserId", "MedicalTeamId" },
                principalTable: "Patients",
                principalColumns: new[] { "UserId", "MedicalTeamId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_MedicalTeams_MedicalTeamId",
                table: "Patients",
                column: "MedicalTeamId",
                principalTable: "MedicalTeams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
