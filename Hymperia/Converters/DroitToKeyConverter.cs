using System;
using System.Globalization;
using System.Windows.Data;
using Hymperia.Facade.Extensions;
using Hymperia.Model.Modeles;

namespace Hymperia.Facade.Converters
{
  [ValueConversion(typeof(Acces.Droit), typeof(string))]
  public sealed class DroitToKeyConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
      (value as Acces.Droit? ?? Acces.Droit.Lecture).ToString();

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
      EnumExtension.Parse<Acces.Droit>(value?.ToString());
  }
}
