using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class _130120221832 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Users_UserId",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_NotificationSettings_UserId",
                table: "NotificationSettings");

            migrationBuilder.DropIndex(
                name: "IX_Devices_UserId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Devices");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedDateTime",
                table: "SurveysAssignationsRelations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationSettings_UserId",
                table: "NotificationSettings",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NotificationSettings_UserId",
                table: "NotificationSettings");

            migrationBuilder.DropColumn(
                name: "CompletedDateTime",
                table: "SurveysAssignationsRelations");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Devices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationSettings_UserId",
                table: "NotificationSettings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_UserId",
                table: "Devices",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Users_UserId",
                table: "Devices",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
