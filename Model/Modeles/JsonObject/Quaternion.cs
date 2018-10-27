using System;
using JetBrains.Annotations;

namespace Hymperia.Model.Modeles.JsonObject
{
  public class Quaternion : IEquatable<Quaternion>
  {
    public static Quaternion Identity => new Quaternion(0, 0, 0, 1);

    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    public double W { get; set; }

    public Quaternion(double x, double y, double z, double w)
    {
      X = x;
      Y = y;
      Z = z;
      W = w;
    }

    #region IEquatable<Quaternion>

    [Pure]
    public override bool Equals(object obj) => Equals(obj as Quaternion);
    [Pure]
    public bool Equals(Quaternion other) => other != null && X == other.X && Y == other.Y && Z == other.Z && W == other.W;
    [Pure]
    public override int GetHashCode()
    {
      var hashCode = 707706286;
      hashCode = hashCode * -1521134295 + X.GetHashCode();
      hashCode = hashCode * -1521134295 + Y.GetHashCode();
      hashCode = hashCode * -1521134295 + Z.GetHashCode();
      hashCode = hashCode * -1521134295 + W.GetHashCode();
      return hashCode;
    }

    #endregion
  }
}
