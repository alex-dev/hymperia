using System.Windows;
using System.Windows.Controls;

namespace Hymperia.Facade.DependencyObjects
{
  public static class ImageHelper
  {
    private const string SourceResourceKey = nameof(SourceResourceKey);

    public static readonly DependencyProperty SourceResourceKeyProperty =
      DependencyProperty.RegisterAttached(
        SourceResourceKey,
        typeof(object),
        typeof(ImageHelper),
        new PropertyMetadata(string.Empty, SourceResourceKeyChanged));

    public static object GetSourceResourceKey(Image element) =>
      element.GetValue(SourceResourceKeyProperty);
    public static void SetSourceResourceKey(Image element, object value) =>
      element.SetValue(SourceResourceKeyProperty, value);

    private static void SourceResourceKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
      (d as Image)?.SetResourceReference(Image.SourceProperty, e.NewValue);
  }
}
