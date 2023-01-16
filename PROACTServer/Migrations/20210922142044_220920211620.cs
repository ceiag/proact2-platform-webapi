using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class _220920211620 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrugTargetPatient_DrugTargets_DrugTargetId",
                table: "DrugTargetPatient");

            migrationBuilder.DropForeignKey(
                name: "FK_DrugTargetPatient_Patients_PatientId",
                table: "DrugTargetPatient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DrugTargetPatient",
                table: "DrugTargetPatient");

            migrationBuilder.RenameTable(
                name: "DrugTargetPatient",
                newName: "DrugTargetsPatient");

            migrationBuilder.RenameIndex(
                name: "IX_DrugTargetPatient_PatientId",
                table: "DrugTargetsPatient",
                newName: "IX_DrugTargetsPatient_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_DrugTargetPatient_DrugTargetId",
                table: "DrugTargetsPatient",
                newName: "IX_DrugTargetsPatient_DrugTargetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DrugTargetsPatient",
                table: "DrugTargetsPatient",
                columns: new[] { "UserId", "MedicalTeamId", "DrugTargetId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DrugTargetsPatient_DrugTargets_DrugTargetId",
                table: "DrugTargetsPatient",
                column: "DrugTargetId",
                principalTable: "DrugTargets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DrugTargetsPatient_Patients_PatientId",
                table: "DrugTargetsPatient",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrugTargetsPatient_DrugTargets_DrugTargetId",
                table: "DrugTargetsPatient");

            migrationBuilder.DropForeignKey(
                name: "FK_DrugTargetsPatient_Patients_PatientId",
                table: "DrugTargetsPatient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DrugTargetsPatient",
                table: "DrugTargetsPatient");

            migrationBuilder.RenameTable(
                name: "DrugTargetsPatient",
                newName: "DrugTargetPatient");

            migrationBuilder.RenameIndex(
                name: "IX_DrugTargetsPatient_PatientId",
                table: "DrugTargetPatient",
                newName: "IX_DrugTargetPatient_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_DrugTargetsPatient_DrugTargetId",
                table: "DrugTargetPatient",
                newName: "IX_DrugTargetPatient_DrugTargetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DrugTargetPatient",
                table: "DrugTargetPatient",
                columns: new[] { "UserId", "MedicalTeamId", "DrugTargetId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DrugTargetPatient_DrugTargets_DrugTargetId",
                table: "DrugTargetPatient",
                column: "DrugTargetId",
                principalTable: "DrugTargets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DrugTargetPatient_Patients_PatientId",
                table: "DrugTargetPatient",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
