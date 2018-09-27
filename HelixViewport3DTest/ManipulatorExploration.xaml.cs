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
using System.Windows.Navigation;
using System.Windows.Shapes;

using HelixToolkit.Wpf;


namespace Hymperia.HelixViewport3DTest
{
  // Ref: https://github.com/helix-toolkit/helix-toolkit/tree/develop/Source/Examples/WPF/ExampleBrowser/Examples/Manipulator
  public partial class ManipulatorExploration : UserControl
  {

    public CombinedManipulator manipulator = new CombinedManipulator();

    public ManipulatorExploration()
    {
      InitializeComponent();
      viewport.Children.Add(AddManipulator());
    }

    private CombinedManipulator AddManipulator()
    {
      return new CombinedManipulator
      {
        Pivot = box1.Center,
        Diameter = (box1.Length + (box1.Length / 10)),

      };
    }
  }
}
