using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class _250820211041 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessagesReplies_Messages_MessageId",
                table: "MessagesReplies");

            migrationBuilder.RenameColumn(
                name: "MessageId",
                table: "MessagesReplies",
                newName: "OriginalMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessagesReplies_Messages_OriginalMessageId",
                table: "MessagesReplies",
                column: "OriginalMessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessagesReplies_Messages_OriginalMessageId",
                table: "MessagesReplies");

            migrationBuilder.RenameColumn(
                name: "OriginalMessageId",
                table: "MessagesReplies",
                newName: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessagesReplies_Messages_MessageId",
                table: "MessagesReplies",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
