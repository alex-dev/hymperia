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
  public partial class FormBrushesExploration : UserControl
  {
    private SphereVisual3D Sphere { get; set; }

    private void RafraichirPointEtCouleur()
    {
      PointDebut = new Point(Point1X, Point1Y);
      PointFin = new Point(Point2X, Point2Y);

      Couleur1 = Color.FromArgb(Couleur1A, Couleur1R, Couleur1G, Couleur1B);
      Couleur2 = Color.FromArgb(Couleur2A, Couleur2R, Couleur2G, Couleur2B);
    }

    public FormBrushesExploration()
    {
      CouleurA = 255;
      Sphere = new SphereVisual3D { };
      ModificationMateriauxARGB(null, null);
      InitializeComponent();
      viewport.Children.Add(Sphere);
    }

    #region SolidColorBrush

    public byte CouleurA { get; set; }
    public byte CouleurR { get; set; }
    public byte CouleurG { get; set; }
    public byte CouleurB { get; set; }

    public void ModificationMateriauxARGB(object sender, RoutedEventArgs e)
    {
      var couleur = Color.FromArgb(CouleurA, CouleurR, CouleurG, CouleurB);
      var maBrush = new SolidColorBrush(couleur);
      Sphere.Fill = maBrush;
    }

    #endregion

    #region LinearGradientBrush
    public double Point1X { get; set; }
    public double Point1Y { get; set; }
    public double Point2X { get; set; }
    public double Point2Y { get; set; }

    public byte Couleur1A { get; set; }
    public byte Couleur1R { get; set; }
    public byte Couleur1G { get; set; }
    public byte Couleur1B { get; set; }
    public byte Couleur2A { get; set; }
    public byte Couleur2R { get; set; }
    public byte Couleur2G { get; set; }
    public byte Couleur2B { get; set; }
    public Point PointDebut { get; set; }
    public Point PointFin { get; set; }
    public Color Couleur1 { get; set; }
    public Color Couleur2 { get; set; }

    private void ModificationLinearGradientBrush(object sender, RoutedEventArgs e)
    {
      RafraichirPointEtCouleur();
      LinearGradientBrush lgb = new LinearGradientBrush(Couleur1, Couleur2, PointDebut, PointFin);
      Sphere.Fill = lgb;
    }

    #endregion

    #region RadialGradiantBrush

    private void ModificationRadialGradientBrush(object sender, RoutedEventArgs e)
    {
      RafraichirPointEtCouleur();
      RadialGradientBrush rgb = new RadialGradientBrush(Couleur1, Couleur2);
      Sphere.Fill = rgb;
    }

    #endregion

  }
}
