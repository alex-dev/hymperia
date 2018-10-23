using System;
using System.Globalization;
using System.Windows.Markup;

namespace Hymperia.Facade.Converters.AggregatorConverters
{
  public abstract class BaseAggregatorConverter : MarkupExtension
  {
    #region Apply Converter

    protected object Apply(ValueConverterData data, object value, CultureInfo culture)
    {
      data.Deconstruct(out var converter, out var target, out object parameter);
      return converter.Convert(value, target, parameter, culture);
    }

    protected object ApplyBack(ValueConverterData data, object value, CultureInfo culture)
    {
      data.DeconstructBack(out var converter, out var target, out object parameter);
      return converter.ConvertBack(value, target, parameter, culture);
    }

    #endregion

    #region Apply MultiConverter

    protected object ApplyMulti(MultiValueConverterData data, object[] value, CultureInfo culture)
    {
      data.Deconstruct(out var converter, out var target, out object parameter);
      return converter.Convert(value, target, parameter, culture);
    }

    protected object[] ApplyBackMulti(MultiValueConverterData data, object value, CultureInfo culture)
    {
      data.DeconstructBack(out var converter, out var target, out object parameter);
      return converter.ConvertBack(value, target, parameter, culture);
    }

    #endregion

    /// <summary>When implemented in a derived class, returns an object that is provided as the value of the target property for this markup extension. </summary>
    /// <returns>The object value to set on the property where the extension is applied. </returns>
    /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
    public override object ProvideValue(IServiceProvider serviceProvider) => this;
  }
}
