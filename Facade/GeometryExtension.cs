using Media = System.Windows.Media.Media3D;
using Object = Hymperia.Model.Modeles.JsonObject;

namespace Hymperia.Facade
{
  public static class GeometryExtension
  {
    public static Object.Point Convert(this Media.Point3D point) => new Object.Point(point.X, point.Y, point.Z);
    public static Media.Point3D Convert(this Object.Point point) => new Media.Point3D(point.X, point.Y, point.Z);

    public static Object.Vector Convert(this Media.Vector3D vector) => new Object.Vector(vector.X, vector.Y, vector.Z);
    public static Media.Vector3D Convert(this Object.Vector vector) => new Media.Vector3D(vector.X, vector.Y, vector.Z);

    public static Object.Quaternion Convert(this Media.Quaternion quaternion) => new Object.Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
    public static Media.Quaternion Convert(this Object.Quaternion quaternion) => new Media.Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

    public static Media.Vector3D ConvertToVector(this Object.Point point) => new Media.Vector3D(point.X, point.Y, point.Z);
    public static Object.Point ConvertToPoint(this Media.Vector3D vector) => new Object.Point(vector.X, vector.Y, vector.Z);
  }
}
