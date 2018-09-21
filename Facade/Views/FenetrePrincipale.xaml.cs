using System.Windows;
using Prism.Regions;
using Hymperia.Facade.Views.Editeur;

namespace Hymperia.Facade.Views
{
  public partial class FenetrePrincipale : Window
  {
    public FenetrePrincipale(IRegionManager manager)
    {
      InitializeComponent();
      manager.RegisterViewWithRegion("ContentRegion", typeof(ProjetEditeur));
    }
  }
}
