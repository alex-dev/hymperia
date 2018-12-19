using Microsoft.EntityFrameworkCore.Migrations;

namespace Hymperia.Model.Migrations
{
  public partial class UserSettings : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterColumn<string>(
          name: "Nom",
          table: "Utilisateurs",
          maxLength: 250,
          nullable: false,
          oldClrType: typeof(string));

      migrationBuilder.AlterColumn<string>(
          name: "MotDePasse",
          table: "Utilisateurs",
          maxLength: 250,
          nullable: false,
          oldClrType: typeof(string));

      migrationBuilder.AddColumn<string>(
          name: "Langue",
          table: "Utilisateurs",
          maxLength: 5,
          nullable: false,
          defaultValue: "fr-CA");

      migrationBuilder.AddColumn<string>(
          name: "Theme",
          table: "Utilisateurs",
          maxLength: 20,
          nullable: false,
          defaultValue: "Dark");

      migrationBuilder.AlterColumn<string>(
          name: "Nom",
          table: "Projets",
          maxLength: 250,
          nullable: false,
          oldClrType: typeof(string));

      migrationBuilder.AlterColumn<string>(
          name: "Nom",
          table: "Materiaux",
          maxLength: 100,
          nullable: false,
          oldClrType: typeof(string));
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "Langue",
          table: "Utilisateurs");

      migrationBuilder.DropColumn(
          name: "Theme",
          table: "Utilisateurs");

      migrationBuilder.AlterColumn<string>(
          name: "Nom",
          table: "Utilisateurs",
          nullable: false,
          oldClrType: typeof(string),
          oldMaxLength: 250);

      migrationBuilder.AlterColumn<string>(
          name: "MotDePasse",
          table: "Utilisateurs",
          nullable: false,
          oldClrType: typeof(string),
          oldMaxLength: 250);

      migrationBuilder.AlterColumn<string>(
          name: "Nom",
          table: "Projets",
          nullable: false,
          oldClrType: typeof(string),
          oldMaxLength: 250);

      migrationBuilder.AlterColumn<string>(
          name: "Nom",
          table: "Materiaux",
          nullable: false,
          oldClrType: typeof(string),
          oldMaxLength: 100);
    }
  }
}
