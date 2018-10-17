using System;

namespace Hymperia.Model
{
  internal static class DoubleExtension
  {
    /// <summary>Convertit un angle en radian</summary>
    public static double ConvertToRadian(this double angle) => angle * Math.PI / 180;
  }
}
