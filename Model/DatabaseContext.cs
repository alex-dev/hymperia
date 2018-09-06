using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Hymperia.Model
{
  public class DatabaseContext : DbContext
  {
    #region Constants

    private const string ConfigurationName = "MainDatabase";

    #endregion

    #region DBSets

    #region Fields
    private DbSet<User> users;
    #endregion

    /// <summary>Retourne le <see cref="DbSet{User}"/>.</summary>
    /// <remarks>
    ///   La syntaxe <code>users ?? (users = Set<User>())</code> retourne users s'il est connu (non <see cref="null"/>),
    ///   sinon l'affecte à un nouveau <see cref="DbSet{User}"/> puis le retourne.
    ///   Un accès "lazy" est préférable ici plutôt que de créer tous les set initialement, ce qui peut être lourd.
    /// </remarks>
    public DbSet<User> Users
    {
      get => users ?? (users = Set<User>());
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

    public async Task Migrate(CancellationToken token = default)
    {
      await Database.MigrateAsync(token);
    }

    #endregion

    #region Events Override

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.Entity<User>().HasAlternateKey(user => user.Name);
      base.OnModelCreating(builder);
    }

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
      builder.UseMySql(ConfigurationManager.ConnectionStrings[ConfigurationName].ConnectionString);
      base.OnConfiguring(builder);
    }

    #endregion
  }
}
