using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class _1701 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LexiconCategories_Lexicons_LexiconId",
                table: "LexiconCategories");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "LexiconCategories");

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Lexicons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "LexiconCategories",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "LexiconId",
                table: "LexiconCategories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LexiconCategories_Name",
                table: "LexiconCategories",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_LexiconCategories_Lexicons_LexiconId",
                table: "LexiconCategories",
                column: "LexiconId",
                principalTable: "Lexicons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LexiconCategories_Lexicons_LexiconId",
                table: "LexiconCategories");

            migrationBuilder.DropIndex(
                name: "IX_LexiconCategories_Name",
                table: "LexiconCategories");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Lexicons");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "LexiconCategories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "LexiconId",
                table: "LexiconCategories",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "LexiconCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LexiconCategories_Lexicons_LexiconId",
                table: "LexiconCategories",
                column: "LexiconId",
                principalTable: "Lexicons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
