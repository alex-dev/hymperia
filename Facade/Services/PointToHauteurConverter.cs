using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Media3D;

namespace Hymperia.Facade.Services
{
  public enum PointOrientation { Top, Bottom }

  public class PointToHauteurConverter : IValueConverter
  {
    /// <inheritdoc />
    /// <remarks>Vers la vue.</remarks>
    public object Convert(object value, Type target, object parameter, CultureInfo culture = default)
    {
      if (typeof(Point3D) != target)
      {
        throw new ArgumentException($"Can only convert to { nameof(Point3D) }.", nameof(target));
      }

      if (!(value is double hauteur))
      {
        throw new InvalidCastException($"Can not cast { nameof(value) } to { nameof(Double) }");
      }

      if (!(parameter is PointOrientation orientation))
      {
        throw new InvalidCastException($"Can not cast { nameof(parameter) } to { nameof(PointOrientation) }");
      }

      switch (orientation)
      {
        case PointOrientation.Top:
          return new Point3D(0, 0, hauteur / 2);
        case PointOrientation.Bottom:
          return new Point3D(0, 0, -hauteur / 2);
      }

      throw new Exception("How did you get there???");
    }

    /// <inheritdoc />
    /// <remarks>Vers le modèle.</remarks>
    public object ConvertBack(object value, Type target, object parameter, CultureInfo culture = default)
    {
      if (typeof(double) != target)
      {
        throw new ArgumentException($"Can only convert to { nameof(Double) }.", nameof(target));
      }

      if (!(value is Point3D point))
      {
        throw new InvalidCastException($"Can not cast { nameof(value) } to { nameof(Point3D) }");
      }

      return point.Z * 2;
    }
  }
}