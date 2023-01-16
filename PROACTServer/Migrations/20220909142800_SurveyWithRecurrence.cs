using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proact.Services.Migrations
{
    public partial class SurveyWithRecurrence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireTime",
                table: "Surveys",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Reccurence",
                table: "Surveys",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "Surveys",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpireTime",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "Reccurence",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Surveys");
        }
    }
}
