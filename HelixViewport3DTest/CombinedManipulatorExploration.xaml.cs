using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using HelixToolkit.Wpf;

namespace HelixViewport3DTest
{
  public partial class CombinedManipulatorExploration : UserControl
  {
    private SphereVisual3D Sphere { get; set; }

    public CombinedManipulatorExploration()
    {
      var manipulator = new CombinedManipulator();
      Sphere = new SphereVisual3D() { Radius = 1 };
      manipulator.Bind(Sphere);
      InitializeComponent();

      viewport.Children.Add(Sphere);
      viewport.Children.Add(manipulator);
    }

    public void Pause(object sender, RoutedEventArgs e)
    {
      Debug.Assert(false);
    }
  }
}
