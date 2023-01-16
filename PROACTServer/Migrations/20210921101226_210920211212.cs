using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class _210920211212 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medics_MedicalTeams_MedicalTeamId",
                table: "Medics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Medics",
                table: "Medics");

            migrationBuilder.AlterColumn<Guid>(
                name: "MedicalTeamId",
                table: "Medics",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Medics",
                table: "Medics",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Medics_UserId",
                table: "Medics",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medics_MedicalTeams_MedicalTeamId",
                table: "Medics",
                column: "MedicalTeamId",
                principalTable: "MedicalTeams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medics_MedicalTeams_MedicalTeamId",
                table: "Medics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Medics",
                table: "Medics");

            migrationBuilder.DropIndex(
                name: "IX_Medics_UserId",
                table: "Medics");

            migrationBuilder.AlterColumn<Guid>(
                name: "MedicalTeamId",
                table: "Medics",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Medics",
                table: "Medics",
                columns: new[] { "UserId", "MedicalTeamId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Medics_MedicalTeams_MedicalTeamId",
                table: "Medics",
                column: "MedicalTeamId",
                principalTable: "MedicalTeams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
