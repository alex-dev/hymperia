/*
 * Auteur : Antoine Mailhot
 * Date de création : 9 décembre 2018
*/

using Hymperia.Facade.EventAggregatorMessages;
using Hymperia.Model.Modeles;
using Prism.Events;
using Prism.Mvvm;
using S = Hymperia.Model.Properties.Settings;

namespace Hymperia.Facade.ViewModels.Reglages.Application
{
  public class ChangementThemeViewModel : BindableBase
  {
    #region Propriete

    public string Theme
    {
      get => theme;
      set => SetProperty(ref theme, value, OnThemeChanged);
    }

    public Utilisateur Utilisateur { get; set; }

    #endregion

    #region Constructeur

    public ChangementThemeViewModel(IEventAggregator events)
    {
      events.GetEvent<ReglageUtilisateurChanged>().Subscribe(OnUtilisateurChanged);
    }

    #endregion

    private void OnThemeChanged()
    {
      Utilisateur.Theme = Theme;
      S.Default.Theme = Theme;
    }

    private void OnUtilisateurChanged(Utilisateur utilisateur)
    {
      Utilisateur = utilisateur;
      theme = utilisateur?.Theme ?? S.Default.Theme;

      RaisePropertyChanged(nameof(Theme));
    }

    #region Private Fields

    private string theme = S.Default.Theme;

    #endregion
  }
}
