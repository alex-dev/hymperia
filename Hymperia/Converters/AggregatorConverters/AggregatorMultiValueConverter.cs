using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Hymperia.Facade.Converters.AggregatorConverters
{
  /// <summary>Aggregates a series of <see cref="IValueConverter"/> and one <see cref="IMultiValueConverter"/>.</summary>
  public class AggregatorMultiValueConverter : BaseAggregatorConverter, IMultiValueConverter
  {
    /// <inheritdoc/>
    public override IList<ValueConverterData> Converters
    {
      get => converters;
      set
      {
        if (!Validate(value))
          throw new ArgumentException($"Only { nameof(IValueConverter) } and { nameof(IMultiValueConverter) } are allowed.");

        converters = value;
      }
    }

    public object Convert(object[] value, Type targetType, object parameter = null, CultureInfo culture = default) =>
          Convert(value, culture);

    public object Convert(object[] value, CultureInfo culture = default) =>
      Converters?.Skip(1)?.Aggregate(
        ApplyMulti(Converters?.First(), value, culture),
        (data, accumulator) => Apply(accumulator, data, culture));

    public object[] ConvertBack(object value, Type[] target, object parameter = null, CultureInfo culture = default) =>
      ConvertBack(value, culture);

    public object[] ConvertBack(object value, CultureInfo culture = default) =>
      ApplyBackMulti(
        Converters?.First(),
        Converters?.Skip(1)?.Reverse()?.Aggregate(
          value,
          (data, accumulator) => ApplyBack(accumulator, data, culture)),
        culture);

    #region Apply MultiConverter

    protected object ApplyMulti(ValueConverterData data, object[] value, CultureInfo culture)
    {
      data.Deconstruct(out IMultiValueConverter converter, out var target, out object parameter);
      return converter.Convert(value, target, parameter, culture);
    }

    protected object[] ApplyBackMulti(ValueConverterData data, object value, CultureInfo culture)
    {
      data.DeconstructBack(out IMultiValueConverter converter, out var target, out object parameter);
      return converter.ConvertBack(value, target, parameter, culture);
    }

    #endregion

    #region Validation

    private bool Validate(IEnumerable<ValueConverterData> converters) =>
      (from data in converters
       select data.Converter)
         .AllFirstsAndAllLast(c => c is IMultiValueConverter, 1, c => c is IValueConverter);

    #endregion
  }
}
