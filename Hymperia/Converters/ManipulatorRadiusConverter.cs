using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Hymperia.Facade.Converters
{
  /// <summary>Convertit un groupe de nombre en la taille optimale d'un manipulator.</summary>
  public class ManipulatorRadiusConverter : IMultiValueConverter
  {
    /// inheritdoc/>
    /// <remarks>Vers la vue.</remarks>
    public object Convert(object[] values, Type target, object parameter = null, CultureInfo culture = default)
    {
      double result = default;
      double[] values_ = (from value in values
                          select ChangeType(value)).ToArray();

      for (int i = 0; i < values.Length; ++i)
      {
        for (int j = i; j < values.Length; ++j)
        {
          double value = values_[i] * values_[i] + values_[j] * values_[j];

          result = value > result ? value : result;
        }
      }

      return Math.Sqrt(result) / 2;
    }

    /// inheritdoc/>
    /// <remarks>Vers le modèle.</remarks>
    public object[] ConvertBack(object value, Type[] targets, object parameter = null, CultureInfo culture = default) =>
      throw new NotImplementedException("Destructive process, impossible to convert back.");

    private static double ChangeType(object value) => System.Convert.ToDouble(value);
  }
}
