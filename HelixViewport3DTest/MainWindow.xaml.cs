using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HelixToolkit.Wpf;
using System.Windows.Media.Media3D;
using System.Windows.Media;

namespace Hymperia.HelixViewport3DTest
{
  /// <summary>
  /// Logique d'interaction pour MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private UserControl Control { get; set; }

    public MainWindow()
    {
      InitializeComponent();
    }

    private void PlaceUserControl(UserControl control)
    {
      grid.Children.Remove(Control);
      Grid.SetColumn(control, 1);
      grid.Children.Add(control);
      Control = control;
    }

    private void AddShapeOnCursor_Click(object sender, RoutedEventArgs e)
    {
      PlaceUserControl(new PlaceRandomShapes());
    }

    private void MaterialExploration_Click(object sender, RoutedEventArgs e)
    {
      PlaceUserControl(new MaterialExploration());
    }

    private void SpherePhiThetaExploration_Click(object sender, RoutedEventArgs e)
    {
        PlaceUserControl(new SpherePhiThetaExploration());
    }
        
    }
}
