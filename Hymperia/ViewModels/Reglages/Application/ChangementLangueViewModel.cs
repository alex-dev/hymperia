/*
 * Auteur : Antoine Mailhot
 * Date de création : 10 décembre 2018
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
  public class ChangementLangueViewModel : BindableBase
  {
    #region Propriete

    public string Langue
    {
      get => langue;
      set => SetProperty(ref langue, value, OnLangueChanged);
    }

    public Utilisateur Utilisateur { get; set; }

    #endregion

    #region Constructeur

    public ChangementLangueViewModel(IEventAggregator events)
    {
      events.GetEvent<ReglageUtilisateurChanged>().Subscribe(OnUtilisateurChanged);
    }

    #endregion

    private void OnLangueChanged()
    {
      Utilisateur.Langue = Langue;
      S.Default.Culture = Langue;
    }

    private void OnUtilisateurChanged(Utilisateur utilisateur)
    {
      Utilisateur = utilisateur;
      langue = utilisateur?.Langue ?? S.Default.Culture;

      RaisePropertyChanged(nameof(Langue));
    }

    #region Private Fields

    private string langue = S.Default.Culture;

    #endregion
  }
}
