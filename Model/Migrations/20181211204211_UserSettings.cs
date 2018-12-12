using Microsoft.EntityFrameworkCore.Migrations;

namespace Hymperia.Model.Migrations.Database
{
  public partial class UserSettings : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<string>(
          name: "Langue",
          table: "Utilisateurs",
          nullable: false,
          defaultValue: "");

      migrationBuilder.AddColumn<string>(
          name: "Theme",
          table: "Utilisateurs",
          nullable: false,
          defaultValue: "");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "Langue",
          table: "Utilisateurs");

      migrationBuilder.DropColumn(
          name: "Theme",
          table: "Utilisateurs");
    }
  }
}
