using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Media3D;
using Hymperia.Model.Modeles.JsonObject;

namespace Hymperia.Facade.Services
{
  public class PointValueConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (value as Point)?.Convert();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (value as Point3D?)?.Convert();
    }
  }
}
