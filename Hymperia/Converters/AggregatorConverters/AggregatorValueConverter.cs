using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Hymperia.Facade.Converters.AggregatorConverters
{
  /// <summary>Aggregates a series of <see cref="IValueConverter"/>.</summary>
  public class AggregatorValueConverter : BaseAggregatorConverter, IValueConverter
  {
    /// <inheritdoc/>
    public override IList<ValueConverterData> Converters
    {
      get => converters;
      set
      {
        if (value.Any(c => !(c.Converter is IValueConverter)))
          throw new ArgumentException($"Only { nameof(IValueConverter) } are allowed.");

        converters = value;
      }
    }

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
