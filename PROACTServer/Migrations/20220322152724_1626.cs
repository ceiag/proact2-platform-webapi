using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class _1626 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisResults_Analysis_AnalysisId",
                table: "AnalysisResults");

            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisResults_LexiconLabels_LexiconLabelId",
                table: "AnalysisResults");

            migrationBuilder.DropForeignKey(
                name: "FK_LexiconLabels_LexiconCategories_LexiconCategoryId",
                table: "LexiconLabels");

            migrationBuilder.AlterColumn<Guid>(
                name: "LexiconCategoryId",
                table: "LexiconLabels",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "LexiconLabelId",
                table: "AnalysisResults",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "AnalysisId",
                table: "AnalysisResults",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisResults_Analysis_AnalysisId",
                table: "AnalysisResults",
                column: "AnalysisId",
                principalTable: "Analysis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisResults_LexiconLabels_LexiconLabelId",
                table: "AnalysisResults",
                column: "LexiconLabelId",
                principalTable: "LexiconLabels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LexiconLabels_LexiconCategories_LexiconCategoryId",
                table: "LexiconLabels",
                column: "LexiconCategoryId",
                principalTable: "LexiconCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisResults_Analysis_AnalysisId",
                table: "AnalysisResults");

            migrationBuilder.DropForeignKey(
                name: "FK_AnalysisResults_LexiconLabels_LexiconLabelId",
                table: "AnalysisResults");

            migrationBuilder.DropForeignKey(
                name: "FK_LexiconLabels_LexiconCategories_LexiconCategoryId",
                table: "LexiconLabels");

            migrationBuilder.AlterColumn<Guid>(
                name: "LexiconCategoryId",
                table: "LexiconLabels",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "LexiconLabelId",
                table: "AnalysisResults",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AnalysisId",
                table: "AnalysisResults",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisResults_Analysis_AnalysisId",
                table: "AnalysisResults",
                column: "AnalysisId",
                principalTable: "Analysis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnalysisResults_LexiconLabels_LexiconLabelId",
                table: "AnalysisResults",
                column: "LexiconLabelId",
                principalTable: "LexiconLabels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LexiconLabels_LexiconCategories_LexiconCategoryId",
                table: "LexiconLabels",
                column: "LexiconCategoryId",
                principalTable: "LexiconCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
