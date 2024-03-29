﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Hymperia.Model.Migrations;
using Hymperia.Model.Modeles;
using Hymperia.Model.Properties;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Hymperia.Model
{
  public class DatabaseContext : DbContext
  {
    #region Constants

    [NotNull]
    public const string ConfigurationName = "MainDatabase";
    private readonly bool UseSettings;

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
    [NotNull]
    [ItemNotNull]
    public DbSet<Acces> Acces => acces ?? (acces = Set<Acces>());

    /// <summary>Retourne le <see cref="DbSet{Materiau}"/>.</summary>
    /// <remarks>
    ///   La syntaxe <code>materiaux ?? (materiaux = Set<Materiau>())</code> retourne <see cref="materiaux"/>
    ///   s'il est connu (non <see cref="null"/>), sinon lui affecte un nouveau <see cref="DbSet{Materiau}"/> puis le 
    ///   retourne.
    ///   Un accès "lazy" est préférable ici plutôt que de créer tous les <see cref="DbSet{T}"/> initialement,
    ///   ce qui peut être lourd.
    /// </remarks>
    [NotNull]
    [ItemNotNull]
    public DbSet<Materiau> Materiaux => materiaux ?? (materiaux = Set<Materiau>());

    /// <summary>Retourne le <see cref="DbSet{Projet}"/>.</summary>
    /// <remarks>
    ///   La syntaxe <code>projets ?? (projets = Set<Projet>())</code> retourne <see cref="projets"/>
    ///   s'il est connu (non <see cref="null"/>), sinon lui affecte un nouveau <see cref="DbSet{Projet}"/> puis le 
    ///   retourne.
    ///   Un accès "lazy" est préférable ici plutôt que de créer tous les <see cref="DbSet{T}"/> initialement,
    ///   ce qui peut être lourd.
    /// </remarks>
    [NotNull]
    [ItemNotNull]
    public DbSet<Projet> Projets => projets ?? (projets = Set<Projet>());

    /// <summary>Retourne le <see cref="DbSet{Utilisateur}"/>.</summary>
    /// <remarks>
    ///   La syntaxe <code>utilisateurs ?? (utilisateurs = Set<Utilisateur>())</code> retourne <see cref="utilisateurs"/>
    ///   s'il est connu (non <see cref="null"/>), sinon lui affecte un nouveau <see cref="DbSet{Utilisateur}"/> puis le 
    ///   retourne.
    ///   Un accès "lazy" est préférable ici plutôt que de créer tous les <see cref="DbSet{T}"/> initialement,
    ///   ce qui peut être lourd.
    /// </remarks>
    [NotNull]
    [ItemNotNull]
    public DbSet<Utilisateur> Utilisateurs => utilisateurs ?? (utilisateurs = Set<Utilisateur>());

    #endregion

    #region Constructors

    /// <summary>Initialise le contexte selon les options par défaut.</summary>
    public DatabaseContext()
    {
      UseSettings = true;
    }

    /// <summary>Initialise le contexte selon les options passées.</summary>
    /// <param name="options">Les options préconfigurées passées au contexte.</param>
    public DatabaseContext([NotNull] DbContextOptions<DatabaseContext> options)
      : base(options) { }

    /// <summary>Initialise le base de donnée selon la connection string passée.</summary>
    /// <param name="connection">Connection string.</param>
    public DatabaseContext(bool useSettings)
    {
      UseSettings = useSettings;
    }

    #endregion

    #region Methods

    public async Task Migrate(bool initialize = false, [NotNull] CancellationToken token = default)
    {
      if (initialize)
        await Database.EnsureDeletedAsync(token).ConfigureAwait(false);

      await Database.MigrateAsync(token).ConfigureAwait(false);

      if (initialize)
        await new Initializer().Initialize(this, token).ConfigureAwait(false);
    }

    #endregion

    #region Events Override

    /// <inheritdoc/>
    protected override void OnModelCreating([NotNull] ModelBuilder builder)
    {
      builder.Entity<Utilisateur>().ToTable("Utilisateurs");
      builder.Entity<Utilisateur>().HasAlternateKey(utilisateur => utilisateur.Nom);
      builder.Entity<Utilisateur>().Property(user => user.Langue).HasDefaultValue("fr-CA");
      builder.Entity<Utilisateur>().Property(user => user.Theme).HasDefaultValue("Dark");

      builder.Entity<Materiau>().ToTable("Materiaux");
      builder.Entity<Materiau>().Property(materiau => materiau.R).HasConversion<int>();
      builder.Entity<Materiau>().Property(materiau => materiau.G).HasConversion<int>();
      builder.Entity<Materiau>().Property(materiau => materiau.B).HasConversion<int>();
      builder.Entity<Materiau>().Property(materiau => materiau.A).HasConversion<int>();
      builder.Entity<Materiau>().HasAlternateKey(materiau => materiau.Nom);

      builder.Entity<Forme>().ToTable("Formes");
      builder.Entity<Forme>().Property(forme => forme._Origine).HasColumnName("Origine");
      builder.Entity<Forme>().Property(forme => forme._Rotation).HasColumnName("Rotation");
      builder.Entity<Forme>().HasOne(forme => forme.Materiau).WithMany()
        .HasForeignKey("IdMateriau");
      builder.Entity<ThetaDivForme>().HasBaseType<Forme>();
      builder.Entity<Cone>().HasBaseType<ThetaDivForme>();
      builder.Entity<Cylindre>().HasBaseType<ThetaDivForme>();
      builder.Entity<Ellipsoide>().HasBaseType<Forme>();
      builder.Entity<PrismeRectangulaire>().HasBaseType<Forme>();

      builder.Entity<Projet>().ToTable("Projets");
      builder.Entity<Projet>().HasMany(projet => projet._Formes).WithOne()
        .HasForeignKey("IdProjet").OnDelete(DeleteBehavior.Cascade);

      builder.Entity<Acces>().ToTable("Acces");
      builder.Entity<Acces>().Property<int>("IdProjet");
      builder.Entity<Acces>().Property<int>("IdUtilisateur");
      builder.Entity<Acces>().Property(acces => acces.DroitDAcces)
        .HasConversion<string>();
      builder.Entity<Acces>().HasOne(acces => acces.Projet).WithMany()
        .HasForeignKey("IdProjet").OnDelete(DeleteBehavior.Cascade);
      builder.Entity<Acces>().HasOne(acces => acces.Utilisateur).WithMany(utilisateur => utilisateur._Acces)
        .HasForeignKey("IdUtilisateur");
      builder.Entity<Acces>().HasKey("IdProjet", "IdUtilisateur");

      base.OnModelCreating(builder);
    }

    /// <inheritdoc/>
    protected override void OnConfiguring([NotNull] DbContextOptionsBuilder builder)
    {
      builder.UseMySql(UseSettings ? GetConnectionString() : RandomConnection());
      builder.EnableDetailedErrors();
      builder.EnableSensitiveDataLogging();
      base.OnConfiguring(builder);
    }

    [NotNull]
    protected static string GetConnectionString() => Settings.Default.MainDatabase ?? RandomConnection();
    [NotNull]
    protected static string RandomConnection() => $"Server=localhost; SslMode=Preferred; Database=hymperia_{ Guid.NewGuid().ToString("N") }; Username=root; Password=;";

    #endregion
  }
}
