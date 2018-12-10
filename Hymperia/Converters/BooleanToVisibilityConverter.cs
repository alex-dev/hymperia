/**
 * Auteur : Antoine Mailhot
 * Date de création : 8 novembre 2018
 */

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Hymperia.Facade.Converters
{
  [ValueConversion(typeof(bool), typeof(Visibility))]
  public sealed class BooleanToVisibilityConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
      value as bool? == true ? Visibility.Visible : Visibility.Collapsed;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
      value as Visibility? == Visibility.Visible;
  }
}
