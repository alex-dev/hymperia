using System.Windows.Controls;
using HelixToolkit.Wpf;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Input;


namespace Hymperia.HelixViewport3DTest
{
  /// <summary>
  /// Logique d'interaction pour PlaceRandomShapes.xaml
  /// </summary>
  public partial class PlaceRandomShapes : UserControl
  {
    public PlaceRandomShapes()
    {
      InitializeComponent();
    }

    private void viewport_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      viewport.pos
      var point = e.GetPosition(viewport);
      viewport.Children.Add(new BoxVisual3D { Center = viewport.CursorPosition ?? default });
    }
  }
}
