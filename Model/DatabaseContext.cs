using System.Configuration;
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

    public DatabaseContext([CanBeNull] DbContextOptions<DatabaseContext> options = null)
      : base(BuildOptions(options))
    {
      users = null;
    }

    private static DbContextOptions<DatabaseContext> BuildOptions([CanBeNull] DbContextOptions<DatabaseContext> options)
    {
      return (options is null
          ? new DbContextOptionsBuilder<DatabaseContext>()
          : new DbContextOptionsBuilder<DatabaseContext>(options))
        .UseMySQL(ConfigurationManager.ConnectionStrings[ConfigurationName].ConnectionString)
        .Options;
    }

    #endregion

    #region Events Override



    #endregion
  }
}
