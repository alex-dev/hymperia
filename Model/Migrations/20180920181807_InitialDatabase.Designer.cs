﻿// <auto-generated />
using System;
using Hymperia.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Hymperia.Model.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20180920181807_InitialDatabase")]
    partial class InitialDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-preview1-35029")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Hymperia.Model.Modeles.Acces", b =>
                {
                    b.Property<int>("idProjet");

                    b.Property<int>("idUtilisateur");

                    b.Property<string>("DroitDAcces")
                        .IsRequired();

                    b.HasKey("idProjet", "idUtilisateur");

                    b.HasIndex("idUtilisateur");

                    b.ToTable("Acces");
                });

            modelBuilder.Entity("Hymperia.Model.Modeles.Forme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<int>("MateriauId");

                    b.Property<int?>("ProjetId");

                    b.HasKey("Id");

                    b.HasIndex("MateriauId");

                    b.HasIndex("ProjetId");

                    b.ToTable("Formes");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Forme");
                });

            modelBuilder.Entity("Hymperia.Model.Modeles.Materiau", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Nom")
                        .IsRequired();

                    b.Property<double>("Prix");

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

                    b.HasAlternateKey("Nom");

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

            modelBuilder.Entity("Hymperia.Model.Modeles.Cylindre", b =>
                {
                    b.HasBaseType("Hymperia.Model.Modeles.Forme");

                    b.Property<double>("Diametre");

                    b.Property<double>("InnerDiametre");

                    b.Property<string>("Point1");

                    b.Property<string>("Point2");

                    b.Property<int>("ThetaDiv");

                    b.HasDiscriminator().HasValue("Cylindre");
                });

            modelBuilder.Entity("Hymperia.Model.Modeles.Ellipsoide", b =>
                {
                    b.HasBaseType("Hymperia.Model.Modeles.Forme");

                    b.Property<string>("Centre");

                    b.Property<int>("PhiDiv");

                    b.Property<double>("RayonX");

                    b.Property<double>("RayonY");

                    b.Property<double>("RayonZ");

                    b.Property<int>("ThetaDiv")
                        .HasColumnName("Ellipsoide_ThetaDiv");

                    b.HasDiscriminator().HasValue("Ellipsoide");
                });

            modelBuilder.Entity("Hymperia.Model.Modeles.PrismeRectangulaire", b =>
                {
                    b.HasBaseType("Hymperia.Model.Modeles.Forme");

                    b.Property<string>("Centre")
                        .HasColumnName("PrismeRectangulaire_Centre");

                    b.Property<double>("Hauteur");

                    b.Property<double>("Largeur");

                    b.Property<double>("Longueur");

                    b.HasDiscriminator().HasValue("PrismeRectangulaire");
                });

            modelBuilder.Entity("Hymperia.Model.Modeles.Acces", b =>
                {
                    b.HasOne("Hymperia.Model.Modeles.Projet", "Projet")
                        .WithMany()
                        .HasForeignKey("idProjet")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Hymperia.Model.Modeles.Utilisateur", "Utilisateur")
                        .WithMany("_Acces")
                        .HasForeignKey("idUtilisateur")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Hymperia.Model.Modeles.Forme", b =>
                {
                    b.HasOne("Hymperia.Model.Modeles.Materiau", "Materiau")
                        .WithMany()
                        .HasForeignKey("MateriauId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Hymperia.Model.Modeles.Projet")
                        .WithMany("_Formes")
                        .HasForeignKey("ProjetId");
                });
#pragma warning restore 612, 618
        }
    }
}
