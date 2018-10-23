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
                { 1, "Bois", "fr_CA", "Bois" },
                { 2, "Acier", "fr_CA", "Acier" },
                { 3, "Cuivre", "fr_CA", "Cuivre" },
                { 4, "Or", "fr_CA", "Or" },
                { 5, "Bois", "en_US", "Wood" },
                { 6, "Acier", "en_US", "Steel" },
                { 7, "Cuivre", "en_US", "Copper" },
                { 8, "Or", "en_US", "Gold" }
        });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "Materiaux");
    }
  }
}
