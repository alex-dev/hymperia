using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using Hymperia.Model.Localization;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Hymperia.Model
{
  /// <summary><see cref="DbContext"/> spécialisé pour la localization.</summary>
  public class LocalizationContext : DbContext
  {
    #region Constants

    [NotNull]
    public const string ConfigurationName = "LocalizationDatabase";
    [CanBeNull]
    private readonly string Connection;

    #endregion

    #region DBSets

    #region Fields
    private DbSet<LocalizedMateriau> materiaux;
    #endregion

    [NotNull]
    [ItemNotNull]
    public DbSet<LocalizedMateriau> Materiaux
    {
      get => materiaux ?? (materiaux = Set<LocalizedMateriau>());
    }

    #endregion

    #region Constructors

    /// <summary>Initialise le contexte selon les options par défaut.</summary>
    public LocalizationContext() { }

    /// <summary>Initialise le contexte selon les options passées.</summary>
    /// <param name="options">Les options préconfigurées passées au contexte.</param>
    public LocalizationContext([NotNull] DbContextOptions<DatabaseContext> options)
      : base(options) { }

    /// <summary>Initialise le base de donnée selon la connection string passée.</summary>
    /// <param name="connection">Connection string.</param>
    public LocalizationContext([NotNull] string connection)
    {
      Connection = connection;
    }

    #endregion

    #region Methods

    public async Task Migrate(bool initialize = false, [NotNull] CancellationToken token = default)
    {
      if (initialize)
        await Database.EnsureDeletedAsync(token);

      await Database.MigrateAsync(token);
    }

    #endregion

    #region Events Override

    /// <inheritdoc/>
    protected override void OnModelCreating([NotNull] ModelBuilder builder)
    {
      builder.Entity<LocalizedMateriau>().ToTable("Materiaux");
      builder.Entity<LocalizedMateriau>()
        .HasAlternateKey(materiau => new { materiau.StringKey, materiau.CultureKey });
      builder.Entity<LocalizedMateriau>()
        .HasAlternateKey(materiau => new { materiau.CultureKey, materiau.Nom });

      base.OnModelCreating(builder);
    }

    /// <inheritdoc/>
    protected override void OnConfiguring([NotNull] DbContextOptionsBuilder builder)
    {
      builder.UseMySql(string.IsNullOrWhiteSpace(Connection) ? GetConnectionString() : Connection);
      builder.EnableRichDataErrorHandling();
      builder.EnableSensitiveDataLogging();
      base.OnConfiguring(builder);
    }

    [NotNull]
    protected static string GetConnectionString()
    {
      string connection = $"Server=localhost; SslMode=Preferred; Database=hymperia_localization_{ Guid.NewGuid().ToString("N") }; Username=root; Password=;";

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
