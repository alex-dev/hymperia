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
  /// <remarks>
  ///   Auteur : Antoine Mailhot
  ///   Date de création : 18 octobre 2018
  /// </remarks>
  public class AffichageProjetsViewModel : BindableBase
  {
    #region Properties

    public ReadOnlyObservableCollection<Projet> Projets { get; private set; }

    public ICommand Navigate { get; private set; }
    public ICommand 

    #endregion

    public AffichageProjetsViewModel(ContextFactory factory, IRegionManager manager)
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

    #region Private Fields

    private readonly IRegionManager Manager;
    private Projet projet;

    #endregion
  }
}

