using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class _240820211523 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MessagesReplies",
                table: "MessagesReplies");

            migrationBuilder.DropIndex(
                name: "IX_MessagesReplies_MessageId",
                table: "MessagesReplies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessagesReplies",
                table: "MessagesReplies",
                columns: new[] { "MessageId", "Id" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MessagesReplies",
                table: "MessagesReplies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessagesReplies",
                table: "MessagesReplies",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MessagesReplies_MessageId",
                table: "MessagesReplies",
                column: "MessageId");
        }
    }
}
