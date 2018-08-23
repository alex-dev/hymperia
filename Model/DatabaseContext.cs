using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace InfoApp.Model
{
  public class DatabaseContext : DbContext
  {
    const string ConfigurationName = "MainDatabase";

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseMySQL(ConfigurationManager.ConnectionStrings[ConfigurationName].ConnectionString);
    }
  }
}
