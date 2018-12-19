using Microsoft.EntityFrameworkCore.Migrations;

namespace Hymperia.Model.Migrations
{
  public partial class Length : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterColumn<string>(
          name: "Nom",
          table: "Materiaux",
          maxLength: 100,
          nullable: false,
          oldClrType: typeof(string));

      migrationBuilder.AlterColumn<string>(
          name: "CultureKey",
          table: "Materiaux",
          maxLength: 5,
          nullable: false,
          oldClrType: typeof(string));

      migrationBuilder.AlterColumn<string>(
          name: "StringKey",
          table: "Materiaux",
          maxLength: 100,
          nullable: false,
          oldClrType: typeof(string));
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterColumn<string>(
          name: "Nom",
          table: "Materiaux",
          nullable: false,
          oldClrType: typeof(string),
          oldMaxLength: 100);

      migrationBuilder.AlterColumn<string>(
          name: "CultureKey",
          table: "Materiaux",
          nullable: false,
          oldClrType: typeof(string),
          oldMaxLength: 5);

      migrationBuilder.AlterColumn<string>(
          name: "StringKey",
          table: "Materiaux",
          nullable: false,
          oldClrType: typeof(string),
          oldMaxLength: 100);
    }
  }
}
