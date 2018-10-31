using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Hymperia.Model.Identity;
using Hymperia.Model.Localization;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Hymperia.Model.DatabaseResources
{
  internal class ResourceManager
  {
    [NotNull]
    [ItemNotNull]
    public async Task<IDictionary<string, LocalizedMateriau>> LoadMateriaux()
    {
      using (await AsyncLock.Lock(MateriauxLocker))
      {
        return Materiaux =
          await Load<LocalizedMateriau, Materiau>(CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
      }
    }

    [CanBeNull]
    public async Task<LocalizedMateriau> GetMateriau([NotNull] string key)
    {
      string lang = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

      if (Materiaux is null || Materiaux.First().Value.CultureKey != lang)
        await LoadMateriaux().ConfigureAwait(false);

      return Materiaux[key];
    }

    [CanBeNull]
    public async Task<LocalizedMateriau> GetMateriau([NotNull] string key, [NotNull] CultureInfo culture) =>
      await Load<LocalizedMateriau, Materiau>(key, culture.TwoLetterISOLanguageName).ConfigureAwait(false);

    [CanBeNull]
    private async Task<TLocalized> Load<TLocalized, TEntity>([NotNull] string key, [NotNull] string culture)
      where TLocalized : class, ILocalizedIdentity<TEntity>
      where TEntity : IIdentity
    {
      using (var context = new LocalizationContext())
      {
        return await context.Set<TLocalized>()
          .SingleOrDefaultAsync(localized => localized.StringKey == key && localized.CultureKey == culture)
          .ConfigureAwait(false);
      }
    }

    [NotNull]
    [ItemNotNull]
    private async Task<Dictionary<string, TLocalized>> Load<TLocalized, TEntity>([NotNull] string culture)
      where TLocalized : class, ILocalizedIdentity<TEntity>
      where TEntity : IIdentity
    {
      using (var context = new LocalizationContext())
      {
        return await context.Set<TLocalized>().Where(localized => localized.CultureKey == culture)
          .ToDictionaryAsync(localized => localized.StringKey).ConfigureAwait(false);
      }
    }

    
    [CanBeNull]
    [ItemNotNull]
    private Dictionary<string, LocalizedMateriau> Materiaux { get; set; }
    [NotNull]
    private readonly object MateriauxLocker = new object();
  }
}
