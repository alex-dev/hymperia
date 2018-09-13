using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Hymperia.Model;

namespace Hymperia.ConsoleModelTest
{
  static class Program
  {
    /// <summary>Conteneur associatif entre les arguments possibles et des fonctions exécutant la tâche.</summary>
    /// <remarks>
    ///   Cette approche a été choisie pour éviter de jouer avec les types et plus de code complexe.
    ///   <list type="bullet">
    ///     <item>La clé n'est que l'argument passé par la console. Choisir quelque chose de simple et court.</item>
    ///     <item>
    ///       L'<see cref="Func{Task}<"/> devrait appelé <see cref="Print{T}"/> avec un callback prenant en paramètre
    ///       le contexte de la DB et retournant le DBSet correspondant à la clé.
    ///     </item>
    ///   </list>
    /// </remarks>
    [ItemNotNull]
    private static IDictionary<string, Func<Task>> Types
    {
      get => new Dictionary<string, Func<Task>>
      {
        { "utilisateurs", async () => await Print(context => context.Utilisateurs) },
        { "acces", async () => await Print(context => context.Acces) },
        { "materiaux", async () => await Print(context => context.Materiaux) }
      };
    }

    /// <summary>Affiche l'ensemble de la collection demandée</summary>
    /// <param name="args">
    ///   <list type="number">
    ///     <item>Le type de la collection à afficher.</item>
    ///   </list>
    /// </param>
    public static async Task Main(string[] args)
    {
      await Migrate();
      await Types[args[0].ToLowerInvariant()]();
      Console.ReadKey(true);
    }

    private static async Task Migrate()
    {
      using (var context = new DatabaseContext())
      {
        await context.Migrate();
      }
    }

    private static async Task Print<T>([NotNull] Func<DatabaseContext, DbSet<T>> callable) where T : class
    {
      foreach (var item in await Query(callable))
      {
        Console.WriteLine(item);
      }
    }

    private static async Task<IEnumerable<T>> Query<T>([NotNull] Func<DatabaseContext, DbSet<T>> callable) where T : class
    {
      using (var context = new DatabaseContext())
      {
        return await callable(context).ToListAsync();
      }
    }
  }
}
