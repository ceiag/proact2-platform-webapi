using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class _030820211729 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DrmProtectedContentUrl",
                table: "MessageAttachments");

            migrationBuilder.DropColumn(
                name: "DrmProtectedMediaAssetId",
                table: "MessageAttachments");

            migrationBuilder.DropColumn(
                name: "MediaAssetId",
                table: "MessageAttachments");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "MessageAttachments",
                newName: "AssetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AssetId",
                table: "MessageAttachments",
                newName: "Token");

            migrationBuilder.AddColumn<string>(
                name: "DrmProtectedContentUrl",
                table: "MessageAttachments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DrmProtectedMediaAssetId",
                table: "MessageAttachments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MediaAssetId",
                table: "MessageAttachments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
