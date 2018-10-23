using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace Hymperia.Facade.Converters.AggregatorConverters
{
  /// <summary>Aggregates a series of <see cref="IValueConverter"/>.</summary>
  [DefaultProperty(nameof(Converters))]
  [ContentProperty(nameof(Converters))]
  public class AggregatorValueConverter : BaseAggregatorConverter, IValueConverter
  {
    /// <summary>Child <see cref="IValueConverter"/>.</summary>
    public LinkedList<ValueConverterData> Converters { get; set; } = new LinkedList<ValueConverterData>();

    public object Convert(object value, Type target, object parameter = null, CultureInfo culture = default) =>
      Convert(value, culture);

    public object Convert(object value, CultureInfo culture = default) =>
      Converters?.Aggregate(value, (data, accumulator) => Apply(accumulator, data, culture));

    public object ConvertBack(object value, Type target, object parameter = null, CultureInfo culture = default) =>
      ConvertBack(value, culture);

    public object ConvertBack(object value, CultureInfo culture = default) =>
      Converters?.Reverse()?.Aggregate(value, (data, accumulator) => ApplyBack(accumulator, data, culture));
  }
}
