using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Prism.Commands;

namespace Hymperia.Facade.DependencyObjects
{
  /// <summary>Provides numeric up down commands behavior for any <see cref="TextBox"/>.</summary>
  public static class NumericTextBoxBehavior
  {
    private const string UpCommand = nameof(UpCommand);
    private const string UpCommandParameter = nameof(UpCommandParameter);
    private const string DownCommand = nameof(DownCommand);
    private const string DownCommandParameter = nameof(DownCommandParameter);

    private static readonly ICommand Up = new DelegateCommand<TextBox>(ExecuteUp);
    private static readonly ICommand Down = new DelegateCommand<TextBox>(ExecuteDown);

    public static readonly DependencyProperty UpCommandProperty =
      DependencyProperty.RegisterAttachedReadOnly(UpCommand, typeof(ICommand),  typeof(NumericTextBoxBehavior), new PropertyMetadata(Up))
      .DependencyProperty;

    public static readonly DependencyProperty DownCommandProperty =
      DependencyProperty.RegisterAttachedReadOnly(DownCommand, typeof(ICommand), typeof(NumericTextBoxBehavior), new PropertyMetadata(Down))
      .DependencyProperty;

    public static readonly DependencyProperty UpCommandParameterProperty =
      DependencyProperty.RegisterAttached(UpCommandParameter, typeof(double), typeof(NumericTextBoxBehavior), new PropertyMetadata(1.0));

    public static readonly DependencyProperty DownCommandParameterProperty =
      DependencyProperty.RegisterAttached(DownCommandParameter, typeof(double), typeof(NumericTextBoxBehavior), new PropertyMetadata(1.0));

    public static ICommand GetUpCommand(TextBox element) =>
      element.GetValue(UpCommandProperty) as ICommand;
    public static ICommand GetDownCommand(TextBox element) =>
      element.GetValue(DownCommandProperty) as ICommand;

    public static double GetUpCommandParameter(TextBox element) =>
      element.GetValue(UpCommandParameterProperty) as double? ?? 0;
    public static void SetUpCommandParameter(TextBox element, object value) =>
      element.SetValue(UpCommandParameterProperty, value);

    public static double GetDownCommandParameter(TextBox element) =>
      element.GetValue(DownCommandParameterProperty) as double? ?? 0;
    public static void SetDownCommandParameter(TextBox element, object value) =>
      element.SetValue(DownCommandParameterProperty, value);

    private static void ExecuteUp(TextBox target)
    {
      double increment = GetUpCommandParameter(target);

      if (!double.TryParse(target.Text, out double value))
        value = 0;

      target.Text = (value + increment).ToString();
    }

    private static void ExecuteDown(TextBox target)
    {
      double increment = GetUpCommandParameter(target);

      if (!double.TryParse(target.Text, out double value))
        value = 0;

      target.Text = (value - increment).ToString();
    }
  }
}
