using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Hymperia.Model.Identity;

namespace Hymperia.Model.DatabaseResources
{
  internal class ResourceSet<T> : IDisposable where T : IIdentity
  {
    private LocalizationContext Context { get; } = new LocalizationContext();
    private Task<IEnumerable<ILocalizedIdentity<T>>> Loading { get; set; }
    private Dictionary<int, ILocalizedIdentity<T>> Data { get; set; }

    public ILocalizedIdentity<T> this[string stringKey, CultureInfo culture]
    {
      get
      {
        string cultureKey = culture.TwoLetterISOLanguageName;
        int key = LocalizedIdentityEqualityComparer<T>.GetHashCode(stringKey, cultureKey);

        try
        {
          return Data[key];
        }
        catch (KeyNotFoundException e)
        {
          Loading = Reload(cultureKey);
          throw e;
        }
      }
    }

    private async Task<IEnumerable<ILocalizedIdentity<T>>> Reload(string culture)
    {
      await (Loading ?? Task.CompletedTask);

      Context
    }

    public void Dispose() => Context.Dispose();
  }
}