using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace HelixToolkit.Wpf
{
  public static class MeshBuilderExtension
  {
    public static void AddCylinder(this MeshBuilder builder, Point3D origine, double height, double innerDiameter, double diameter, int thetaDiv)
    {
      var direction = new Vector3D(0, 0, 1);
      var textures = new List<double> { 1, 0, 1, 0 };
      var points = new PointCollection
      {
        new Point(-height / 2, innerDiameter / 2),
        new Point(-height / 2, diameter / 2),
        new Point(height / 2, diameter / 2),
        new Point(height / 2, innerDiameter / 2)
      };

      if (innerDiameter > 0)
      {
        points.Add(new Point(-height / 2, innerDiameter / 2));
        textures.Add(1);
      }

      builder.AddRevolvedGeometry(points, textures, origine, direction, thetaDiv);
    }
  }
}
