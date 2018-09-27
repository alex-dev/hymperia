using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.Moq;

namespace UnitTests.ModelIntegrationTests
{
  [TestClass]
  public class MockedContextIsWorking
  {
    private string DatabaseString { get; set; }

    [TestInitialize]
    public async Task Initialize()
    {
      using (var context = new MockedContext())
      {
        DatabaseString = context.DatabaseString;
        await context.Migrate();
      }
    }

    [TestCleanup]
    public async Task Cleanup()
    {
      using (var context = new MockedContext(DatabaseString))
      {
        await context.Database.EnsureDeletedAsync();
      }
    }

    [TestMethod]
    public async Task ShouldGenerateInMemoryContext()
    {
      using (var context = new MockedContext(DatabaseString))
      {
        try
        {
          await context.Projets.CountAsync();
        }
        catch (Exception)
        {
          Assert.Fail();
        }
      }
    }
  }
}
