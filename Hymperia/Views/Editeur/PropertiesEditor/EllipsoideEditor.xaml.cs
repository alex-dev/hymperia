using System.Windows.Controls;
using Hymperia.Facade.Constants;
using Hymperia.Facade.ModelWrappers;
using Prism.Regions;

namespace Hymperia.Facade.Views.Editeur.PropertiesEditor
{
  public partial class EllipsoideEditor : UserControl, INavigationAware
  {
    public EllipsoideEditor()
    {
      InitializeComponent();
    }

    #region INavigationAware 

    public bool IsNavigationTarget(NavigationContext context) => context.Parameters[NavigationParameterKeys.Forme] is EllipsoideWrapper;
    public void OnNavigatedTo(NavigationContext context) => DataContext = (EllipsoideWrapper)context.Parameters[NavigationParameterKeys.Forme];
    public void OnNavigatedFrom(NavigationContext context) => DataContext = null;

    #endregion
  }
}
