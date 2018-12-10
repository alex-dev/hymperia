using System;
using System.Globalization;
using System.Windows.Data;
using Hymperia.Facade.Extensions;
using M = System.Windows.Media.Media3D;
using O = Hymperia.Model.Modeles.JsonObject;

namespace Hymperia.Facade.Converters
{
  [ValueConversion(typeof(M.Point3D), typeof(O.Point))]
  public sealed class Point3DToPointConverter : IValueConverter
  {
    /// inheritdoc/>
    /// <remarks>Vers la vue.</remarks>
    public object Convert(object value, Type target, object parameter = null, CultureInfo culture = default) =>
      ((M.Point3D?)value)?.Convert();

    /// inheritdoc/>
    /// <remarks>Vers le modèle.</remarks>
    public object ConvertBack(object value, Type target, object parameter = null, CultureInfo culture = default) =>
      ((O.Point)value)?.Convert();
  }
}
