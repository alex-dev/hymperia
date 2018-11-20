using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hymperia.Facade.DependencyObjects
{
  /// <summary>Provides masking behavior for any <see cref="TextBox"/>.</summary>
  public static class MaskingBehavior
  {
    const string IsUpdating = nameof(IsUpdating);
    const string MaskExpression = nameof(MaskExpression);
    const string Mask = nameof(Mask);

    private static readonly DependencyProperty IsUpdatingProperty =
      DependencyProperty.Register(IsUpdating, typeof(bool), typeof(MaskingBehavior));

    private static readonly DependencyPropertyKey MaskExpressionPropertyKey =
      DependencyProperty.RegisterAttachedReadOnly(MaskExpression, typeof(Regex), typeof(MaskingBehavior), new FrameworkPropertyMetadata());

    /// <summary>Identifies the <see cref="Mask"/> dependency property.</summary>
    public static readonly DependencyProperty MaskProperty =
      DependencyProperty.RegisterAttached(Mask, typeof(string), typeof(MaskingBehavior), new FrameworkPropertyMetadata(OnMaskChanged));

    /// <summary>Identifies the <see cref="MaskExpression"/> dependency property.</summary>
    public static readonly DependencyProperty MaskExpressionProperty = MaskExpressionPropertyKey.DependencyProperty;

    public static string GetMask(TextBox box) =>
      box.GetValue(MaskProperty) as string;
    public static void SetMask(TextBox box, string mask) =>
      box.SetValue(MaskProperty, mask);
    private static void ClearMask(TextBox box) =>
      box.ClearValue(MaskProperty);

    public static Regex GetMaskExpression(TextBox box) =>
      box.GetValue(MaskExpressionProperty) as Regex;
    private static void SetMaskExpression(TextBox box, Regex regex) =>
      box.SetValue(MaskExpressionPropertyKey, regex);
    private static void ClearMaskExpression(TextBox box) =>
      box.ClearValue(MaskExpressionPropertyKey);

    private static bool GetIsUpdating(TextBox box) =>
      box.GetValue(IsUpdatingProperty) as bool? ?? false;
    public static void SetIsUpdating(TextBox box, bool mask) =>
      box.SetValue(IsUpdatingProperty, mask);
    private static void ClearIsUpdating(TextBox box) =>
      box.ClearValue(IsUpdatingProperty);

    public static void Clear(TextBox box)
    {
      ClearMask(box);
      ClearMaskExpression(box);
      ClearIsUpdating(box);
    }

    private static void OnMaskChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      const RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture;

      string mask = e.NewValue as string;
      var box = d as TextBox;

      if (box is null)
        return;

      box.PreviewTextInput -= OnTextBoxPreviewText;
      box.PreviewKeyDown -= OnTextBoxPreviewText;
      DataObject.RemovePastingHandler(box, OnPasting);

      if (mask is null)
        Clear(box);
      else
      {
        SetMaskExpression(box, new Regex(mask, options));
        box.PreviewTextInput += OnTextBoxPreviewText;
        box.PreviewKeyDown += OnTextBoxPreviewText;
        DataObject.AddPastingHandler(box, OnPasting);
      }
    }

    private static void OnTextBoxPreviewText(object sender, TextCompositionEventArgs e)
    {
      if (!GetMaskExpression((TextBox)sender).IsMatch(ConstructProposedText((TextBox)sender, e.Text)))
        e.Handled = true;
    }

    private static void OnTextBoxPreviewText(object sender, KeyEventArgs e)
    {
      // Pressing space doesn't raise PreviewTextInput, so we need to handle explicitly here.
      if (e.Key == Key.Space &&
        !GetMaskExpression((TextBox)sender).IsMatch(ConstructProposedText((TextBox)sender, " ")))
        e.Handled = true;
    }

    private static void OnPasting(object sender, DataObjectPastingEventArgs e)
    {
      if (!e.DataObject.GetDataPresent(typeof(string))
        || !GetMaskExpression((TextBox)sender).IsMatch(
          ConstructProposedText((TextBox)sender, (string)e.DataObject.GetData(typeof(string)))))
        e.CancelCommand();
    }

    private static string ConstructProposedText(TextBox textBox, string newtext)
    {
      var text = textBox.Text;

      if (textBox.SelectionStart != -1)
        text = text.Remove(textBox.SelectionStart, textBox.SelectionLength);

      text = text.Insert(textBox.CaretIndex, newtext);

      return text;
    }
  }
}
