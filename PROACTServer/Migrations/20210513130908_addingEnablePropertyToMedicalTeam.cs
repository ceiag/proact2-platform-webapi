using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class addingEnablePropertyToMedicalTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "MedicalTeams",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "MedicalTeams");
        }
    }
}
