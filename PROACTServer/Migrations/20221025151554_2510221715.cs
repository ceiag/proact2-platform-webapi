using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proact.Services.Migrations
{
    public partial class _2510221715 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DataManagerId",
                table: "NursesMedicalTeamRelations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DataManagers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataManagers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataManagers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DataManagersMedicalTeamRelations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicalTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataManagersMedicalTeamRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataManagersMedicalTeamRelations_DataManagers_DataManagerId",
                        column: x => x.DataManagerId,
                        principalTable: "DataManagers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DataManagersMedicalTeamRelations_MedicalTeams_MedicalTeamId",
                        column: x => x.MedicalTeamId,
                        principalTable: "MedicalTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NursesMedicalTeamRelations_DataManagerId",
                table: "NursesMedicalTeamRelations",
                column: "DataManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_DataManagers_UserId",
                table: "DataManagers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DataManagersMedicalTeamRelations_DataManagerId",
                table: "DataManagersMedicalTeamRelations",
                column: "DataManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_DataManagersMedicalTeamRelations_MedicalTeamId",
                table: "DataManagersMedicalTeamRelations",
                column: "MedicalTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_NursesMedicalTeamRelations_DataManagers_DataManagerId",
                table: "NursesMedicalTeamRelations",
                column: "DataManagerId",
                principalTable: "DataManagers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NursesMedicalTeamRelations_DataManagers_DataManagerId",
                table: "NursesMedicalTeamRelations");

            migrationBuilder.DropTable(
                name: "DataManagersMedicalTeamRelations");

            migrationBuilder.DropTable(
                name: "DataManagers");

            migrationBuilder.DropIndex(
                name: "IX_NursesMedicalTeamRelations_DataManagerId",
                table: "NursesMedicalTeamRelations");

            migrationBuilder.DropColumn(
                name: "DataManagerId",
                table: "NursesMedicalTeamRelations");
        }
    }
}
