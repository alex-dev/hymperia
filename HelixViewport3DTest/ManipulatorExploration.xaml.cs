using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using H = Hymperia.HelixViewport3DTest.Manipulators;
using HelixToolkit.Wpf;

namespace HelixViewport3DTest
{
  /* 
   * Ref: https://github.com/helix-toolkit/helix-toolkit/tree/develop/Source/Examples/WPF/ExampleBrowser/Examples/Manipulator
   * Ref: https://helixtoolkit.userecho.com/communities/1/topics/394-manipulator-scale-problem
   */
  public partial class ManipulatorExploration : UserControl
  {

    public ManipulatorExploration()
    {
      InitializeComponent();
      //var b = new CubeVisual3D { Center = new Point3D(0, 0, 0), SideLength = 3 };
      var b = new SphereVisual3D { Center = new Point3D(0, 0, 0), Radius = 1 };

      viewport.Children.Add(b);
      AddManipulator(b);
    }

    private void AddManipulator(SphereVisual3D source)
    {
      var m = new H.MovementManipulator
      {
        Diameter = (source.Radius * 2.2)
        //Diameter = source.SideLength * 1.1
      };

      m.Bind(source);
      viewport.Children.Add(m);
    }
  }
}
