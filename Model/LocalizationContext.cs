using System;
using System.Threading;
using System.Threading.Tasks;
using Hymperia.Model.Localization;
using Hymperia.Model.Properties;
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
    private readonly bool UseSettings;

    #endregion

    #region DBSets

    #region Fields
    private DbSet<LocalizedMateriau> materiaux;
    #endregion

    [NotNull]
    [ItemNotNull]
    public DbSet<LocalizedMateriau> Materiaux => materiaux ?? (materiaux = Set<LocalizedMateriau>());

    #endregion

    #region Constructors

    /// <summary>Initialise le contexte selon les options par défaut.</summary>
    public LocalizationContext()
    {
      UseSettings = true;
    }

    /// <summary>Initialise le contexte selon les options passées.</summary>
    /// <param name="options">Les options préconfigurées passées au contexte.</param>
    public LocalizationContext([NotNull] DbContextOptions<DatabaseContext> options)
      : base(options) { }

    /// <summary>Initialise le base de donnée selon la connection string passée.</summary>
    /// <param name="connection">Connection string.</param>
    public LocalizationContext(bool useSettings)
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
    }

    #endregion

    #region Events Override

    /// <inheritdoc/>
    protected override void OnModelCreating([NotNull] ModelBuilder builder)
    {
      builder.Entity<LocalizedMateriau>().ToTable("Materiaux");
      builder.Entity<LocalizedMateriau>()
        .HasKey(materiau => new { materiau.StringKey, materiau.CultureKey });
      builder.Entity<LocalizedMateriau>()
        .HasAlternateKey(materiau => new { materiau.CultureKey, materiau.Nom });

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
    protected static string GetConnectionString() => Settings.Default.LocalizationDatabase ?? RandomConnection();
    [NotNull]
    protected static string RandomConnection() => $"Server=localhost; SslMode=Preferred; Database=hymperia_localization_{ Guid.NewGuid().ToString("N") }; Username=root; Password=;";

    #endregion
  }
}
