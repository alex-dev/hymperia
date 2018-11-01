using System.Threading.Tasks;
using Hymperia.Model;

namespace Hymperia.DatabaseTools
{
  internal static class Deploy
  {
    public static async Task Migrate(bool initialize)
    {
      await Task.WhenAll(
        MigrateMain(initialize),
        MigrateLocalized(initialize)).ConfigureAwait(false);
    }

    private static async Task MigrateMain(bool intialize)
    {
      using (var context = new DatabaseContext())
        await context.Migrate(intialize).ConfigureAwait(false);
    }

    private static async Task MigrateLocalized(bool intialize)
    {
      using (var context = new LocalizationContext())
        await context.Migrate(intialize).ConfigureAwait(false);
    }
  }
}
