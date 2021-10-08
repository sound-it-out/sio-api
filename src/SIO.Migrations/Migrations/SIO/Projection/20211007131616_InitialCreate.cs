using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SIO.Migrations.Migrations.SIO.Projection
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Audit",
                columns: table => new
                {
                    Subject = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audit", x => x.Subject);
                });

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TranslationType = table.Column<int>(type: "int", nullable: false),
                    TranslationSubject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CharactersProcessed = table.Column<long>(type: "bigint", nullable: false),
                    TotalCharacters = table.Column<long>(type: "bigint", nullable: false),
                    TranslationProgress = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectionState",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Position = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectionState", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "UserDocuments",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDocuments", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "AuditEvent",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TimeStamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    User = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditEvent_Audit_AuditId",
                        column: x => x.AuditId,
                        principalTable: "Audit",
                        principalColumn: "Subject",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDocument",
                columns: table => new
                {
                    DocumentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDocument", x => x.DocumentId);
                    table.ForeignKey(
                        name: "FK_UserDocument_UserDocuments_ParentId",
                        column: x => x.ParentId,
                        principalTable: "UserDocuments",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditEvent_AuditId",
                table: "AuditEvent",
                column: "AuditId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDocument_ParentId",
                table: "UserDocument",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditEvent");

            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "ProjectionState");

            migrationBuilder.DropTable(
                name: "UserDocument");

            migrationBuilder.DropTable(
                name: "Audit");

            migrationBuilder.DropTable(
                name: "UserDocuments");
        }
    }
}
