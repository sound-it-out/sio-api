using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SIO.Migrations.Migrations.OpenEventSourcing.Projection
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TranslationId = table.Column<Guid>(nullable: true),
                    Data = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    LastModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Version = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectionState",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    Position = table.Column<long>(nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    LastModifiedDate = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectionState", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Data = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    LastModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Version = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserDocument",
                columns: table => new
                {
                    DocumentId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Data = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    LastModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Version = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDocument", x => new { x.DocumentId, x.UserId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "ProjectionState");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "UserDocument");
        }
    }
}
