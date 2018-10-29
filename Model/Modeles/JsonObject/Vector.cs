using System;
using System.Diagnostics.Contracts;

namespace Hymperia.Model.Modeles.JsonObject
{
  public class Vector : IEquatable<Vector>
  {
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    public Vector(double x, double y, double z)
    {
      X = x;
      Y = y;
      Z = z;
    }

    #region IEquatable<Vector>

    [Pure]
    public override bool Equals(object obj) => Equals(obj as Vector);
    [Pure]
    public bool Equals(Vector other) => other != null && X == other.X && Y == other.Y && Z == other.Z;
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
