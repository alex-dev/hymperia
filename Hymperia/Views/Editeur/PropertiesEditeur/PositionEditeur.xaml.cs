using Hymperia.Facade.DependencyObjects;

namespace Hymperia.Facade.Views.Editeur.PropertiesEditeur
{
  public partial class PositionEditeur : DataUserControl
  {
    public PositionEditeur(PositionEditeurViewModel context)
    {
      DataContext = context;
      InitializeComponent();
    }
  }
}
