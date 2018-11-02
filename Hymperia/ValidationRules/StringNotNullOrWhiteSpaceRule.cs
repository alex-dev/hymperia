using System.Globalization;
using System.Windows.Controls;
using Hymperia.Facade.Properties;

namespace Hymperia.Facade.ValidationRules
{
  public class StringNotNullOrWhiteSpaceRule : ValidationRule
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo) =>
       string.IsNullOrWhiteSpace(value as string)
        ? new ValidationResult(false, Resources.FieldCannotBeEmpty)
        : ValidationResult.ValidResult;
  }
}
