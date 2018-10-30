using System.Collections.Generic;
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
    public static async Task<LocalizedMateriau> GetMateriau([NotNull] string key) =>
      await DBManager.GetMateriau(key).ConfigureAwait(false);
    public static async Task<IDictionary<string, LocalizedMateriau>> LoadMateriaux() =>
      await DBManager.LoadMateriaux().ConfigureAwait(false);
  }
}
