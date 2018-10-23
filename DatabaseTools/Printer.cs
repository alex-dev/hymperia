using System;
using System.Collections.Generic;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;

namespace Hymperia.DatabaseTools
{
  internal static class Printer
  {
    public static void Print([ItemNotNull] IEnumerable<object> data)
    {
      Console.WriteLine(Model.Properties.Resources.Cone);
      Console.WriteLine(Model.Properties.Resources.Cylinder);
      Console.WriteLine(Model.Properties.Resources.Ellipsoid);
      Console.WriteLine(Model.Properties.Resources.Prism);

      foreach (var item in data)
      {
        Console.WriteLine(item);
      }
    }

    public static void PrintLocalizedMateriau(IEnumerable<Tuple<Materiau, string>> data)
    {
      foreach (var (item, nom) in data)
      {
        Console.WriteLine($"{ item.Id} - { nom }: { item.Prix }");
      }
    }
  }
}
