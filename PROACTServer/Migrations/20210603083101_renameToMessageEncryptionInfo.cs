using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class renameToMessageEncryptionInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_MessageEncryption_EncryptionMessageEncryptionId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "MessageEncryption");

            migrationBuilder.RenameColumn(
                name: "EncryptionMessageEncryptionId",
                table: "Messages",
                newName: "EncryptionMessageEncryptionInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_EncryptionMessageEncryptionId",
                table: "Messages",
                newName: "IX_Messages_EncryptionMessageEncryptionInfoId");

            migrationBuilder.CreateTable(
                name: "MessageEncryptionInfo",
                columns: table => new
                {
                    MessageEncryptionInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EncryptedIV = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    EncryptedKey = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageEncryptionInfo", x => x.MessageEncryptionInfoId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_MessageEncryptionInfo_EncryptionMessageEncryptionInfoId",
                table: "Messages",
                column: "EncryptionMessageEncryptionInfoId",
                principalTable: "MessageEncryptionInfo",
                principalColumn: "MessageEncryptionInfoId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_MessageEncryptionInfo_EncryptionMessageEncryptionInfoId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "MessageEncryptionInfo");

            migrationBuilder.RenameColumn(
                name: "EncryptionMessageEncryptionInfoId",
                table: "Messages",
                newName: "EncryptionMessageEncryptionId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_EncryptionMessageEncryptionInfoId",
                table: "Messages",
                newName: "IX_Messages_EncryptionMessageEncryptionId");

            migrationBuilder.CreateTable(
                name: "MessageEncryption",
                columns: table => new
                {
                    MessageEncryptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EncryptedIV = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    EncryptedKey = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageEncryption", x => x.MessageEncryptionId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_MessageEncryption_EncryptionMessageEncryptionId",
                table: "Messages",
                column: "EncryptionMessageEncryptionId",
                principalTable: "MessageEncryption",
                principalColumn: "MessageEncryptionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
