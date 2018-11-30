using System;
using System.Globalization;
using System.Windows.Data;
using Hymperia.Model.Modeles;

namespace Hymperia.Facade.Converters
{
  public sealed class DroitConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
      (value as Acces.Droit? ?? Acces.Droit.Lecture) >= Acces.Droit.LectureEcriture;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
      (value as bool? ?? false) ? Acces.Droit.LectureEcriture : Acces.Droit.Lecture;
  }
}
