using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Hymperia.Facade.Converters.AggregatorConverters
{
  [DefaultProperty(nameof(Converters))]
  [ContentProperty(nameof(Converters))]
  public abstract class BaseAggregatorConverter : MarkupExtension
  {
    /// <summary>Chlid converters</summary>
    public abstract IList<ValueConverterData> Converters { get; set; }

    #region Apply Converter

    protected object Apply(ValueConverterData data, object value, CultureInfo culture)
    {
      data.Deconstruct(out IValueConverter converter, out var target, out object parameter);
      return converter.Convert(value, target, parameter, culture);
    }

    protected object ApplyBack(ValueConverterData data, object value, CultureInfo culture)
    {
      data.DeconstructBack(out IValueConverter converter, out var target, out object parameter);
      return converter.ConvertBack(value, target, parameter, culture);
    }

    #endregion

    /// <summary>When implemented in a derived class, returns an object that is provided as the value of the target property for this markup extension. </summary>
    /// <returns>The object value to set on the property where the extension is applied. </returns>
    /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    #region Private Fields

    protected IList<ValueConverterData> converters;

    #endregion
  }
}
