using System.Threading.Tasks;
using Hymperia.Model;

namespace ModelIntegrationTests.Base
{
  public abstract class MockedContext : DatabaseContext
  {
    public new static string GetConnectionString() => DatabaseContext.GetConnectionString();

    public static DatabaseContext Create(string connection)
    {
      return new DatabaseContext(connection);
    }

    public static async Task Migrate(DatabaseContext context)
    {
      await context.Migrate(true);
    }

    public static async Task Delete(DatabaseContext context)
    {
      await context.Database.EnsureDeletedAsync();
    }
  }
}
