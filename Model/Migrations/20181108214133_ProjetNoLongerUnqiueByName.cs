using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hymperia.Model.Migrations.Database
{
  public partial class ProjetNoLongerUnqiueByName : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder) =>
      migrationBuilder.DropIndex(
        name: "AK_Projets_Nom",
        table: "Projets");

    protected override void Down(MigrationBuilder migrationBuilder) =>
      migrationBuilder.AddUniqueConstraint(
        name: "AK_Projets_Nom",
        table: "Projets",
        column: "Nom");

  }
}
