using System.Windows.Data;
using Hymperia.Facade.DependencyObjects;
using Hymperia.Facade.ViewModels.Editeur.PropertiesEditeur;

namespace Hymperia.Facade.Views.Editeur.PropertiesEditeur
{
  public partial class CylindreEditeur : DataUserControl
  {
    public CylindreEditeur(FormePropertiesViewModel context)
    {
      DataContext = context;
      InitializeComponent();
      SetBinding(DataProperty, new Binding("Forme") { Source = DataContext, Mode = BindingMode.OneWayToSource });
    }
  }
}
