using Hymperia.Model.Modeles;
using Hymperia.Model.Properties;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hymperia.DatabaseTools
{
  internal static class Printer
  {
    public static void Print([ItemNotNull] IEnumerable<object> data)
    {
      foreach (object item in data)
      {
        Console.WriteLine(item);
      }
    }

    public static async Task PrintMateriaux([ItemNotNull] IEnumerable<Materiau> data)
    {
      foreach (var item in data)
      {
        Console.WriteLine(Resources.MateriauToString(item.Id, (await Resources.GetMateriau(item.Nom)).Nom, item.Prix));
      }
    }

    public static void PrintFormes()
    {
      Console.WriteLine(Model.Properties.Resources.Cone);
      Console.WriteLine(Model.Properties.Resources.Cylinder);
      Console.WriteLine(Model.Properties.Resources.Ellipsoid);
      Console.WriteLine(Model.Properties.Resources.Prism);
    }
  }
}
