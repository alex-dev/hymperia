using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
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
    private DbSet<Projet> projets;
    private DbSet<Materiau> materiau;
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

    /// <summary>Retourne le <see cref="DbSet{Materiau}"/>.</summary>
    /// <remarks>
    ///   La syntaxe <code>materiau ?? (materiau = Set<Materiau>())</code> retourne <see cref="materiau"/>
    ///   s'il est connu (non <see cref="null"/>), sinon lui affecte un nouveau <see cref="DbSet{Materiau}"/> puis le 
    ///   retourne.
    ///   Un accès "lazy" est préférable ici plutôt que de créer tous les <see cref="DbSet{T}"/> initialement,
    ///   ce qui peut être lourd.
    /// </remarks>
    [ItemNotNull]
    public DbSet<Materiau> Materiau
    {
      get => materiau ?? (materiau = Set<Materiau>());
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
    }

    #endregion

    #region Events Override

    /// <inheritdoc/>
    protected override void OnModelCreating([NotNull] ModelBuilder builder)
    {
      builder.Entity<Utilisateur>().HasAlternateKey(Utilisateur => Utilisateur.Nom);
      builder.Entity<Acces>().HasKey(Acces => new { Acces.Projet, Acces.Utilisateur });
      base.OnModelCreating(builder);
    }

    /// <inheritdoc/>
    protected override void OnConfiguring([NotNull] DbContextOptionsBuilder builder)
    {
      builder.UseMySql(ConfigurationManager.ConnectionStrings[ConfigurationName].ConnectionString);
      base.OnConfiguring(builder);
    }

    #endregion
  }
}
