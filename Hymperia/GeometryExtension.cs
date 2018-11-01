using Media = System.Windows.Media.Media3D;
using Object = Hymperia.Model.Modeles.JsonObject;

namespace Hymperia.Facade
{
  public static class GeometryExtension
  {
    /// <summary>Convertit un <see cref="Media.Point3D"/> en <see cref="Object.Point"/>.</summary>
    public static Object.Point Convert(this Media.Point3D point) => new Object.Point(point.X, point.Y, point.Z);
    /// <summary>Convertit un <see cref="Object.Point"/> en <see cref="Media.Point3D"/>.</summary>
    public static Media.Point3D Convert(this Object.Point point) => new Media.Point3D(point.X, point.Y, point.Z);

    /// <summary>Convertit un <see cref="Media.Vector3D"/> en <see cref="Object.Vector"/>.</summary>
    public static Object.Vector Convert(this Media.Vector3D vector) => new Object.Vector(vector.X, vector.Y, vector.Z);
    /// <summary>Convertit un <see cref="Object.Vector"/> en <see cref="Media.Vector3D"/>.</summary>
    public static Media.Vector3D Convert(this Object.Vector vector) => new Media.Vector3D(vector.X, vector.Y, vector.Z);

    /// <summary>Convertit un <see cref="Media.Quaternion"/> en <see cref="Object.Quaternion"/>.</summary>
    public static Object.Quaternion Convert(this Media.Quaternion quaternion) => new Object.Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
    /// <summary>Convertit un <see cref="Object.Quaternion"/> en <see cref="Media.Quaternion"/>.</summary>
    public static Media.Quaternion Convert(this Object.Quaternion quaternion)
    {
      var value = new Media.Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

      if (!value.IsNormalized)
        value.Normalize();

      return value;
    }

    /// <summary>Convertit un <see cref="Object.Point"/> en <see cref="Media.Vector3D"/>.</summary>
    public static Media.Vector3D ConvertToVector(this Object.Point point) => new Media.Vector3D(point.X, point.Y, point.Z);
    /// <summary>Convertit un <see cref="Media.Vector3D"/> en <see cref="Object.Point"/>.</summary>
    public static Object.Point ConvertToPoint(this Media.Vector3D vector) => new Object.Point(vector.X, vector.Y, vector.Z);
  }
}
