using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class _101120211038 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationSettings_Users_UserId",
                table: "NotificationSettings");

            migrationBuilder.DropIndex(
                name: "IX_NotificationSettings_UserId",
                table: "NotificationSettings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "NotificationSettings");

            migrationBuilder.AddColumn<Guid>(
                name: "NotificationSettingsId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StopAt",
                table: "NotificationSettings",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartAt",
                table: "NotificationSettings",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_Users_NotificationSettingsId",
                table: "Users",
                column: "NotificationSettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_NotificationSettings_NotificationSettingsId",
                table: "Users",
                column: "NotificationSettingsId",
                principalTable: "NotificationSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_NotificationSettings_NotificationSettingsId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_NotificationSettingsId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NotificationSettingsId",
                table: "Users");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StopAt",
                table: "NotificationSettings",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartAt",
                table: "NotificationSettings",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "NotificationSettings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_NotificationSettings_UserId",
                table: "NotificationSettings",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationSettings_Users_UserId",
                table: "NotificationSettings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
