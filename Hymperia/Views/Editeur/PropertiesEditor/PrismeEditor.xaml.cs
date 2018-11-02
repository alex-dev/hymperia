using System.Windows.Controls;
using Hymperia.Facade.ModelWrappers;
using Prism.Regions;

namespace Hymperia.Facade.Views.Editeur.PropertiesEditor
{
  public partial class PrismeEditor : UserControl, INavigationAware
  {
    public PrismeEditor()
    {
      InitializeComponent();
    }

    #region INavigationAware 

    public bool IsNavigationTarget(NavigationContext context) => context.Parameters[NavigationParameterKeys.Forme] is PrismeRectangulaireWrapper;
    public void OnNavigatedTo(NavigationContext context) => DataContext = (PrismeRectangulaireWrapper)context.Parameters[NavigationParameterKeys.Forme];
    public void OnNavigatedFrom(NavigationContext context) => DataContext = null;

    #endregion
  }
}
