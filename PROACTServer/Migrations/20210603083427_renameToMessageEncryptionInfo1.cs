using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class renameToMessageEncryptionInfo1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_MessageEncryptionInfo_EncryptionMessageEncryptionInfoId",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "EncryptionMessageEncryptionInfoId",
                table: "Messages",
                newName: "MessageEncryptionInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_EncryptionMessageEncryptionInfoId",
                table: "Messages",
                newName: "IX_Messages_MessageEncryptionInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_MessageEncryptionInfo_MessageEncryptionInfoId",
                table: "Messages",
                column: "MessageEncryptionInfoId",
                principalTable: "MessageEncryptionInfo",
                principalColumn: "MessageEncryptionInfoId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_MessageEncryptionInfo_MessageEncryptionInfoId",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "MessageEncryptionInfoId",
                table: "Messages",
                newName: "EncryptionMessageEncryptionInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_MessageEncryptionInfoId",
                table: "Messages",
                newName: "IX_Messages_EncryptionMessageEncryptionInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_MessageEncryptionInfo_EncryptionMessageEncryptionInfoId",
                table: "Messages",
                column: "EncryptionMessageEncryptionInfoId",
                principalTable: "MessageEncryptionInfo",
                principalColumn: "MessageEncryptionInfoId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
