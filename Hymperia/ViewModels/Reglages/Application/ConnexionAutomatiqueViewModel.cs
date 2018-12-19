/*
 * Auteur : Antoine Mailhot
 * Date de création : 23 novembre 2018 
*/

using Hymperia.Facade.EventAggregatorMessages;
using Hymperia.Model.Modeles;
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
      set => SetProperty(ref selectionne, value, OnChanged);
    }

    public Utilisateur Utilisateur { get; set; }

    #endregion

    #region Constructeur

    public ConnexionAutomatiqueViewModel(IEventAggregator events)
    {
      events.GetEvent<ReglageUtilisateurChanged>().Subscribe(OnUtilisateurChanged);
    }

    #endregion

    #region Sauvegarder

    private void OnChanged()
    {
      S.Default.ConnexionAutomatique = Selectionne;
      S.Default.Utilisateur = Selectionne ? Utilisateur.Nom : string.Empty;
      S.Default.MotDePasse = Selectionne ? Utilisateur.MotDePasse : string.Empty;
    }

    #endregion

    private void OnUtilisateurChanged(Utilisateur utilisateur)
    {
      Utilisateur = utilisateur;
      Selectionne = utilisateur is Utilisateur
        && S.Default.ConnexionAutomatique
        && Utilisateur.Nom == S.Default.Utilisateur;
    }

    #region Private Fields

    private bool selectionne = false;
    
    #endregion
  }
}
