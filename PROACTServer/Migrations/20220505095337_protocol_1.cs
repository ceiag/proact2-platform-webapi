using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proact.Services.Migrations
{
    public partial class protocol_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InternalCode",
                table: "ProjectProtocols");

            migrationBuilder.DropColumn(
                name: "InternationalCode",
                table: "ProjectProtocols");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ProjectProtocols");

            migrationBuilder.DropColumn(
                name: "InternalCode",
                table: "PatientProtocols");

            migrationBuilder.DropColumn(
                name: "InternationalCode",
                table: "PatientProtocols");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "PatientProtocols");

            migrationBuilder.AddColumn<Guid>(
                name: "ProtocolId",
                table: "ProjectProtocols",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProtocolId",
                table: "PatientProtocols",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Protocols",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternationalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Protocols", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectProtocols_ProtocolId",
                table: "ProjectProtocols",
                column: "ProtocolId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientProtocols_ProtocolId",
                table: "PatientProtocols",
                column: "ProtocolId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientProtocols_Protocols_ProtocolId",
                table: "PatientProtocols",
                column: "ProtocolId",
                principalTable: "Protocols",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectProtocols_Protocols_ProtocolId",
                table: "ProjectProtocols",
                column: "ProtocolId",
                principalTable: "Protocols",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientProtocols_Protocols_ProtocolId",
                table: "PatientProtocols");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectProtocols_Protocols_ProtocolId",
                table: "ProjectProtocols");

            migrationBuilder.DropTable(
                name: "Protocols");

            migrationBuilder.DropIndex(
                name: "IX_ProjectProtocols_ProtocolId",
                table: "ProjectProtocols");

            migrationBuilder.DropIndex(
                name: "IX_PatientProtocols_ProtocolId",
                table: "PatientProtocols");

            migrationBuilder.DropColumn(
                name: "ProtocolId",
                table: "ProjectProtocols");

            migrationBuilder.DropColumn(
                name: "ProtocolId",
                table: "PatientProtocols");

            migrationBuilder.AddColumn<string>(
                name: "InternalCode",
                table: "ProjectProtocols",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternationalCode",
                table: "ProjectProtocols",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ProjectProtocols",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternalCode",
                table: "PatientProtocols",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternationalCode",
                table: "PatientProtocols",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PatientProtocols",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
