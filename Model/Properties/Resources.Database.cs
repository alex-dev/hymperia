using System.Threading.Tasks;
using Hymperia.Model.DatabaseResources;
using Hymperia.Model.Localization;
using JetBrains.Annotations;

namespace Hymperia.Model.Properties
{
  public static partial class Resources
  {
    private static readonly ResourceManager DBManager = new ResourceManager();

    [CanBeNull]
    internal static async Task<LocalizedMateriau> GetMateriau([NotNull] string key) => await DBManager.GetMateriau(key);
  }
}
