using System.Windows;
using System.Windows.Controls;
using HelixToolkit.Wpf;

namespace Hymperia.HelixViewport3DTest
{
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
      PlaceUserControl(new FormBrushesExploration());
    }

    private void SpherePhiThetaExploration_Click(object sender, RoutedEventArgs e)
    {
        PlaceUserControl(new SpherePhiThetaExploration());
    }

    private void ManipulatorExploration_Click(object sender, RoutedEventArgs e)
    {
      PlaceUserControl(new ManipulatorExploration());
    }

    private void SelectionExploration_Click(object sender, RoutedEventArgs e)
    {
      PlaceUserControl(new SelectionExploration());
    }

  }
}
