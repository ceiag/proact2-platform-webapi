using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proact.Services.Migrations
{
    public partial class institute_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "InstituteId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InstituteId",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Institutes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Institutes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstituteAdmins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstituteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstituteAdmins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstituteAdmins_Institutes_InstituteId",
                        column: x => x.InstituteId,
                        principalTable: "Institutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstituteAdmins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_InstituteId",
                table: "Users",
                column: "InstituteId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_InstituteId",
                table: "Projects",
                column: "InstituteId");

            migrationBuilder.CreateIndex(
                name: "IX_InstituteAdmins_InstituteId",
                table: "InstituteAdmins",
                column: "InstituteId");

            migrationBuilder.CreateIndex(
                name: "IX_InstituteAdmins_UserId",
                table: "InstituteAdmins",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Institutes_InstituteId",
                table: "Projects",
                column: "InstituteId",
                principalTable: "Institutes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Institutes_InstituteId",
                table: "Users",
                column: "InstituteId",
                principalTable: "Institutes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Institutes_InstituteId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Institutes_InstituteId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "InstituteAdmins");

            migrationBuilder.DropTable(
                name: "Institutes");

            migrationBuilder.DropIndex(
                name: "IX_Users_InstituteId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Projects_InstituteId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "InstituteId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "InstituteId",
                table: "Projects");
        }
    }
}
