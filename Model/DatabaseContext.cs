using System.Configuration;
using Microsoft.EntityFrameworkCore;
using JetBrains.Annotations;

namespace Hymperia.Model
{
  public class DatabaseContext : DbContext
  {
    #region Constants

    private const string ConfigurationName = "MainDatabase";

    #endregion

    #region DBSets

    public DbSet<User> Users { get; set; }

    #endregion

    #region Constructors

    public DatabaseContext([CanBeNull] DbContextOptions<DatabaseContext> options = null)
      : base(BuildOptions(options)) { }

    private static DbContextOptions<DatabaseContext> BuildOptions([CanBeNull] DbContextOptions<DatabaseContext> options)
    {
      return (options is null
          ? new DbContextOptionsBuilder<DatabaseContext>()
          : new DbContextOptionsBuilder<DatabaseContext>(options))
        .UseMySQL(ConfigurationManager.ConnectionStrings[ConfigurationName].ConnectionString)
        .Options;
    }

    #endregion
  }
}
