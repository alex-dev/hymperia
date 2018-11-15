using System;
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

    public static Regex GetMaskExpression(TextBox box) =>
      box.GetValue(MaskExpressionProperty) as Regex;
    private static void SetMaskExpression(TextBox box, Regex regex) =>
      box.SetValue(MaskExpressionPropertyKey, regex);

    private static bool GetIsUpdating(TextBox box) =>
      box.GetValue(MaskProperty) as bool? ?? false;
    public static void SetMask(TextBox box, bool mask) =>
      box.SetValue(MaskProperty, mask);


    private static void OnMaskChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      string mask = e.NewValue as string;
      var box = d as TextBox;

      if (box is null)
        return;

      box.PreviewTextInput -= OnTextBoxPreviewText;
      box.PreviewKeyDown -= OnTextBoxPreviewText;
      DataObject.RemovePastingHandler(box, Pasting);

      if (mask == null)
      {
        box.ClearValue(MaskProperty);
        box.ClearValue(MaskExpressionProperty);
        box.ClearValue(IsUpdatingProperty);
      }
      else
      {
        textBox.SetValue(MaskProperty, mask);
        SetMaskExpression(textBox, new Regex(mask, RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace));
        textBox.PreviewTextInput += textBox_PreviewTextInput;
        textBox.PreviewKeyDown += textBox_PreviewKeyDown;
        DataObject.AddPastingHandler(textBox, Pasting);
      }
    }

    private static void OnTextBoxPreviewText(object sender, TextCompositionEventArgs e)
    {
      var textBox = sender as TextBox;
      var maskExpression = GetMaskExpression(textBox);

      if (maskExpression == null)
      {
        return;
      }

      var proposedText = GetProposedText(textBox, e.Text);

      if (!maskExpression.IsMatch(proposedText))
      {
        e.Handled = true;
      }
    }

    private static void textBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      var textBox = sender as TextBox;
      var maskExpression = GetMaskExpression(textBox);

      if (maskExpression == null)
      {
        return;
      }

      //pressing space doesn't raise PreviewTextInput - no idea why, but we need to handle
      //explicitly here
      if (e.Key == Key.Space)
      {
        var proposedText = GetProposedText(textBox, " ");

        if (!maskExpression.IsMatch(proposedText))
        {
          e.Handled = true;
        }
      }
    }

    private static void Pasting(object sender, DataObjectPastingEventArgs e)
    {
      var textBox = sender as TextBox;
      var maskExpression = GetMaskExpression(textBox);

      if (maskExpression == null)
      {
        return;
      }

      if (e.DataObject.GetDataPresent(typeof(string)))
      {
        var pastedText = e.DataObject.GetData(typeof(string)) as string;
        var proposedText = GetProposedText(textBox, pastedText);

        if (!maskExpression.IsMatch(proposedText))
        {
          e.CancelCommand();
        }
      }
      else
      {
        e.CancelCommand();
      }
    }

    private static string GetProposedText(TextBox textBox, string newText)
    {
      var text = textBox.Text;

      if (textBox.SelectionStart != -1)
      {
        text = text.Remove(textBox.SelectionStart, textBox.SelectionLength);
      }

      text = text.Insert(textBox.CaretIndex, newText);

      return text;
    }

    #region Disposable Pattern

    private class Monitor : IDisposable
    {
      private PasswordBox PasswordBox { get; }

      private Monitor(PasswordBox box)
      {
        PasswordBox = box;
      }

      public static Monitor Create(PasswordBox box)
      {
        SetIsUpdating(box, true);
        return new Monitor(box);
      }

      public void Dispose() => SetIsUpdating(PasswordBox, false);
    }

    #endregion
  }
}
