using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Media3D;

namespace Hymperia.Facade.Services.PointsToHeightConverters
{
  [ValueConversion(typeof(double), typeof(Point3D))]
  public class HeightToPointsConverter : IValueConverter
  {
    /// <inheritdoc />
    /// <remarks>Vers la vue.</remarks>
    public object Convert(object value, Type target, object parameter, CultureInfo culture = default) =>
      PointsToHeightStaticConverter.ConvertToPoint((double)value, (PointOrientation)parameter, target);

    /// <inheritdoc />
    /// <remarks>Vers le modèle.</remarks>
    public object ConvertBack(object value, Type target, object parameter, CultureInfo culture = default) =>
      PointsToHeightStaticConverter.ConvertToHeight((Point3D)value, target);
  }
}