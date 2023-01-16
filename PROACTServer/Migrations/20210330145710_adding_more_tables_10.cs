using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class adding_more_tables_10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EncryptionMessageEncryptionId",
                table: "Messages",
                type: "uniqueidentifier",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Messages_EncryptionMessageEncryptionId",
                table: "Messages",
                column: "EncryptionMessageEncryptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_MessageEncryption_EncryptionMessageEncryptionId",
                table: "Messages",
                column: "EncryptionMessageEncryptionId",
                principalTable: "MessageEncryption",
                principalColumn: "MessageEncryptionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_MessageEncryption_EncryptionMessageEncryptionId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "MessageEncryption");

            migrationBuilder.DropIndex(
                name: "IX_Messages_EncryptionMessageEncryptionId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "EncryptionMessageEncryptionId",
                table: "Messages");
        }
    }
}
