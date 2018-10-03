using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Hymperia.Facade.Services;
using Hymperia.Model.Modeles;

namespace Hymperia.Facade.ViewModels
{
  public class FenetrePrincipaleViewModel : BindableBase
  {
    #region Attributes

    #region Fields

    private Projet projet;

    #endregion

    public Projet Projet
    {
      get => projet;
      set => SetProperty(ref projet, value);
    }

    public ReadOnlyObservableCollection<Projet> Projets { get; private set; }

    public ICommand Navigate { get; private set; }

    private readonly IRegionManager Manager;

    #endregion

    public FenetrePrincipaleViewModel(ContextFactory factory, IRegionManager manager)
    {
      Manager = manager;
      Navigate = new DelegateCommand(NavigateToViewport, () => Projet is Projet).ObservesProperty(() => Projet);

      using (var context = factory.GetContext())
      {
        Projets = new ReadOnlyObservableCollection<Projet>(
          new ObservableCollection<Projet>(context.Projets.ToList()));
      }
    }

    private void NavigateToViewport()
    {
      Manager.RequestNavigate("ContentRegion", "Editeur", new NavigationParameters
      {
        { "Projet", Projet }
      });
    }
  }
}
