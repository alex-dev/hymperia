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

namespace Hymperia.HelixViewport3DTest
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
      Point3D point1 = Cylindre.Point1;
      Point3D point2 = Cylindre.Point2;
      point1.X --;
      point2.X --;
      Cylindre.Point1 = point1;
      Cylindre.Point2 = point2;
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
