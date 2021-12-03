using Microsoft.EntityFrameworkCore.Migrations;

namespace SIO.Migrations.Migrations.SIO.Projection
{
    public partial class AddTranslationOptionSubjectToDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TranslationOptionSubject",
                table: "Document",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TranslationOptionSubject",
                table: "Document");
        }
    }
}
