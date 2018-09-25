using System.Windows;
using Prism.Regions;
using Hymperia.Facade.Views.Editeur;

namespace Hymperia.Facade.Views
{
  public partial class FenetrePrincipale : Window
  {
    private IRegionManager Manager { get; set; }

    public FenetrePrincipale(IRegionManager manager)
    {
      Manager = manager;
      InitializeComponent();
      manager.RegisterViewWithRegion("ContentRegion", typeof(ProjetEditeur));
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
      Manager.RequestNavigate("ContentRegion", "Test");
    }

    private void Open_Click(object sender, RoutedEventArgs e)
    {
      Manager.RequestNavigate("ContentRegion", "Editeur");
    }
  }
}
