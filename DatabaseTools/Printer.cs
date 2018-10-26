using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace Hymperia.DatabaseTools
{
  internal static class Printer
  {
    public static void Print([ItemNotNull] IEnumerable<object> data)
    {
      foreach (var item in data)
      {
        Console.WriteLine(item);
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
