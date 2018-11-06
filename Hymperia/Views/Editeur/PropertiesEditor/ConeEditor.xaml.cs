using System.Windows.Controls;
using Hymperia.Facade.Constants;
using Hymperia.Facade.ModelWrappers;
using Prism.Regions;

namespace Hymperia.Facade.Views.Editeur.PropertiesEditor
{
  public partial class ConeEditor : UserControl, INavigationAware
  {
    public ConeEditor()
    {
      InitializeComponent();
    }

    #region INavigationAware 

    public bool IsNavigationTarget(NavigationContext context) => context.Parameters[NavigationParameterKeys.Forme] is ConeWrapper;
    public void OnNavigatedTo(NavigationContext context) => DataContext = (ConeWrapper)context.Parameters[NavigationParameterKeys.Forme];
    public void OnNavigatedFrom(NavigationContext context) => DataContext = null;

    #endregion
  }
}
