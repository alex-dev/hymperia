using System;
using System.Drawing;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hymperia.Model.Migrations
{
  public partial class MateriauFill : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<string>(
          name: "Fill",
          table: "Materiaux",
          nullable: true);

      migrationBuilder.UpdateData("Materiaux", "Nom", "Bois", "Fill", new JsonObject<Brush>(Brushes.SaddleBrown).Json);
      migrationBuilder.UpdateData("Materiaux", "Nom", "Acier", "Fill", new JsonObject<Brush>(Brushes.Gray).Json);
      migrationBuilder.UpdateData("Materiaux", "Nom", "Cuivre", "Fill", new JsonObject<Brush>(Brushes.DarkOrange).Json);
      migrationBuilder.UpdateData("Materiaux", "Nom", "Or", "Fill", new JsonObject<Brush>(Brushes.Gold).Json);

      migrationBuilder.AlterColumn<string>(
          name: "Fill",
          table: "Materiaux",
          nullable: false);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "Fill",
          table: "Materiaux");

    }
  }
}
