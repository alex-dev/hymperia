using Microsoft.EntityFrameworkCore.Migrations;

namespace Hymperia.Model.Migrations
{
  public partial class Rotation : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterColumn<string>(
          name: "Origine",
          table: "Formes",
          nullable: false,
          oldClrType: typeof(string));

      migrationBuilder.AlterColumn<string>(
          name: "Point",
          table: "Formes",
          nullable: true,
          oldClrType: typeof(string),
          oldNullable: true);

      migrationBuilder.AddColumn<string>(
          name: "Rotation",
          table: "Formes",
          nullable: false);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "Rotation",
          table: "Formes");

      migrationBuilder.AlterColumn<string>(
          name: "Origine",
          table: "Formes",
          nullable: false,
          oldClrType: typeof(string));

      migrationBuilder.AlterColumn<string>(
          name: "Point",
          table: "Formes",
          nullable: true,
          oldClrType: typeof(string),
          oldNullable: true);
    }
  }
}
