using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proact.Services.Migrations
{
    public partial class Researcher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Researchers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Researchers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Researchers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResearchersMedicalTeamRelation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResearcherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicalTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchersMedicalTeamRelation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResearchersMedicalTeamRelation_MedicalTeams_MedicalTeamId",
                        column: x => x.MedicalTeamId,
                        principalTable: "MedicalTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResearchersMedicalTeamRelation_Researchers_ResearcherId",
                        column: x => x.ResearcherId,
                        principalTable: "Researchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Researchers_UserId",
                table: "Researchers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchersMedicalTeamRelation_MedicalTeamId",
                table: "ResearchersMedicalTeamRelation",
                column: "MedicalTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_ResearchersMedicalTeamRelation_ResearcherId",
                table: "ResearchersMedicalTeamRelation",
                column: "ResearcherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResearchersMedicalTeamRelation");

            migrationBuilder.DropTable(
                name: "Researchers");
        }
    }
}
