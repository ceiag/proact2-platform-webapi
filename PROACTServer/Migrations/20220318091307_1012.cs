using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class _1012 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAnalystConsoleActive",
                table: "ProjectProperties",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSurveysSystemActive",
                table: "ProjectProperties",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAnalystConsoleActive",
                table: "ProjectProperties");

            migrationBuilder.DropColumn(
                name: "IsSurveysSystemActive",
                table: "ProjectProperties");
        }
    }
}
