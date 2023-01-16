using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class _021120211122 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageRecipients_Users_RecipientId",
                table: "MessageRecipients");

            migrationBuilder.RenameColumn(
                name: "RecipientId",
                table: "MessageRecipients",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MessageRecipients_RecipientId",
                table: "MessageRecipients",
                newName: "IX_MessageRecipients_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageRecipients_Users_UserId",
                table: "MessageRecipients",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageRecipients_Users_UserId",
                table: "MessageRecipients");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "MessageRecipients",
                newName: "RecipientId");

            migrationBuilder.RenameIndex(
                name: "IX_MessageRecipients_UserId",
                table: "MessageRecipients",
                newName: "IX_MessageRecipients_RecipientId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageRecipients_Users_RecipientId",
                table: "MessageRecipients",
                column: "RecipientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
