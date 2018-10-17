using System;
using System.Globalization;
using System.Windows.Data;
using M = System.Windows.Media.Media3D;
using O = Hymperia.Model.Modeles.JsonObject;

namespace Hymperia.Facade.Services
{
  /// <summary>Convertit des <see cref="M.Point3D"/> en <see cref="O.Point"/>.</summary>
  public class Point3DToPointConverter : IValueConverter
  {
    /// <inheritdoc />
    /// <remarks>Vers la vue.</remarks>
    public object Convert(object value, Type target, object parameter = null, CultureInfo culture = default) =>
      ((M.Point3D?)value)?.Convert();

    /// <inheritdoc />
    /// <remarks>Vers le modèle.</remarks>
    public object ConvertBack(object value, Type target, object parameter = null, CultureInfo culture = default) =>
      ((O.Point)value)?.Convert();
  }
}
