using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace Hymperia.Facade.Converters.AggregatorConverters
{
  /// <summary>Value transporter for aggregator converters.</summary>

  public class MultiValueConverterData : MarkupExtension
  {
    public Type ConvertTargetType { get; set; }
    public Type[] ConvertBackTargetTypes { get; set; }
    public object ConverterParameter { get; set; }
    public IMultiValueConverter Converter { get; set; }

    public void Deconstruct(out IMultiValueConverter converter, out Type target, out object parameter)
    {
      converter = Converter;
      target = ConvertTargetType;
      parameter = ConverterParameter;
    }

    public void DeconstructBack(out IMultiValueConverter converter, out Type[] target, out object parameter)
    {
      converter = Converter;
      target = ConvertBackTargetTypes;
      parameter = ConverterParameter;
    }

    public override object ProvideValue(IServiceProvider serviceProvider) => this;
  }
}
