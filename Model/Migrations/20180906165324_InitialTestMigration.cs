using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hymperia.Model.Migrations
{
  public partial class InitialTestMigration : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
        name: "Utilisateurs",
        columns: table => new
        {
          Id = table.Column<int>(nullable: false)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
          Nom = table.Column<string>(maxLength: 25, nullable: false)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_Utilisateurs", x => x.Id);
          table.UniqueConstraint("AK_Utilisateurs_Nom", x => x.Nom);
        });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(name: "Utilisateurs");
    }
  }
}
