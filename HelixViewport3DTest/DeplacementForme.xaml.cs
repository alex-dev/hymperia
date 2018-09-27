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
  /// Logique d'interaction pour DeplacementForme.xaml
  /// </summary>
  public partial class DeplacementForme : UserControl
  {

    PipeVisual3D  Cylindre { get; set; } 
    public DeplacementForme()
    {
      Cylindre = new PipeVisual3D();
      InitializeComponent();
      viewport.Children.Add(Cylindre);

      
      Cylindre.Point2 = new Point3D(0, 0, -10);

      

      // Translate X
      TranslateManipulator translateManipulatorX = new TranslateManipulator();
      translateManipulatorX.Visibility = Visibility.Visible;
      translateManipulatorX.Bind(Cylindre);
      translateManipulatorX.Color = Colors.DarkRed;
      translateManipulatorX.Direction = new Vector3D(1, 0, 0);
      viewport.Children.Add(translateManipulatorX);

      // Translate Y
      TranslateManipulator translateManipulatorY = new TranslateManipulator();
      translateManipulatorY.Visibility = Visibility.Visible;
      translateManipulatorY.Bind(Cylindre);
      translateManipulatorY.Color = Colors.AliceBlue;
      translateManipulatorY.Direction = new Vector3D(0,1,0);
      viewport.Children.Add(translateManipulatorY);

      // Translate Z
      TranslateManipulator translateManipulatorZ = new TranslateManipulator();
      translateManipulatorZ.Visibility = Visibility.Visible;
      translateManipulatorZ.Bind(Cylindre);
      translateManipulatorZ.Color = Colors.ForestGreen;
      translateManipulatorZ.Direction = new Vector3D(0,0,-1);
      viewport.Children.Add(translateManipulatorZ);

      //translateManipulatorX.MouseUp += MouseUpHandler;
      //translateManipulatorY.MouseUp += MouseUpHandler;
      //translateManipulatorZ.MouseUp += MouseUpHandler;

    }

    //private void MouseUpHandler(object sender, MouseButtonEventArgs e)
    //{
    //  Cylindre.BeginEdit();
    //  Cylindre.Point1 = Cylindre.Transform.Transform(Cylindre.Point1);
    //  Cylindre.Point2 = Cylindre.Transform.Transform(Cylindre.Point2);
    //  Cylindre.Transform = default;
    //  Cylindre.EndEdit();      
    //}

    public void Pause(object sender, RoutedEventArgs e)
    {
      return;
    }

    public void KeyDown(object sender, KeyEventArgs e)
    {
      if (Keyboard.IsKeyDown(Key.NumPad8))
        DeplacementGauche();
      if (Keyboard.IsKeyDown(Key.NumPad2))
        DeplacementDroite();
      if (Keyboard.IsKeyDown(Key.NumPad6))
        DeplacementHaut();
      if (Keyboard.IsKeyDown(Key.NumPad4))
        DeplacementBas();
    }

    public void DeplacementGauche()
    {
      Transform3D transform3D;
      Vector3D vector3D = new Vector3D();

      TranslateManipulator translateManipulator = new TranslateManipulator();
      //Point3D point1 = Cylindre.Point1;
      //Point3D point2 = Cylindre.Point2;
      //point1.X --;
      //point2.X --;
      //Cylindre.Point1 = point1;
      //Cylindre.Point2 = point2;
    }

    public void DeplacementDroite()
    {
      Point3D point1 = Cylindre.Point1;
      Point3D point2 = Cylindre.Point2;
      point1.X++;
      point2.X++;
      Cylindre.Point1 = point1;
      Cylindre.Point2 = point2;
    }

    public void DeplacementHaut()
    {
      Point3D point1 = Cylindre.Point1;
      Point3D point2 = Cylindre.Point2;
      point1.Y++;
      point2.Y++;
      Cylindre.Point1 = point1;
      Cylindre.Point2 = point2;
    }

    public void DeplacementBas()
    {
      Point3D point1 = Cylindre.Point1;
      Point3D point2 = Cylindre.Point2;
      point1.Y--;
      point2.Y--;
      Cylindre.Point1 = point1;
      Cylindre.Point2 = point2;
    }
    
  }
}
