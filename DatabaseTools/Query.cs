using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hymperia.Model;
using Hymperia.Model.Localization;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Hymperia.DatabaseTools
{
  internal static class Query
  {
    [NotNull]
    [ItemNotNull]
    public static async Task<IEnumerable<Materiau>> QueryMateriaux()
    {
      using (var context = new DatabaseContext())
      {
        return await context.Materiaux.ToListAsync();
      }
    }

    [NotNull]
    [ItemNotNull]
    public static async Task<IEnumerable<Projet>> QueryProjets()
    {
      using (var context = new DatabaseContext())
      {
        return await context.Projets.ToListAsync();
      }
    }

    [NotNull]
    [ItemNotNull]
    public static async Task<IEnumerable<Utilisateur>> QueryUtilisateurs()
    {
      using (var context = new DatabaseContext())
      {
        return await context.Utilisateurs.IncludeAcces().ToListAsync();
      }
    }

    [NotNull]
    [ItemNotNull]
    public static async Task<IEnumerable<LocalizedMateriau>> QueryLocalizedMateriaux(string key)
    {
      using (var context = new LocalizationContext())
      {
        return await context.Materiaux.Where(materiau => materiau.CultureKey == key).ToListAsync();
      }
    }
  }
}
