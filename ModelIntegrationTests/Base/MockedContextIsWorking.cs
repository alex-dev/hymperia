using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModelIntegrationTests.Base
{
  [TestClass]
  public class MockedContextIsWorking : BaseTest
  {
    [TestMethod]
    public async Task ShouldGenerateTemporaryContext()
    {
      using (var context = GetDatabase())
      {
        await context.Projets.CountAsync();
      }
    }
  }
}
