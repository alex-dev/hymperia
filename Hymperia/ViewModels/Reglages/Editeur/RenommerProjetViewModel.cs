/*
 * Auteur : Antoine Mailhot
 * Date de création : 28 novembre 2018 
*/
using Hymperia.Facade.CommandAggregatorCommands;
using Hymperia.Facade.EventAggregatorMessages;
using Hymperia.Model.Modeles;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace Hymperia.Facade.ViewModels.Reglages.Editeur
{
  public class RenommerProjetViewModel : BindableBase
  {
    #region Propriete

    public string Nom
    {
      get => nom;
      set => SetProperty(ref nom, value);
    }

    public Projet Projet
    {
      get => projet;
      set => SetProperty(ref projet, value);
    }

    #endregion

    #region Constructeur

    public RenommerProjetViewModel(ICommandAggregator commands, IEventAggregator events)
    {
      commands.GetCommand<PreSauvegarderReglageEditeur>().RegisterCommand(new DelegateCommand(PreSauvegarder));
      events.GetEvent<ReglageProjetChanged>().Subscribe(OnProjetChanged);
    }

    #endregion

    #region Sauvegarder

    private void PreSauvegarder()
    {
      if (Nom != "")
      {
        Projet.RenommerProjet(Nom);
      }
    }

    #endregion

    private void OnProjetChanged(Projet projet) => Projet = projet;

    #region Private Fields

    private string nom = "";
    private Projet projet;

    #endregion
  }
}
