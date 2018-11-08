using Prism.Regions;
using System.Windows;

namespace Hymperia.Facade.Views
{
  public partial class FenetrePrincipale : Window
  {
    public FenetrePrincipale(IRegionManager manager)
    {
      manager.RegisterViewWithRegion("ContentRegion", typeof(Connexion));
      InitializeComponent();
    }
  }
}
