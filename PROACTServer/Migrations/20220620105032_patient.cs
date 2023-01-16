using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proact.Services.Migrations
{
    public partial class patient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ECode",
                table: "Patients");

            migrationBuilder.RenameColumn(
                name: "PatientDataAssetId",
                table: "Patients",
                newName: "Code");

            migrationBuilder.AddColumn<DateTime>(
                name: "TreatmentEndDate",
                table: "Patients",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TreatmentEndDate",
                table: "Patients");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Patients",
                newName: "PatientDataAssetId");

            migrationBuilder.AddColumn<string>(
                name: "ECode",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
