﻿using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hymperia.Model.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Materiaux",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nom = table.Column<string>(nullable: false),
                    Prix = table.Column<double>(nullable: false)
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
                    MateriauId = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    ProjetId = table.Column<int>(nullable: true),
                    Point1 = table.Column<string>(nullable: true),
                    Point2 = table.Column<string>(nullable: true),
                    Diametre = table.Column<double>(nullable: true),
                    InnerDiametre = table.Column<double>(nullable: true),
                    ThetaDiv = table.Column<int>(nullable: true),
                    Centre = table.Column<string>(nullable: true),
                    RayonX = table.Column<double>(nullable: true),
                    RayonY = table.Column<double>(nullable: true),
                    RayonZ = table.Column<double>(nullable: true),
                    PhiDiv = table.Column<int>(nullable: true),
                    Ellipsoide_ThetaDiv = table.Column<int>(nullable: true),
                    PrismeRectangulaire_Centre = table.Column<string>(nullable: true),
                    Hauteur = table.Column<double>(nullable: true),
                    Largeur = table.Column<double>(nullable: true),
                    Longueur = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Formes_Materiaux_MateriauId",
                        column: x => x.MateriauId,
                        principalTable: "Materiaux",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Formes_Projets_ProjetId",
                        column: x => x.ProjetId,
                        principalTable: "Projets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Acces",
                columns: table => new
                {
                    idProjet = table.Column<int>(nullable: false),
                    idUtilisateur = table.Column<int>(nullable: false),
                    DroitDAcces = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Acces", x => new { x.idProjet, x.idUtilisateur });
                    table.ForeignKey(
                        name: "FK_Acces_Projets_idProjet",
                        column: x => x.idProjet,
                        principalTable: "Projets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Acces_Utilisateurs_idUtilisateur",
                        column: x => x.idUtilisateur,
                        principalTable: "Utilisateurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Acces_idUtilisateur",
                table: "Acces",
                column: "idUtilisateur");

            migrationBuilder.CreateIndex(
                name: "IX_Formes_MateriauId",
                table: "Formes",
                column: "MateriauId");

            migrationBuilder.CreateIndex(
                name: "IX_Formes_ProjetId",
                table: "Formes",
                column: "ProjetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Acces");

            migrationBuilder.DropTable(
                name: "Formes");

            migrationBuilder.DropTable(
                name: "Utilisateurs");

            migrationBuilder.DropTable(
                name: "Materiaux");

            migrationBuilder.DropTable(
                name: "Projets");
        }
    }
}