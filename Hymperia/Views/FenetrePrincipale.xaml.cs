using Hymperia.Facade.Constants;
using Prism.Regions;
using System.Windows;

namespace Hymperia.Facade.Views
{
  public partial class FenetrePrincipale : Window
  {
    public FenetrePrincipale(IRegionManager manager)
    {
      Loaded += (sender, e) => manager.RequestNavigate(RegionKeys.ContentRegion, NavigationKeys.Connexion);
      InitializeComponent();
    }
  }
}
