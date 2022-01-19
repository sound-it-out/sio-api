using Microsoft.EntityFrameworkCore.Migrations;

namespace SIO.Migrations.Migrations.SIO.Projection
{
    public partial class AddTranslationOptionProjection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TranslationOption",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TranslationType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranslationOption", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TranslationOption");
        }
    }
}
