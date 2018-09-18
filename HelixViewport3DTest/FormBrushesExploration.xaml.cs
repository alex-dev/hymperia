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
  public partial class FormBrushesExploration : UserControl
  {
    private SphereVisual3D Sphere { get; set; }

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
    #endregion

    #region RadialGradiantBrush
    #endregion

    public FormBrushesExploration()
    {
      CouleurA = 255;
      Sphere = new SphereVisual3D { };
      ModificationMateriauxARGB(null, null);
      InitializeComponent();
      viewport.Children.Add(Sphere);
    }
  }
}
