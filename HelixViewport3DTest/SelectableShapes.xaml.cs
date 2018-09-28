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
  /// Logique d'interaction pour SelectableShapes.xaml
  /// </summary>
  public partial class SelectableShapes : UserControl
  {

    BoxVisual3D shape = new BoxVisual3D();
    public SelectableShapes()
    {
      InitializeComponent();

      viewport.Children.Add(shape);
    }

    private void viewport_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {

    }
  }
}
