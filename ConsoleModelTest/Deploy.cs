using System.Threading.Tasks;
using Hymperia.Model;

namespace Hymperia.ConsoleModelTest
{
  internal static class Deploy
  {
    public static async Task Migrate(bool initialize)
    {
      using (var context = new DatabaseContext())
      {
        await context.Migrate(initialize);
      }
    }
  }
}
