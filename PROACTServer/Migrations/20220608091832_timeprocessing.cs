using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proact.Services.Migrations
{
    public partial class timeprocessing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Landscape",
                table: "MessageAttachments");

            migrationBuilder.AddColumn<double>(
                name: "ProcessingTime",
                table: "MessageAttachments",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessingTime",
                table: "MessageAttachments");

            migrationBuilder.AddColumn<bool>(
                name: "Landscape",
                table: "MessageAttachments",
                type: "bit",
                nullable: true);
        }
    }
}
