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
using HelixToolkit.Wpf;

namespace HelixViewport3DTest
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
      rotateManipulatorX.Axis = new Vector3D(-10, 0, 0);
      rotateManipulatorX.Pivot = Cylindre.Point2;
      viewport.Children.Add(rotateManipulatorX);

      RotateManipulator rotateManipulatorY = new RotateManipulator();
      rotateManipulatorY.Visibility = Visibility.Visible;
      rotateManipulatorY.Bind(Cylindre);
      rotateManipulatorY.Color = Colors.Aqua;
      rotateManipulatorY.Axis = new Vector3D(0, -10, 0);
      rotateManipulatorY.Pivot = Cylindre.Point2;
      viewport.Children.Add(rotateManipulatorY);

      RotateManipulator rotateManipulatorZ = new RotateManipulator();
      rotateManipulatorZ.Visibility = Visibility.Visible;
      rotateManipulatorZ.Bind(Cylindre);
      rotateManipulatorZ.Color = Colors.Aqua;
      rotateManipulatorZ.Axis = new Vector3D(0, 0, -10);
      rotateManipulatorZ.Pivot = Cylindre.Point2;
      viewport.Children.Add(rotateManipulatorZ);

    }
  }
}
