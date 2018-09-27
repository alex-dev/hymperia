﻿using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Hymperia.Model.Migrations;
using Hymperia.Model.Modeles;

namespace Hymperia.Model
{
  public class DatabaseContext : DbContext
  {
    #region Constants

    private const string ConfigurationName = "MainDatabase";

    #endregion

    #region DBSets

    #region Fields
    private DbSet<Acces> acces;
    private DbSet<Materiau> materiaux;
    private DbSet<Projet> projets;
    private DbSet<Utilisateur> utilisateurs;
    #endregion

    /// <summary>Retourne le <see cref="DbSet{Acces}"/>.</summary>
    /// <remarks>
    ///   La syntaxe <code>acces ?? (acces = Set<Acces>())</code> retourne <see cref="acces"/>
    ///   s'il est connu (non <see cref="null"/>), sinon lui affecte un nouveau <see cref="DbSet{Acces}"/> puis le 
    ///   retourne.
    ///   Un accès "lazy" est préférable ici plutôt que de créer tous les <see cref="DbSet{T}"/> initialement,
    ///   ce qui peut être lourd.
    /// </remarks>
    [ItemNotNull]
    public DbSet<Acces> Acces
    {
      get => acces ?? (acces = Set<Acces>());
    }

    /// <summary>Retourne le <see cref="DbSet{Materiau}"/>.</summary>
    /// <remarks>
    ///   La syntaxe <code>materiaux ?? (materiaux = Set<Materiau>())</code> retourne <see cref="materiaux"/>
    ///   s'il est connu (non <see cref="null"/>), sinon lui affecte un nouveau <see cref="DbSet{Materiau}"/> puis le 
    ///   retourne.
    ///   Un accès "lazy" est préférable ici plutôt que de créer tous les <see cref="DbSet{T}"/> initialement,
    ///   ce qui peut être lourd.
    /// </remarks>
    [ItemNotNull]
    public DbSet<Materiau> Materiaux
    {
      get => materiaux ?? (materiaux = Set<Materiau>());
    }

    /// <summary>Retourne le <see cref="DbSet{Projet}"/>.</summary>
    /// <remarks>
    ///   La syntaxe <code>projets ?? (projets = Set<Projet>())</code> retourne <see cref="projets"/>
    ///   s'il est connu (non <see cref="null"/>), sinon lui affecte un nouveau <see cref="DbSet{Projet}"/> puis le 
    ///   retourne.
    ///   Un accès "lazy" est préférable ici plutôt que de créer tous les <see cref="DbSet{T}"/> initialement,
    ///   ce qui peut être lourd.
    /// </remarks>
    [ItemNotNull]
    public DbSet<Projet> Projets
    {
      get => projets ?? (projets = Set<Projet>());
    }

    /// <summary>Retourne le <see cref="DbSet{Utilisateur}"/>.</summary>
    /// <remarks>
    ///   La syntaxe <code>utilisateurs ?? (utilisateurs = Set<Utilisateur>())</code> retourne <see cref="utilisateurs"/>
    ///   s'il est connu (non <see cref="null"/>), sinon lui affecte un nouveau <see cref="DbSet{Utilisateur}"/> puis le 
    ///   retourne.
    ///   Un accès "lazy" est préférable ici plutôt que de créer tous les <see cref="DbSet{T}"/> initialement,
    ///   ce qui peut être lourd.
    /// </remarks>
    [ItemNotNull]
    public DbSet<Utilisateur> Utilisateurs
    {
      get => utilisateurs ?? (utilisateurs = Set<Utilisateur>());
    }

    #endregion

    #region Constructors

    /// <summary>Initialise le contexte selon les options par défaut.</summary>
    public DatabaseContext() : base() { }

    /// <summary>Initialise le contexte selon les options passées.</summary>
    /// <param name="options">Les options préconfigurées passées au contexte.</param>
    public DatabaseContext([NotNull] DbContextOptions<DatabaseContext> options)
      : base(options) { }

    #endregion

    #region Methods

    public async Task Migrate([NotNull] CancellationToken token = default)
    {
      await Database.EnsureDeletedAsync(token);
      await Database.MigrateAsync(token);
      await new Initializer().Initialize(this);
    }

    #endregion

    #region Events Override

    /// <inheritdoc/>
    protected override void OnModelCreating([NotNull] ModelBuilder builder)
    {
      builder.Entity<Utilisateur>().ToTable("Utilisateurs");
      builder.Entity<Utilisateur>().HasAlternateKey(utilisateur => utilisateur.Nom);
    
      builder.Entity<Materiau>().ToTable("Materiaux");
      builder.Entity<Materiau>().HasAlternateKey(materiau => materiau.Nom);

      builder.Entity<Forme>().ToTable("Formes");
      builder.Entity<Forme>().HasOne(forme => forme.Materiau).WithMany()
        .HasForeignKey("IdMateriau");
      builder.Entity<Forme>().Property(forme => forme._Origine).HasColumnName("Origine");
      builder.Entity<Forme>().Property(forme => forme._Rotation).HasColumnName("Rotation");
      builder.Entity<ThetaDivForme>().HasBaseType<Forme>();
      builder.Entity<Cone>().HasBaseType<ThetaDivForme>();
      builder.Entity<Cylindre>().HasBaseType<ThetaDivForme>();
      builder.Entity<Cylindre>().Property(forme => forme._Point).HasColumnName("Point");
      builder.Entity<Ellipsoide>().HasBaseType<Forme>();
      builder.Entity<PrismeRectangulaire>().HasBaseType<Forme>();

      builder.Entity<Projet>().ToTable("Projets");
      builder.Entity<Projet>().HasAlternateKey(projet => projet.Nom);
      builder.Entity<Projet>().HasMany(projet => projet._Formes).WithOne()
        .HasForeignKey("IdProjet");

      builder.Entity<Acces>().ToTable("Acces");
      builder.Entity<Acces>().Property<int>("IdProjet");
      builder.Entity<Acces>().Property<int>("IdUtilisateur");
      builder.Entity<Acces>().Property(acces => acces.DroitDAcces)
        .HasConversion(new EnumToStringConverter<Acces.Droit>());
      builder.Entity<Acces>().HasOne(acces => acces.Projet).WithMany()
        .HasForeignKey("IdProjet");
      builder.Entity<Acces>().HasOne(acces => acces.Utilisateur).WithMany(utilisateur => utilisateur._Acces)
        .HasForeignKey("IdUtilisateur");
      builder.Entity<Acces>().HasKey("IdProjet", "IdUtilisateur");

      base.OnModelCreating(builder);
    }

    /// <inheritdoc/>
    protected override void OnConfiguring([NotNull] DbContextOptionsBuilder builder)
    {
      builder.UseMySql(GetConnectionString());
      builder.EnableRichDataErrorHandling();
      builder.EnableSensitiveDataLogging();
      BaseOnConfiguring(builder);
    }

    protected void BaseOnConfiguring([NotNull] DbContextOptionsBuilder builder)
    {
      base.OnConfiguring(builder);
    }

    private string GetConnectionString()
    {
      const string connection = "Server=420.cstj.qc.ca; SslMode=Preferred; Database=hymperia_test_deploy; Username=Hymperia; Password=infoH25978;";

      try
      {
        return ConfigurationManager.ConnectionStrings[ConfigurationName]?.ConnectionString
          ?? connection;
      }
      catch (ConfigurationErrorsException)
      {
        return connection;
      }
    }

    #endregion
  }
}
