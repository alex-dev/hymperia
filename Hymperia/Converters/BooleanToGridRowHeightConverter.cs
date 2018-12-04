/*
  Auteur : Guillaume Le Blanc - Alexandre Parent
  Date : 2018-12-04
  Fichier : BooleanToGridRowHeightConverter.cs
*/
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Hymperia.Facade.Converters
{
  [ValueConversion(typeof(bool), typeof(GridLength))]
  public sealed class BooleanToGridRowHeightConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
      (value as bool? ?? false)
        ? new GridLength(System.Convert.ToDouble(parameter), GridUnitType.Star)
        : new GridLength(0);

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
      throw new NotImplementedException();
  }
}
