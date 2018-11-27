using System;
using System.Globalization;
using System.Windows.Data;

namespace Hymperia.Facade.Converters
{
  public sealed class BooleanInverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
      !(value as bool? ?? false);

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
      !(value as bool? ?? false);
  }
}
