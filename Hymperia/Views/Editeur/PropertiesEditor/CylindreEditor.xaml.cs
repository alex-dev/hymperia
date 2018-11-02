using System.Windows.Controls;
using Hymperia.Facade.ModelWrappers;
using Prism.Regions;

namespace Hymperia.Facade.Views.Editeur.PropertiesEditor
{
  public partial class CylindreEditor : UserControl, INavigationAware
  {
    public CylindreEditor()
    {
      InitializeComponent();
    }

    #region INavigationAware 

    public bool IsNavigationTarget(NavigationContext context) => context.Parameters[NavigationParameterKeys.Forme] is CylindreWrapper;
    public void OnNavigatedTo(NavigationContext context) => DataContext = (CylindreWrapper)context.Parameters[NavigationParameterKeys.Forme];
    public void OnNavigatedFrom(NavigationContext context) => DataContext = null;

    #endregion
  }
}
