using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class _170620211103 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBroadcast",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "ChatMessageType",
                table: "Messages",
                newName: "MessageType");

            migrationBuilder.AddColumn<int>(
                name: "MediaType",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "MessageType",
                table: "Messages",
                newName: "ChatMessageType");

            migrationBuilder.AddColumn<bool>(
                name: "IsBroadcast",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
