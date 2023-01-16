using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class _250820211028 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ReplyMessageId",
                table: "MessagesReplies",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MessagesReplies_ReplyMessageId",
                table: "MessagesReplies",
                column: "ReplyMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessagesReplies_Messages_ReplyMessageId",
                table: "MessagesReplies",
                column: "ReplyMessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessagesReplies_Messages_ReplyMessageId",
                table: "MessagesReplies");

            migrationBuilder.DropIndex(
                name: "IX_MessagesReplies_ReplyMessageId",
                table: "MessagesReplies");

            migrationBuilder.DropColumn(
                name: "ReplyMessageId",
                table: "MessagesReplies");
        }
    }
}
