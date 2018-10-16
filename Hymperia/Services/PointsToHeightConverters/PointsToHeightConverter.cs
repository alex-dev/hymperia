using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Media3D;

namespace Hymperia.Facade.Services.PointsToHeightConverters
{
  public class PointsToHeightConverter : IValueConverter
  {
    /// <inheritdoc />
    /// <remarks>Vers la vue.</remarks>
    public object Convert(object value, Type target, object parameter, CultureInfo culture = default) =>
      PointsToHeightStaticConverter.ConvertToHeight((Point3D)value, target);

    /// <inheritdoc />
    /// <remarks>Vers le modèle.</remarks>
    public object ConvertBack(object value, Type target, object parameter, CultureInfo culture = default) =>
      PointsToHeightStaticConverter.ConvertToPoint((double)value, (PointOrientation)parameter, target);
  }
}