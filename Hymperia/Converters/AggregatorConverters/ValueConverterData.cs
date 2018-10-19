using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace Hymperia.Facade.Converters.AggregatorConverters
{
  /// <summary>
  ///   Value transporter for aggregator converters.
  ///   Considering the complexity of typing, no validition are done on converters parameters and
  ///   target types. <see cref="InvalidCastException"/> will be thrown when used and casted into
  ///   expected values.
  /// </summary>
  public class ValueConverterData : MarkupExtension
  {
    public object ConvertTargetType { get; set; }
    public object ConvertBackTargetType { get; set; }
    public object ConverterParameter { get; set; }
    public object Converter
    {
      get => converter;
      set
      {
        if (!(value is IValueConverter) || !(value is IMultiValueConverter))
          throw new ArgumentException($"Only { nameof(IValueConverter) } and { nameof(IMultiValueConverter) } are allowed.");

        converter = value;
      }
    }

    public void Deconstruct(out IValueConverter converter, out Type target, out object parameter)
    {
      converter = (IValueConverter)Converter;
      target = (Type)ConvertTargetType;
      parameter = ConverterParameter;
    }

    public void Deconstruct(out IMultiValueConverter converter, out Type target, out object parameter)
    {
      converter = (IMultiValueConverter)Converter;
      target = (Type)ConvertTargetType;
      parameter = ConverterParameter;
    }

    public void DeconstructBack(out IValueConverter converter, out Type target, out object parameter)
    {
      converter = (IValueConverter)Converter;
      target = (Type)ConvertBackTargetType;
      parameter = ConverterParameter;
    }

    public void DeconstructBack(out IMultiValueConverter converter, out Type[] target, out object parameter)
    {
      converter = (IMultiValueConverter)Converter;
      target = (Type[])ConvertBackTargetType;
      parameter = ConverterParameter;
    }

    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    private object converter { get; set; }
  }
}
