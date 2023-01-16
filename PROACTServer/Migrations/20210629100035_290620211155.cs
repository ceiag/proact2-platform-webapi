using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class _290620211155 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentStatus",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ContentUrl",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "DrmProtectedContentUrl",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "DrmProtectedMediaAssetId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "IsMarked",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Landscape",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "MediaAssetId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "UploadedTime",
                table: "Messages");

            migrationBuilder.CreateTable(
                name: "MessageAttachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UploadedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    Landscape = table.Column<bool>(type: "bit", nullable: true),
                    MediaAssetId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DrmProtectedContentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DrmProtectedMediaAssetId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentStatus = table.Column<int>(type: "int", nullable: true),
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageAttachments_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageAttachments_MessageId",
                table: "MessageAttachments",
                column: "MessageId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageAttachments");

            migrationBuilder.AddColumn<int>(
                name: "ContentStatus",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentUrl",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DrmProtectedContentUrl",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DrmProtectedMediaAssetId",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMarked",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Landscape",
                table: "Messages",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MediaAssetId",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MediaType",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UploadedTime",
                table: "Messages",
                type: "datetime2",
                nullable: true);
        }
    }
}
