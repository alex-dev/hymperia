using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Hymperia.Model;
using Hymperia.Model.Modeles;

namespace Hymperia.ConsoleModelTest
{
  internal static class Query
  {
    public const string ConfigurationName = DatabaseContext.ConfigurationName;

    [ItemNotNull]
    public static async Task<IEnumerable<Materiau>> QueryMateriaux()
    {
      using (var context = new DatabaseContext())
      {
        return await context.Materiaux.ToListAsync();
      }
    }

    [ItemNotNull]
    public static async Task<IEnumerable<Projet>> QueryProjets()
    {
      using (var context = new DatabaseContext())
      {
        return await context.Projets.ToListAsync();
      }
    }

    [ItemNotNull]
    public static async Task<IEnumerable<Utilisateur>> QueryUtilisateurs()
    {
      using (var context = new DatabaseContext())
      {
        return await context.Utilisateurs.IncludeAcces().ToListAsync();
      }
    }
  }
}
