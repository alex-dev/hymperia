using System;
using System.Collections.Generic;
using System.Linq;
using Hymperia.Model;

namespace ConsoleModelTest
{
  static class Program
  {
    /// <summary>Conteneur associatif entre les arguments possibles et des fonctions exécutant la tâche.</summary>
    /// <remarks>
    ///   Cette approche a été choisie pour éviter de jouer avec les types et plus de code complexe.
    ///   <list type="bullet">
    ///     <item>La clé n'est que l'argument passé par la console. Choisir quelque chose de simple et court.</item>
    ///     <item>
    ///       L'<see cref="Action"/> devrait appelé <see cref="Print{T}"/> avec un callback prenant en paramètre
    ///       le contexte de la DB et retournant le DBSet correspondant à la clé.
    ///     </item>
    ///   </list>
    /// </remarks>
    /// 
    private static IDictionary<string, Action> Types
    {
      get => new Dictionary<string, Action>
      {
        { "users", () => Print(context => context.Users ) }
      };
    }

    /// <summary>Affiche l'ensemble de la collection demandée</summary>
    /// <param name="args">
    ///   <list type="number">
    ///     <item>Le type de la collection à afficher.</item>
    ///   </list>
    /// </param>
    public static void Main(string[] args)
    {
      Types[args[0].ToLowerInvariant()]();
    }

    private static void Print<T>(Func<DatabaseContext, IEnumerable<T>> callable) where T : class
    {
      foreach (var item in Query(callable))
      {
        Console.WriteLine(item);
      }
    }

    private static IEnumerable<T> Query<T>(Func<DatabaseContext, IEnumerable<T>> callable) where T : class
    {
      using (var context = new DatabaseContext())
      {
        return callable(context).ToList();
      }
    }
  }
}
