/*
 * Auteur : Antoine Mailhot
 * Date de création : 9 décembre 2018
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
  class ChangementThemeViewModel : BindableBase
  {
    #region Propriete

    public string Theme
    {
      get => theme;
      set => SetProperty(ref theme, value);
    }

    public Utilisateur Utilisateur { get; set; }

    #endregion

    #region Constructeur

    public ChangementThemeViewModel(ICommandAggregator commands, IEventAggregator events)
    {
      commands.GetCommand<PreSauvegarderReglageApplication>().RegisterCommand(new DelegateCommand(PreSauvegarder));
      events.GetEvent<ReglageUtilisateurChanged>().Subscribe(OnUtilisateurChanged);
    }

    #endregion

    #region Sauvegarder

    private void PreSauvegarder()
    {
      S.Default.Theme = Theme;
    }

    #endregion

    private void OnUtilisateurChanged(Utilisateur utilisateur) => Utilisateur = utilisateur;

    #region Private Fields

    private string theme;

    #endregion
  }
}
