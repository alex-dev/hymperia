using System.Windows.Media.Media3D;

namespace Hymperia.Facade
{
  public static class MatrixExtension
  {
    /// <summary>
    ///   Extrait la translation d'une matrice composées de translation et rotation.
    ///   Attention: Scale et skew ne sont pas supportés.
    /// </summary>
    public static Vector3D ExtractTranslate(this Matrix3D matrix)
    {
      return matrix.M44 == 1
        ? new Vector3D(matrix.OffsetX, matrix.OffsetY, matrix.OffsetZ)
        : new Vector3D(matrix.OffsetX, matrix.OffsetY, matrix.OffsetZ) / matrix.M44;
    }

    /// <summary>
    ///   Extrait la rotation d'une matrice composées de translation et rotation.
    ///   Attention: Scale et skew ne sont pas supportés.
    /// </summary>
    public static Quaternion ExtractRotation(this Matrix3D matrix)
    {
      /*

      x2 = quaternion.X + quaternion.X;
      y2 = quaternion.Y + quaternion.Y;
      z2 = quaternion.Z + quaternion.Z;
      xx = quaternion.X * x2;
      xy = quaternion.X * y2;
      xz = quaternion.X * z2;
      yy = quaternion.Y * y2;
      yz = quaternion.Y * z2;
      zz = quaternion.Z * z2;
      wx = quaternion.W * x2;
      wy = quaternion.W * y2;
      wz = quaternion.W * z2;



      1 - matrix._m11 = yy + zz;
      matrix._m12 = xy + wz;
      matrix._m13 = xz - wy;
      matrix._m21 = xy - wz;
      1 - atrix._m22 = xx + zz;
      matrix._m23 = yz + wx;
      matrix._m31 = xz + wy;
      matrix._m32 = yz - wx;
      1 - matrix._m33 = xx + yy;
      */
      return new Quaternion();
    }
  }
}
