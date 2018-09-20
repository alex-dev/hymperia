using System;

namespace Hymperia.Model
{
  internal static class DoubleExtension
  {
    public static double ConvertToRadian(this double angle) => angle * Math.PI / 180;
  }
}
