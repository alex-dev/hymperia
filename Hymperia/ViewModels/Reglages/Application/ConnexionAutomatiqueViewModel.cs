/*
 * Auteur : Antoine Mailhot
 * Date de création : 23 novembre 2018 
*/

using Hymperia.Facade.CommandAggregatorCommands;
using Hymperia.Facade.EventAggregatorMessages;
using Hymperia.Model.Modeles;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using S = Hymperia.Model.Properties.Settings;

namespace Hymperia.Facade.ViewModels.Reglages.Application
{
  public class ConnexionAutomatiqueViewModel : BindableBase
  {
    #region Propriete

    public bool Selectionne
    {
      get => selectionne;
      set => SetProperty(ref selectionne, value);
    }

    public Utilisateur Utilisateur { get; set; }

    #endregion

    #region Constructeur

    public ConnexionAutomatiqueViewModel(ICommandAggregator commands, IEventAggregator events)
    {
      commands.GetCommand<PreSauvegarderReglageApplication>().RegisterCommand(new DelegateCommand(PreSauvegarder));
      events.GetEvent<ReglageUtilisateurChanged>().Subscribe(OnUtilisateurChanged);
    }

    #endregion

    #region Sauvegarder

    private void PreSauvegarder()
    {
      S.Default.ConnexionAutomatique = Selectionne;
      S.Default.Utilisateur = Selectionne ? Utilisateur.Nom : string.Empty;
      S.Default.MotDePasse = Selectionne ? Utilisateur.MotDePasse : string.Empty;
    }

    #endregion

    private void OnUtilisateurChanged(Utilisateur utilisateur) => Utilisateur = utilisateur;

    #region Private Fields

    private bool selectionne = S.Default.ConnexionAutomatique;
    
    #endregion
  }
}
