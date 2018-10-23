using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace Hymperia.Facade.Converters.AggregatorConverters
{
  /// <summary>Value transporter for aggregator converters.</summary>
  public class ValueConverterData : MarkupExtension
  {
    public Type ConvertTargetType { get; set; }
    public Type ConvertBackTargetType { get; set; }
    public object ConverterParameter { get; set; }
    public IValueConverter Converter { get; set; }

    public void Deconstruct(out IValueConverter converter, out Type target, out object parameter)
    {
      converter = Converter;
      target = ConvertTargetType;
      parameter = ConverterParameter;
    }

    public void DeconstructBack(out IValueConverter converter, out Type target, out object parameter)
    {
      converter = Converter;
      target = ConvertBackTargetType;
      parameter = ConverterParameter;
    }

    public override object ProvideValue(IServiceProvider serviceProvider) => this;
  }
}
