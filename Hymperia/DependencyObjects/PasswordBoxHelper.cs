using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Hymperia.Facade.DependencyObjects
{
  public static class PasswordBoxHelper
  {
    #region Dependency Properties

    private const string Password = nameof(Password);
    private const string Attach = nameof(Attach);
    private const string IsUpdating = nameof(IsUpdating);

    public static readonly DependencyProperty PasswordProperty =
      DependencyProperty.RegisterAttached(
        Password,
        typeof(string),
        typeof(PasswordBoxHelper),
        new FrameworkPropertyMetadata(
          string.Empty,
          FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
          OnPasswordChanged,
          (o, value) => value,
          true,
          UpdateSourceTrigger.PropertyChanged));

    public static readonly DependencyProperty AttachProperty =
      DependencyProperty.RegisterAttached(Attach,
        typeof(bool),
        typeof(PasswordBoxHelper),
        new PropertyMetadata(false, OnAttachChanged));

    private static readonly DependencyProperty IsUpdatingProperty =
      DependencyProperty.RegisterAttached(IsUpdating, typeof(bool), typeof(PasswordBoxHelper));

    #endregion

    #region Attached Properties Accessors

    public static string GetPassword(DependencyObject dp) =>
      (string)dp.GetValue(PasswordProperty);

    public static void SetPassword(DependencyObject dp, string value) =>
      dp.SetValue(PasswordProperty, value);

    public static bool GetAttach(DependencyObject dp) =>
      (bool)dp.GetValue(AttachProperty);

    public static void SetAttach(DependencyObject dp, bool value) =>
      dp.SetValue(AttachProperty, value);

    private static bool GetIsUpdating(DependencyObject dp) =>
      (bool)dp.GetValue(IsUpdatingProperty);

    private static void SetIsUpdating(DependencyObject dp, bool value) =>
      dp.SetValue(IsUpdatingProperty, value);

    #endregion

    private static void OnAttachChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      if (!(sender is PasswordBox box))
        return;

      if ((bool)e.OldValue)
        box.PasswordChanged -= OnPasswordChanged;

      if ((bool)e.NewValue)
        box.PasswordChanged += OnPasswordChanged;
    }

    private static void OnPasswordChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      if (!(sender is PasswordBox box))
        return;

      if (GetIsUpdating(box))
        return;

      using (Monitor.Create(box))
        box.Password = (string)e.NewValue;
    }

    private static void OnPasswordChanged(object sender, RoutedEventArgs e)
    {
      if (!(sender is PasswordBox box))
        return;

      using (Monitor.Create(box))
        SetPassword(box, box.Password);
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
