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

<<<<<<< HEAD
    private void SelectionExploration_Click(object sender, RoutedEventArgs e)
    {
      PlaceUserControl(new SelectionExploration());
    }

=======
    private void DeplacementForme_Click(object sender, RoutedEventArgs e)
    {
      PlaceUserControl(new DeplacementForme());
    }
>>>>>>> 1140e3a7454185f8686d4f2cd33d534fbba215b7
  }
}
