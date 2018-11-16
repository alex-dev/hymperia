using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hymperia.Facade.DependencyObjects
{
  /// <summary>Provides numeric up down commands behavior for any <see cref="TextBox"/>.</summary>
  public static class NumericTextBoxBehavior
  {
    private const string UpCommand = nameof(UpCommand);
    private const string UpCommandParameter = nameof(UpCommandParameter);
    private const string DownCommand = nameof(DownCommand);
    private const string DownCommandParameter = nameof(DownCommandParameter);

    public static readonly DependencyProperty UpCommandProperty =
      DependencyProperty.RegisterAttached(UpCommand, typeof(ICommand),  typeof(NumericTextBoxBehavior));

    public static readonly DependencyProperty UpCommandParameterProperty =
      DependencyProperty.RegisterAttached(UpCommandParameter, typeof(object), typeof(NumericTextBoxBehavior));

    public static readonly DependencyProperty DownCommandProperty =
      DependencyProperty.RegisterAttached(DownCommand, typeof(ICommand), typeof(NumericTextBoxBehavior));

    public static readonly DependencyProperty DownCommandParameterProperty =
      DependencyProperty.RegisterAttached(DownCommandParameter, typeof(object), typeof(NumericTextBoxBehavior));

    public static object GetUpCommand(TextBox element) =>
      element.GetValue(UpCommandProperty);
    public static void SetUpCommand(TextBox element, object value) =>
      element.SetValue(UpCommandProperty, value);

    public static object GetUpCommandParameter(TextBox element) =>
      element.GetValue(UpCommandParameterProperty);
    public static void SetUpCommandParameter(TextBox element, object value) =>
      element.SetValue(UpCommandParameterProperty, value);

    public static object GetDownCommand(TextBox element) =>
      element.GetValue(DownCommandProperty);
    public static void SetDownCommand(TextBox element, object value) =>
      element.SetValue(DownCommandProperty, value);

    public static object GetDownCommandParameter(TextBox element) =>
      element.GetValue(DownCommandParameterProperty);
    public static void SetDownCommandParameter(TextBox element, object value) =>
      element.SetValue(DownCommandParameterProperty, value);
  }
}
