using System.Windows.Media.Media3D;
using Hymperia.Model.Modeles.JsonObject;

namespace Hymperia.Facade
{
  public static class GeometryExtension
  {
    public static Point Convert(this Point3D point) => new Point(point.X, point.Y, point.Z);
    public static Point3D Convert(this Point point) => new Point3D(point.X, point.Y, point.Z);

    public static Vector Convert(this Vector3D vector) => new Vector(vector.X, vector.Y, vector.Z);
    public static Vector3D Convert(this Vector vector) => new Vector3D(vector.X, vector.Y, vector.Z);

    public static Model.Modeles.JsonObject.Quaternion Convert(this System.Windows.Media.Media3D.Quaternion quaternion)
    {
      return new Model.Modeles.JsonObject.Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
    }
    public static System.Windows.Media.Media3D.Quaternion Convert(this Model.Modeles.JsonObject.Quaternion quaternion)
    {
      return new System.Windows.Media.Media3D.Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
    }
  }
}
