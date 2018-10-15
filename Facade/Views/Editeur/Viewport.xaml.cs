using System.Windows;
using System.Windows.Data;
using Hymperia.Facade.BaseClasses;
using Hymperia.Facade.Services;
using Object = Hymperia.Model.Modeles.JsonObject;
using JetBrains.Annotations;

namespace Hymperia.Facade.Views.Editeur
{
  public partial class Viewport : RegionContextAwareUserControl
  {
    public static readonly DependencyProperty CursorPositionProperty;

    public Object.Point CursorPosition
    {
      get => (Object.Point)GetValue(CursorPositionProperty);
      set => SetValue(CursorPositionProperty, value);
    }

    static Viewport()
    {
      Point3DToPointConverter = new Point3DToPointConverter();
      CursorPositionProperty = DependencyProperty.Register("CursorPosition", typeof(Object.Point), typeof(Viewport));
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
    private static readonly Point3DToPointConverter Point3DToPointConverter;

    #endregion
  }
}
