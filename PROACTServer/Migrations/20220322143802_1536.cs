using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class _1536 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisResults_Analysis_ReviewId",
                table: "AnalysisResults");

            migrationBuilder.RenameColumn(
                name: "ReviewId",
                table: "AnalysisResults",
                newName: "AnalysisId");

            migrationBuilder.RenameIndex(
                name: "IX_AnalysisResults_ReviewId",
                table: "AnalysisResults",
                newName: "IX_AnalysisResults_AnalysisId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisResults_Analysis_AnalysisId",
                table: "AnalysisResults",
                column: "AnalysisId",
                principalTable: "Analysis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisResults_Analysis_AnalysisId",
                table: "AnalysisResults");

            migrationBuilder.RenameColumn(
                name: "AnalysisId",
                table: "AnalysisResults",
                newName: "ReviewId");

            migrationBuilder.RenameIndex(
                name: "IX_AnalysisResults_AnalysisId",
                table: "AnalysisResults",
                newName: "IX_AnalysisResults_ReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisResults_Analysis_ReviewId",
                table: "AnalysisResults",
                column: "ReviewId",
                principalTable: "Analysis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
