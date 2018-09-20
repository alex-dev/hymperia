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
  /// <summary>
  /// Logique d'interaction pour RotationForme.xaml
  /// </summary>
  public partial class RotationForme : UserControl
  {
    PipeVisual3D Cylindre { get; set; }

    public RotationForme()
    {
      Cylindre = new PipeVisual3D();
      InitializeComponent();
      viewport.Children.Add(Cylindre);

      RotateManipulator rotateManipulatorX = new RotateManipulator();
      rotateManipulatorX.Visibility = Visibility.Visible;
      rotateManipulatorX.Bind(Cylindre);
      rotateManipulatorX.Color = Colors.Aqua;
      viewport.Children.Add(rotateManipulatorX);


      
    }
  }
}
