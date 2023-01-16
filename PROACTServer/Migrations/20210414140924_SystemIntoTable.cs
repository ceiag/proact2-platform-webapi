using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class SystemIntoTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProactSystemInfo",
                columns: table => new
                {
                    ProactSystemInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SystemInitialized = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProactSystemInfo", x => x.ProactSystemInfoId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProactSystemInfo");
        }
    }
}
