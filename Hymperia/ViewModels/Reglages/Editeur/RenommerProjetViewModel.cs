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
      set => SetProperty(ref nom, value, () => Projet.RenommerProjet(value));
    }

    public Projet Projet { get; set; }

    #endregion

    #region Constructeur

    public RenommerProjetViewModel(ICommandAggregator commands, IEventAggregator events)
    {
      events.GetEvent<ReglageProjetChanged>().Subscribe(OnProjetChanged);
    }

    #endregion

    private void OnProjetChanged(Projet projet)
    {
      Projet = projet;
      nom = Projet?.Nom;

      RaisePropertyChanged(nameof(Nom));
    }

    #region Private Fields

    private string nom;

    #endregion
  }
}
