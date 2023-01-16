using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class _240820211624 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_MessagesData_Id",
                table: "Messages");

            migrationBuilder.CreateIndex(
                name: "IX_MessagesData_MessageId",
                table: "MessagesData",
                column: "MessageId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MessagesData_Messages_MessageId",
                table: "MessagesData",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessagesData_Messages_MessageId",
                table: "MessagesData");

            migrationBuilder.DropIndex(
                name: "IX_MessagesData_MessageId",
                table: "MessagesData");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_MessagesData_Id",
                table: "Messages",
                column: "Id",
                principalTable: "MessagesData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
