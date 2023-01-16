using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proact.Services.Migrations
{
    public partial class Extension : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "MessageAttachments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Extension",
                table: "MessageAttachments");
        }
    }
}
