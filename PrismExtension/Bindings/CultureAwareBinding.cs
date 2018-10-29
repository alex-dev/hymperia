using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Prism.Bindings
{
  /// <summary>Binding specifically ensuring <see cref="CultureInfo.CurrentCulture"/> is used instead of <see cref="XmlLanguage"/> culture.</summary>
  public class CultureAwareBinding : Binding
  {
    public CultureAwareBinding()
    {
      ConverterCulture = CultureInfo.CurrentCulture;
    }

    public CultureAwareBinding(string path)
    {
      ConverterCulture = CultureInfo.CurrentCulture;
    }
  }
}
