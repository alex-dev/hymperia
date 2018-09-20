using System;
using System.Configuration;
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
    private DbSet<Utilisateur> utilisateurs;
    private DbSet<Acces> acces;
    private DbSet<Materiau> materiaux;
    #endregion

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
      await Database.MigrateAsync(token);
      Task.WaitAll(new Initializer().Initialize(utilisateurs, acces, materiaux));
      await SaveChangesAsync();
    }

    #endregion

    #region Events Override

    /// <inheritdoc/>
    protected override void OnModelCreating([NotNull] ModelBuilder builder)
    {
      builder.Entity<Materiau>().HasAlternateKey(materiau => materiau.Nom);
      builder.Entity<Projet>().HasAlternateKey(projet => projet.Nom);
      builder.Entity<Utilisateur>().HasAlternateKey(utilisateur => utilisateur.Nom);
      builder.Entity<Acces>().HasAlternateKey(acces => new { acces.Projet, acces.Utilisateur });
      builder.Entity<Acces>().Property(acces => acces.DroitDAcces)
        .HasConversion(new EnumToStringConverter<Acces.Droit>());
      base.OnModelCreating(builder);
    }

    /// <inheritdoc/>
    protected override void OnConfiguring([NotNull] DbContextOptionsBuilder builder)
    {
      string connection = ConfigurationManager.ConnectionStrings[ConfigurationName]?.ConnectionString
          ?? "Server=420.cstj.qc.ca; SslMode=Preferred; Database=hymperia_test_deploy; Username=Hymperia; Password=infoH25978;";
      builder.UseMySql(connection);
      base.OnConfiguring(builder);
    }

    #endregion
  }
}
