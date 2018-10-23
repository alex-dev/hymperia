using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace Hymperia.Facade.Converters.AggregatorConverters
{
  /// <summary>Aggregates a series of <see cref="IValueConverter"/> and one <see cref="IMultiValueConverter"/>.</summary>
  [DefaultProperty(nameof(Converters))]
  [ContentProperty(nameof(Converters))]
  public class AggregatorMultiValueConverter : BaseAggregatorConverter, IMultiValueConverter
  {
    /// <summary>Child <see cref="IMultiValueConverter"/>.</summary>
    public MultiValueConverterData Converter { get; set; }

    /// <summary>Child <see cref="IValueConverter"/>.</summary>
    public LinkedList<ValueConverterData> Converters { get; set; } = new LinkedList<ValueConverterData>();

    public object Convert(object[] value, Type targetType, object parameter = null, CultureInfo culture = default) =>
          Convert(value, culture);

    public object Convert(object[] value, CultureInfo culture = default) =>
      Converters?.Aggregate(
        ApplyMulti(Converter, value, culture),
        (data, accumulator) => Apply(accumulator, data, culture));

    public object[] ConvertBack(object value, Type[] target, object parameter = null, CultureInfo culture = default) =>
      ConvertBack(value, culture);

    public object[] ConvertBack(object value, CultureInfo culture = default) =>
      ApplyBackMulti(
        Converter,
        Converters?.Reverse()?.Aggregate(value, (data, accumulator) => ApplyBack(accumulator, data, culture)),
        culture);
  }
}
