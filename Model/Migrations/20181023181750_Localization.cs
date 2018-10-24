using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hymperia.Model.Migrations
{
  public partial class Localization : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "Materiaux",
          columns: table => new
          {
            Id = table.Column<int>(nullable: false)
                  .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            StringKey = table.Column<string>(nullable: false),
            CultureKey = table.Column<string>(nullable: false),
            Nom = table.Column<string>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Materiaux", x => x.Id);
            table.UniqueConstraint("AK_Materiaux_CultureKey_Nom", x => new { x.CultureKey, x.Nom });
            table.UniqueConstraint("AK_Materiaux_StringKey_CultureKey", x => new { x.StringKey, x.CultureKey });
          });

      migrationBuilder.InsertData(
        "Materiaux",
        new string[] { "Id", "StringKey", "CultureKey", "Nom" },
        new object[,]
        {
                { 1, "Bois", "fr", "Bois" },
                { 2, "Acier", "fr", "Acier" },
                { 3, "Cuivre", "fr", "Cuivre" },
                { 4, "Or", "fr", "Or" },
                { 5, "Bois", "en", "Wood" },
                { 6, "Acier", "en", "Steel" },
                { 7, "Cuivre", "en", "Copper" },
                { 8, "Or", "en", "Gold" }
        });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "Materiaux");
    }
  }
}
