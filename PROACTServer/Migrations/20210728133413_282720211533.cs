using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class _282720211533 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "MessageAttachments");

            migrationBuilder.AddColumn<double>(
                name: "DurationInMilliseconds",
                table: "MessageAttachments",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationInMilliseconds",
                table: "MessageAttachments");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "MessageAttachments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
