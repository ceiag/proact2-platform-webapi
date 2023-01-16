using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Proact.Services.Migrations
{
    public partial class adding_more_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginalMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsHandled = table.Column<bool>(type: "bit", nullable: false),
                    IsBroadcast = table.Column<bool>(type: "bit", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MedicalTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsMarked = table.Column<bool>(type: "bit", nullable: false),
                    Emotion = table.Column<int>(type: "int", nullable: true),
                    ContentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DrmProtectedContentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DrmProtectedMediaAssetId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentStatus = table.Column<int>(type: "int", nullable: true),
                    MediaAssetId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecordedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UploadedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: true),
                    ChatMessageType = table.Column<int>(type: "int", nullable: false),
                    Landscape = table.Column<bool>(type: "bit", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_MedicalTeams_MedicalTeamId",
                        column: x => x.MedicalTeamId,
                        principalTable: "MedicalTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MessageRecipients",
                columns: table => new
                {
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecipientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ReadTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageRecipients", x => new { x.MessageId, x.RecipientId });
                    table.ForeignKey(
                        name: "FK_MessageRecipients_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MessageRecipients_Users_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WelcomeMessages",
                columns: table => new
                {
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WelcomeMessages", x => new { x.MessageId, x.EntityId });
                    table.ForeignKey(
                        name: "FK_WelcomeMessages_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageRecipients_RecipientId",
                table: "MessageRecipients",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_AuthorId",
                table: "Messages",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_MedicalTeamId",
                table: "Messages",
                column: "MedicalTeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageRecipients");

            migrationBuilder.DropTable(
                name: "WelcomeMessages");

            migrationBuilder.DropTable(
                name: "Messages");
        }
    }
}
