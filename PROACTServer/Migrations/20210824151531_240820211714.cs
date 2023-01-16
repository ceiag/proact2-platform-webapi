using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class _240820211714 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_MessageEncryptionInfo_MessageEncryptionInfoId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "MessageEncryptionInfo");

            migrationBuilder.DropIndex(
                name: "IX_Messages_MessageEncryptionInfoId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "MessageEncryptionInfoId",
                table: "Messages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MessageEncryptionInfoId",
                table: "Messages",
                type: "uniqueidentifier",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Messages_MessageEncryptionInfoId",
                table: "Messages",
                column: "MessageEncryptionInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_MessageEncryptionInfo_MessageEncryptionInfoId",
                table: "Messages",
                column: "MessageEncryptionInfoId",
                principalTable: "MessageEncryptionInfo",
                principalColumn: "MessageEncryptionInfoId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
