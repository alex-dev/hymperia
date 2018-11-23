using System.Windows;
using System.Windows.Controls;

namespace Hymperia.Facade.DependencyObjects
{
  public abstract class DataUserControl : UserControl
  {
    public static readonly DependencyProperty DataProperty =
      DependencyProperty.Register(nameof(Data), typeof(object), typeof(DataUserControl));

    public object Data
    {
      get => GetValue(DataProperty);
      set => SetValue(DataProperty, value);
    }

    public DataUserControl() { }
  }
}
