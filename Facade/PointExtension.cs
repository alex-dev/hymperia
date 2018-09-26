using System.Windows.Media.Media3D;
using Hymperia.Model.Modeles.JsonObject;

namespace Hymperia.Facade
{
  public static class PointExtension
  {
    public static Point Convert(this Point3D point) => new Point(point.X, point.Y, point.Z);
    public static Point3D Convert(this Point point) => new Point3D(point.X, point.Y, point.Z);
  }
}
