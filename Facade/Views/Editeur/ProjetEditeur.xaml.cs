using System.Windows.Controls;
using Prism.Regions;

namespace Hymperia.Facade.Views.Editeur
{
  /// <summary>
  /// Logique d'interaction pour Editor.xaml
  /// </summary>
  public partial class ProjetEditeur : UserControl
  {
    public ProjetEditeur(IRegionManager manager)
    {
      InitializeComponent();
      Unloaded += (sender, args) => DataContext = null;
      manager.RegisterViewWithRegion("ViewportRegion", typeof(Viewport));
    }
  }
}
