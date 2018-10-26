using Microsoft.EntityFrameworkCore.Migrations;

namespace Hymperia.Model.Migrations
{
  public partial class Cascade : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_Formes_Projets_IdProjet",
          table: "Formes");

      migrationBuilder.AddForeignKey(
          name: "FK_Formes_Projets_IdProjet",
          table: "Formes",
          column: "IdProjet",
          principalTable: "Projets",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_Formes_Projets_IdProjet",
          table: "Formes");

      migrationBuilder.AddForeignKey(
          name: "FK_Formes_Projets_IdProjet",
          table: "Formes",
          column: "IdProjet",
          principalTable: "Projets",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);
    }
  }
}
