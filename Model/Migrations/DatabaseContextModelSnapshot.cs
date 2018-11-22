﻿// <auto-generated />
using System;
using Hymperia.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hymperia.Model.Migrations
{
  [DbContext(typeof(DatabaseContext))]
  partial class DatabaseContextModelSnapshot : ModelSnapshot
  {
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
      modelBuilder
          .HasAnnotation("ProductVersion", "2.2.0-preview3-35497")
          .HasAnnotation("Relational:MaxIdentifierLength", 64);

      modelBuilder.Entity("Hymperia.Model.Modeles.Acces", b =>
          {
            b.Property<int>("IdProjet");

            b.Property<int>("IdUtilisateur");

            b.Property<string>("DroitDAcces")
                      .IsRequired();

            b.HasKey("IdProjet", "IdUtilisateur");

            b.HasIndex("IdUtilisateur");

            b.ToTable("Acces");
          });

      modelBuilder.Entity("Hymperia.Model.Modeles.Forme", b =>
          {
            b.Property<int>("Id")
                      .ValueGeneratedOnAdd();

            b.Property<string>("Discriminator")
                      .IsRequired();

            b.Property<int>("IdMateriau");

            b.Property<int?>("IdProjet");

            b.Property<string>("_Origine")
                      .IsRequired()
                      .HasColumnName("Origine");

            b.Property<string>("_Rotation")
                      .IsRequired()
                      .HasColumnName("Rotation");

            b.HasKey("Id");

            b.HasIndex("IdMateriau");

            b.HasIndex("IdProjet");

            b.ToTable("Formes");

            b.HasDiscriminator<string>("Discriminator").HasValue("Forme");
          });

      modelBuilder.Entity("Hymperia.Model.Modeles.Materiau", b =>
          {
            b.Property<int>("Id")
                      .ValueGeneratedOnAdd();

            b.Property<int>("A");

            b.Property<int>("B");

            b.Property<int>("G");

            b.Property<string>("Nom")
                      .IsRequired();

            b.Property<double>("Prix");

            b.Property<int>("R");

            b.HasKey("Id");

            b.HasAlternateKey("Nom");

            b.ToTable("Materiaux");
          });

      modelBuilder.Entity("Hymperia.Model.Modeles.Projet", b =>
          {
            b.Property<int>("Id")
                      .ValueGeneratedOnAdd();

            b.Property<string>("Nom")
                      .IsRequired();

            b.HasKey("Id");

            b.ToTable("Projets");
          });

      modelBuilder.Entity("Hymperia.Model.Modeles.Utilisateur", b =>
          {
            b.Property<int>("Id")
                      .ValueGeneratedOnAdd();

            b.Property<string>("MotDePasse")
                      .IsRequired();

            b.Property<string>("Nom")
                      .IsRequired();

            b.HasKey("Id");

            b.HasAlternateKey("Nom");

            b.ToTable("Utilisateurs");
          });

      modelBuilder.Entity("Hymperia.Model.Modeles.Ellipsoide", b =>
          {
            b.HasBaseType("Hymperia.Model.Modeles.Forme");

            b.Property<int>("PhiDiv");

            b.Property<double>("RayonX");

            b.Property<double>("RayonY");

            b.Property<double>("RayonZ");

            b.Property<int>("ThetaDiv");

            b.HasDiscriminator().HasValue("Ellipsoide");
          });

      modelBuilder.Entity("Hymperia.Model.Modeles.PrismeRectangulaire", b =>
          {
            b.HasBaseType("Hymperia.Model.Modeles.Forme");

            b.Property<double>("Hauteur")
                      .HasColumnName("PrismeRectangulaire_Hauteur");

            b.Property<double>("Largeur");

            b.Property<double>("Longueur");

            b.HasDiscriminator().HasValue("PrismeRectangulaire");
          });

      modelBuilder.Entity("Hymperia.Model.Modeles.ThetaDivForme", b =>
          {
            b.HasBaseType("Hymperia.Model.Modeles.Forme");

            b.Property<int>("ThetaDiv")
                      .HasColumnName("ThetaDivForme_ThetaDiv");

            b.HasDiscriminator().HasValue("ThetaDivForme");
          });

      modelBuilder.Entity("Hymperia.Model.Modeles.Cone", b =>
          {
            b.HasBaseType("Hymperia.Model.Modeles.ThetaDivForme");

            b.Property<double>("Hauteur");

            b.Property<double>("RayonBase");

            b.Property<double>("RayonTop");

            b.HasDiscriminator().HasValue("Cone");
          });

      modelBuilder.Entity("Hymperia.Model.Modeles.Cylindre", b =>
          {
            b.HasBaseType("Hymperia.Model.Modeles.ThetaDivForme");

            b.Property<double>("Diametre");

            b.Property<double>("Hauteur")
                      .HasColumnName("Cylindre_Hauteur");

            b.Property<double>("InnerDiametre");

            b.HasDiscriminator().HasValue("Cylindre");
          });

      modelBuilder.Entity("Hymperia.Model.Modeles.Acces", b =>
          {
            b.HasOne("Hymperia.Model.Modeles.Projet", "Projet")
                      .WithMany()
                      .HasForeignKey("IdProjet")
                      .OnDelete(DeleteBehavior.Cascade);

            b.HasOne("Hymperia.Model.Modeles.Utilisateur", "Utilisateur")
                      .WithMany("_Acces")
                      .HasForeignKey("IdUtilisateur")
                      .OnDelete(DeleteBehavior.Cascade);
          });

      modelBuilder.Entity("Hymperia.Model.Modeles.Forme", b =>
          {
            b.HasOne("Hymperia.Model.Modeles.Materiau", "Materiau")
                      .WithMany()
                      .HasForeignKey("IdMateriau")
                      .OnDelete(DeleteBehavior.Cascade);

            b.HasOne("Hymperia.Model.Modeles.Projet")
                      .WithMany("_Formes")
                      .HasForeignKey("IdProjet")
                      .OnDelete(DeleteBehavior.Cascade);
          });
#pragma warning restore 612, 618
    }
  }
}
