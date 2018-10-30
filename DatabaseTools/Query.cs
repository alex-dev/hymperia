using Hymperia.Model;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        return await context.Materiaux.ToListAsync().ConfigureAwait(false);
      }
    }

    [NotNull]
    [ItemNotNull]
    public static async Task<IEnumerable<Projet>> QueryProjets()
    {
      using (var context = new DatabaseContext())
      {
        return await context.Projets.ToListAsync().ConfigureAwait(false);
      }
    }

    [NotNull]
    [ItemNotNull]
    public static async Task<IEnumerable<Utilisateur>> QueryUtilisateurs()
    {
      using (var context = new DatabaseContext())
      {
        return await context.Utilisateurs.IncludeAcces().ToListAsync().ConfigureAwait(false);
      }
    }
  }
}
