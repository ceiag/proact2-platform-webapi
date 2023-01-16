using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proact.Services.Migrations
{
    public partial class _1406221705 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessingTime",
                table: "MessageAttachments");

            migrationBuilder.RenameColumn(
                name: "Extension",
                table: "MessageAttachments",
                newName: "ContainerName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContainerName",
                table: "MessageAttachments",
                newName: "Extension");

            migrationBuilder.AddColumn<double>(
                name: "ProcessingTime",
                table: "MessageAttachments",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
