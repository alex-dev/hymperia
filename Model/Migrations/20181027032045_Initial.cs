﻿using System.Drawing;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hymperia.Model.Migrations
{
  public partial class Initial : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      var wood = Color.FromKnownColor(KnownColor.SaddleBrown);
      var steel = Color.FromKnownColor(KnownColor.Gray);
      var copper = Color.FromKnownColor(KnownColor.DarkOrange);
      var gold = Color.FromKnownColor(KnownColor.Gold);

      migrationBuilder.CreateTable(
        name: "Materiaux",
        columns: table => new
        {
          Id = table.Column<int>(nullable: false)
            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
          Nom = table.Column<string>(nullable: false),
          Prix = table.Column<double>(nullable: false),
          R = table.Column<int>(nullable: false),
          G = table.Column<int>(nullable: false),
          B = table.Column<int>(nullable: false),
          A = table.Column<int>(nullable: false)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_Materiaux", x => x.Id);
          table.UniqueConstraint("AK_Materiaux_Nom", x => x.Nom);
        });

      migrationBuilder.CreateTable(
        name: "Projets",
        columns: table => new
        {
          Id = table.Column<int>(nullable: false)
            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
          Nom = table.Column<string>(nullable: false)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_Projets", x => x.Id);
          table.UniqueConstraint("AK_Projets_Nom", x => x.Nom);
        });

      migrationBuilder.CreateTable(
        name: "Utilisateurs",
        columns: table => new
        {
          Id = table.Column<int>(nullable: false)
            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
          Nom = table.Column<string>(nullable: false),
          MotDePasse = table.Column<string>(nullable: false)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_Utilisateurs", x => x.Id);
          table.UniqueConstraint("AK_Utilisateurs_Nom", x => x.Nom);
        });

      migrationBuilder.CreateTable(
        name: "Formes",
        columns: table => new
        {
          Id = table.Column<int>(nullable: false)
            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
          Origine = table.Column<string>(nullable: false),
          Rotation = table.Column<string>(nullable: false),
          IdMateriau = table.Column<int>(nullable: false),
          Discriminator = table.Column<string>(nullable: false),
          IdProjet = table.Column<int>(nullable: true),
          RayonX = table.Column<double>(nullable: true),
          RayonY = table.Column<double>(nullable: true),
          RayonZ = table.Column<double>(nullable: true),
          PhiDiv = table.Column<int>(nullable: true),
          ThetaDiv = table.Column<int>(nullable: true),
          PrismeRectangulaire_Hauteur = table.Column<double>(nullable: true),
          Largeur = table.Column<double>(nullable: true),
          Longueur = table.Column<double>(nullable: true),
          ThetaDivForme_ThetaDiv = table.Column<int>(nullable: true),
          Hauteur = table.Column<double>(nullable: true),
          RayonBase = table.Column<double>(nullable: true),
          RayonTop = table.Column<double>(nullable: true),
          Cylindre_Hauteur = table.Column<double>(nullable: true),
          Diametre = table.Column<double>(nullable: true),
          InnerDiametre = table.Column<double>(nullable: true)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_Formes", x => x.Id);
          table.ForeignKey(
            name: "FK_Formes_Materiaux_IdMateriau",
            column: x => x.IdMateriau,
            principalTable: "Materiaux",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
          table.ForeignKey(
            name: "FK_Formes_Projets_IdProjet",
            column: x => x.IdProjet,
            principalTable: "Projets",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
        });

      migrationBuilder.CreateTable(
        name: "Acces",
        columns: table => new
        {
          IdProjet = table.Column<int>(nullable: false),
          IdUtilisateur = table.Column<int>(nullable: false),
          DroitDAcces = table.Column<string>(nullable: false)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_Acces", x => new { x.IdProjet, x.IdUtilisateur });
          table.ForeignKey(
            name: "FK_Acces_Projets_IdProjet",
            column: x => x.IdProjet,
            principalTable: "Projets",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
          table.ForeignKey(
            name: "FK_Acces_Utilisateurs_IdUtilisateur",
            column: x => x.IdUtilisateur,
            principalTable: "Utilisateurs",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
        });

      migrationBuilder.CreateIndex(
        name: "IX_Acces_IdUtilisateur",
        table: "Acces",
        column: "IdUtilisateur");

      migrationBuilder.CreateIndex(
        name: "IX_Formes_IdMateriau",
        table: "Formes",
        column: "IdMateriau");

      migrationBuilder.CreateIndex(
        name: "IX_Formes_IdProjet",
        table: "Formes",
        column: "IdProjet");

      migrationBuilder.InsertData(
        "Materiaux",
        new string[] { "Id", "Nom", "Prix", "R", "G", "B", "A" },
        new object[,]
        {
          { 1, "Bois", 252.59, wood.R, wood.G, wood.B, wood.A },
          { 2, "Acier", 7785.63, steel.R, steel.G, steel.B, steel.A },
          { 3, "Cuivre", 791362.18, copper.R, copper.G, copper.B, copper.A },
          { 4, "Or", 923582368875.00, gold.R, gold.G, gold.B, gold.A },
          { 5, "Verre", 11934.65, 173, 195, 199, 191 }
        });
      }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(name: "Acces");
      migrationBuilder.DropTable(name: "Formes");
      migrationBuilder.DropTable(name: "Utilisateurs");
      migrationBuilder.DropTable(name: "Materiaux");
      migrationBuilder.DropTable(name: "Projets");
    }
  }
}
