using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class _240820211639 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MessagesData",
                table: "MessagesData");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessagesData",
                table: "MessagesData",
                columns: new[] { "MessageId", "Id" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MessagesData",
                table: "MessagesData");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessagesData",
                table: "MessagesData",
                column: "Id");
        }
    }
}
