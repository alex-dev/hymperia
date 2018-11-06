using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Hymperia.Facade.Services;
using Hymperia.Model.Modeles;
using Hymperia.Facade.Constants;

namespace Hymperia.Facade.ViewModels
{
  public class FenetrePrincipaleViewModel : BindableBase
  {
    #region Properties

    public Utilisateur Utilisateur
    {
      get => utilisateur;
      set => SetProperty(ref utilisateur, value);
    }

    public ReadOnlyObservableCollection<Utilisateur> Utilisateurs { get; private set; }

    public ICommand Navigate { get; private set; }

    #endregion

    public FenetrePrincipaleViewModel(ContextFactory factory, IRegionManager manager)
    {
      Manager = manager;
      Navigate = new DelegateCommand(NavigateToViewport, () => Utilisateur is Utilisateur).ObservesProperty(() => Utilisateur);

      using (var context = factory.GetContext())
      {
        Utilisateurs = new ReadOnlyObservableCollection<Utilisateur>(
          new ObservableCollection<Utilisateur>(context.Utilisateurs.ToList()));
      }
    }

    private void NavigateToViewport()
    {
      Manager.RequestNavigate("ContentRegion", NavigationKeys.AffichageProjets, new NavigationParameters
      {
        { NavigationParameterKeys.Utilisateur, Utilisateur }
      });
    }

    #region Private Fields

    private readonly IRegionManager Manager;
    private Utilisateur utilisateur;

    #endregion
  }
}

