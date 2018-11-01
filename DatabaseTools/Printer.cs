using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hymperia.Model.Modeles;
using Hymperia.Model.Properties;
using JetBrains.Annotations;

namespace Hymperia.DatabaseTools
{
  internal static class Printer
  {
    public static void Print([ItemNotNull] IEnumerable<object> data)
    {
      foreach (object item in data)
        Console.WriteLine(item);
    }

    public static async Task PrintMateriaux([ItemNotNull] IEnumerable<Materiau> data)
    {
      foreach (var item in data)
        Console.WriteLine(Resources.MateriauToString(item.Id, (await Resources.GetMateriau(item.Nom)).Nom, item.Prix));
    }

    public static void PrintFormes()
    {
      Console.WriteLine(Resources.Cone);
      Console.WriteLine(Resources.Cylinder);
      Console.WriteLine(Resources.Ellipsoid);
      Console.WriteLine(Resources.Prism);
    }
  }
}
