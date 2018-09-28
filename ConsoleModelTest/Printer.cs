using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Hymperia.ConsoleModelTest
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
  }
}
