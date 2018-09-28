using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hymperia.Model;
using ModelIntegrationTests.Base;

namespace ModelIntegrationTests
{
  public class BaseTest
  {
    private string DatabaseString { get; set; }

    protected DatabaseContext GetDatabase()
    {
      return MockedContext.Create(DatabaseString);
    }

    [TestInitialize]
    public async Task Initialize()
    {
      DatabaseString = MockedContext.GetConnectionString();

      using (var context = MockedContext.Create(DatabaseString))
      {
        await MockedContext.Migrate(context);
      }
    }

    [TestCleanup]
    public async Task Cleanup()
    {
      using (var context = MockedContext.Create(DatabaseString))
      {
        await MockedContext.Delete(context);
      }
    }
  }
}
