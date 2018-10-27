using System;
using JetBrains.Annotations;

namespace Hymperia.Model.Modeles.JsonObject
{
  public class Point : IEquatable<Point>
  {
    public static Point Center => new Point(0, 0, 0);

    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    public Point(double x, double y, double z)
    {
      X = x;
      Y = y;
      Z = z;
    }

    #region IEquatable<Point>

    [Pure]
    public override bool Equals(object obj) => Equals(obj as Point);
    [Pure]
    public bool Equals(Point other) => other != null && X == other.X && Y == other.Y && Z == other.Z;
    [Pure]
    public override int GetHashCode()
    {
      var hashCode = -307843816;
      hashCode = hashCode * -1521134295 + X.GetHashCode();
      hashCode = hashCode * -1521134295 + Y.GetHashCode();
      hashCode = hashCode * -1521134295 + Z.GetHashCode();
      return hashCode;
    }

    #endregion
  }
}
