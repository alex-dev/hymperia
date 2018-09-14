using System.Windows.Controls;
using HelixToolkit.Wpf;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Input;
using System;

namespace Hymperia.HelixViewport3DTest
{
  /// <summary>
  /// Logique d'interaction pour PlaceRandomShapes.xaml
  /// </summary>
  public partial class PlaceRandomShapes : UserControl
  {
    private Random Random { get; set; }

    private Func<MeshElement3D>[] Generator { get; set; }

    public PlaceRandomShapes()
    {
      Random = new Random();
      Generator = new Func<MeshElement3D>[]
      {
        //CreateBox,
        CreateSphere,
      };

      InitializeComponent();
    }

    private void Viewport_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      viewport.Children.Add(Generator[Random.Next() % Generator.Length]());
    }

    private MeshElement3D CreateBox()
    {
      return new BoxVisual3D
      {
        Center = viewport.CursorPosition ?? default,
        Height = Random.Next(1, 5),
        Length = Random.Next(1, 5),
        Width = Random.Next(1, 5)
      };
    }

    private MeshElement3D CreateSphere()
    {
      return new SphereVisual3D
      {
        Center = viewport.CursorPosition ?? default,
        Radius = Random.Next(1, 5),
        //PhiDiv = Random.Next(0, 90),
        //ThetaDiv = Random.Next(0, 90)
      };

    }
    /*private MeshElement3D CreatePipe()
    {
      return new PipeVisual3D { Center = viewport.CursorPosition ?? default };

    }  */



  }
}
