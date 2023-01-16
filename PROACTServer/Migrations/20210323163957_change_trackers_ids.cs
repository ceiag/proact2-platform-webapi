using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class change_trackers_ids : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creator",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "Projects");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "User",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ModifierId",
                table: "User",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ModifierId",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ModifierId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ModifierId",
                table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
