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
            table.PrimaryKey("PK_Materiaux", x => new { x.StringKey, x.CultureKey });
            table.UniqueConstraint("AK_Materiaux_CultureKey_Nom", x => new { x.CultureKey, x.Nom });
          });

      migrationBuilder.InsertData(
        "Materiaux",
        new string[] { "StringKey", "CultureKey", "Nom" },
        new object[,]
        {
                { "Bois", "fr", "Bois" },
                { "Acier", "fr", "Acier" },
                { "Cuivre", "fr", "Cuivre" },
                { "Or", "fr", "Or" },
                { "Bois", "en", "Wood" },
                { "Acier", "en", "Steel" },
                { "Cuivre", "en", "Copper" },
                { "Or", "en", "Gold" }
        });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "Materiaux");
    }
  }
}
