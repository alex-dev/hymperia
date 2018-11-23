using System.Windows;

namespace Hymperia.Facade.DependencyObjects
{
  public static class TabItemNameBehavior
  {
    private const string TabItemName = nameof(TabItemName);

    public static readonly DependencyProperty TabItemNameProperty =
      DependencyProperty.RegisterAttached(
        TabItemName,
        typeof(string),
        typeof(TabItemNameBehavior),
        new PropertyMetadata(string.Empty));

    public static void SetTabItemName(DependencyObject element, string value) =>
      element.SetValue(TabItemNameProperty, value);

    public static string GetTabItemName(DependencyObject element) =>
      element.GetValue(TabItemNameProperty) as string;
  }
}
