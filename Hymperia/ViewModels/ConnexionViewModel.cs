/**
 * Auteur : Antoine Mailhot
 * Date de création : 8 novembre 2018
 */

using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using Hymperia.Facade.Constants;
using Hymperia.Facade.Properties;
using Hymperia.Facade.Services;
using Hymperia.Facade.Titles;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Titles;
using B = BCrypt.Net;
using S = Hymperia.Model.Properties.Settings;

namespace Hymperia.Facade.ViewModels
{
  public class ConnexionViewModel : BindableBase, INavigationAware, INotifyDataErrorInfo
  {
    #region Properties

    public string Username
    {
      get => username;
      set => SetProperty(ref username, value);
    }

    public DelegateCommand<PasswordBox> Connexion { get; }
    public DelegateCommand Inscription { get; }
    public DelegateCommand ConnexionAutomatique { get; }

    #endregion

    #region Constructeur

    public ConnexionViewModel(ContextFactory factory, IRegionManager manager, [NotNull] ITitleAggregator titles)
    {
      Factory = factory;
      Manager = manager;

      ConnexionAutomatique = new DelegateCommand(_ConnexionAutomatique, CanConnect);
      Connexion = new DelegateCommand<PasswordBox>(_Connexion);
      Inscription = new DelegateCommand(_Inscription);
      MainWindowTitle = titles.GetTitle<MainWindowTitle>();
    }

    #endregion

    private void _ConnexionAutomatique() =>
      _Connexion(S.Default.Utilisateur, utilisateur => S.Default.MotDePasse == utilisateur.MotDePasse);

    private bool CanConnect() => S.Default.ConnexionAutomatique;

    private void _Connexion(PasswordBox password) =>
      _Connexion(Username, utilisateur => B.BCrypt.Verify(password.Password, utilisateur.MotDePasse, true));

    private void _Connexion(string username, Predicate<Utilisateur> validate)
    {
      Utilisateur utilisateur;

      using (var context = Factory.GetContext())
        utilisateur = context.Utilisateurs.SingleOrDefault(u => u.Nom == username);

      HasErrors = !(utilisateur is Utilisateur && validate(utilisateur));

      if (HasErrors)
        Username = username;
      else
        Navigate(utilisateur);
    }

    #region Navigation

    private void Navigate(Utilisateur utilisateur) =>
      Manager.RequestNavigate(RegionKeys.ContentRegion, NavigationKeys.AffichageProjets, new NavigationParameters
      {
        { NavigationParameterKeys.Utilisateur, utilisateur }
      });

    private void _Inscription() =>
      Manager.RequestNavigate(RegionKeys.ContentRegion, NavigationKeys.Inscription);

    #endregion

    #region INavigationAware 

    public bool IsNavigationTarget(NavigationContext context) => true;

    public void OnNavigatedTo(NavigationContext context)
    {
      MainWindowTitle.SetTitle();
    }

    public void OnNavigatedFrom(NavigationContext context) { }

    #endregion

    #region INotifyDataErrorInfo

    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    public bool HasErrors
    {
      get => errors;
      private set => SetProperty(ref errors, value);
    }

    public IEnumerable GetErrors(string propertyName)
    {
      if (HasErrors)
        yield return Resources.InvalidCredential;
    }

    #endregion

    #region Services

    [NotNull]
    private readonly ContextFactory Factory;
    [NotNull]
    private readonly IRegionManager Manager;
    [NotNull]
    private readonly MainWindowTitle MainWindowTitle;

    #endregion

    #region Private Fields

    private string username = string.Empty;
    private bool errors;

    #endregion
  }
}
