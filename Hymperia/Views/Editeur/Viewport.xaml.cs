using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Hymperia.Facade.Converters;
using JetBrains.Annotations;
using Object = Hymperia.Model.Modeles.JsonObject;

namespace Hymperia.Facade.Views.Editeur
{
  public partial class Viewport : UserControl
  {
    public static readonly DependencyProperty CursorPositionProperty =
      DependencyProperty.Register("CursorPosition", typeof(Object.Point), typeof(Viewport));

    public Object.Point CursorPosition
    {
      get => (Object.Point)GetValue(CursorPositionProperty);
      set => SetValue(CursorPositionProperty, value);
    }

    public Viewport() : base()
    {
      InitializeComponent();
      SetBinding(CursorPositionProperty, new Binding("CursorPosition")
      {
        Source = viewport,
        Converter = Point3DToPointConverter,
        Mode = BindingMode.OneWay
      });
    }

    #region Static Services

    [NotNull]
    private static readonly Point3DToPointConverter Point3DToPointConverter =
      (Point3DToPointConverter)Application.Current.Resources["Point3DToPoint"];

    #endregion
  }
}
