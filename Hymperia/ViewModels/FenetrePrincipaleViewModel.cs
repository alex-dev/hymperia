using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Hymperia.Facade.Services;
using Hymperia.Model.Modeles;
using Hymperia.Facade.Constants;
using JetBrains.Annotations;

namespace Hymperia.Facade.ViewModels
{
  public class FenetrePrincipaleViewModel : BindableBase
  {
    #region Properties

    public string UtilisateurLogin { get; set; }
    public string MotDepasseLogin { get; set; } = "$2y$15$eiI786bZMg0HrJP4BphbveEXb1UHmkkd5p8feoUpDqYwuvgHjik2q";

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
      Factory = factory;
      Manager = manager;
      Navigate = new DelegateCommand(NavigateToViewport/*, () => Utilisateur is Utilisateur*/).ObservesProperty(() => Utilisateur);

      //using (var context = factory.GetContext())
      //{
      //  Utilisateurs = new ReadOnlyObservableCollection<Utilisateur>(
      //    new ObservableCollection<Utilisateur>(context.Utilisateurs.ToList()));
      //}
    }

    private void NavigateToViewport()
    {
      // Si le mot de passe est bon..
      if (true) {
        using (var context = Factory.GetContext())
        {
          Utilisateur utilisateurLogin = context.Utilisateurs.Where(u => u.Nom == UtilisateurLogin) as Utilisateur;
        }

        Manager.RequestNavigate("ContentRegion", NavigationKeys.AffichageProjets, new NavigationParameters
        {
          { NavigationParameterKeys.Utilisateur, UtilisateurLogin }
        });
      }
    }

    #region Services

    [NotNull]
    private readonly ContextFactory Factory;

    #endregion

    #region Private Fields

    private readonly IRegionManager Manager;
    private Utilisateur utilisateur;

    #endregion
  }
}

