﻿using System;
using System.Windows.Media.Media3D;

namespace Hymperia.Facade.Converters.PointsToHeightConverters
{
  public enum PointOrientation { Top, Bottom }

  /// <summary>Implémente la logique derrière <see cref="PointsToHeightConverter"/> et <see cref="HeightToPointsConverter"/>.</summary>
  internal static class PointsToHeightStaticConverter
  {
    public static object ConvertToPoint(double height, PointOrientation orientation, Type target)
    {
      if (typeof(Point3D) != target)
        throw new ArgumentException($"Can only convert to { nameof(Point3D) }.", nameof(target));

      switch (orientation)
      {
        case PointOrientation.Top:
          return new Point3D(0, 0, height / 2);
        case PointOrientation.Bottom:
          return new Point3D(0, 0, -height / 2);
        default:
          throw new NotImplementedException("How did you get there???");
      }
    }

    public static object ConvertToHeight(Point3D point, Type target) => ChangeType(point.Z * 2, target);

    private static object ChangeType(object value, Type target) => Convert.ChangeType(value, target);
  }
}