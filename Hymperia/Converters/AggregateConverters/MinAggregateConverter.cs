using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Hymperia.Facade.Properties;

namespace Hymperia.Facade.Converters.AggregateConverters
{
  /// <summary>Aggrège des nombres en leur valeur minimale.</summary>
  /// <remarks><see cref="ConvertBack(object, Type[], object, CultureInfo)"/> n'est pas implémenté parce que la transformation est un processus destructif.</remarks>
  public sealed class MinAggregateConverter : IMultiValueConverter
  {
    /// inheritdoc/>
    public object Convert(object[] values, Type target, object parameter = null, CultureInfo culture = default) =>
      ChangeType((from double value in values select Math.Abs(value)).Max(), target);

    /// inheritdoc/>
    public object[] ConvertBack(object value, Type[] targets, object parameter = null, CultureInfo culture = default) =>
      throw new NotImplementedException(Resources.DestructiveConversion);

    private object ChangeType(object value, Type target) => System.Convert.ChangeType(value, target);
  }
}
