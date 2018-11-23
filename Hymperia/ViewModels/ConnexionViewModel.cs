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
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using B = BCrypt.Net;
using S = Hymperia.Model.Properties.Settings;

namespace Hymperia.Facade.ViewModels
{
  public class ConnexionViewModel : BindableBase, INotifyDataErrorInfo
  {
    #region Properties

    public string Username { get; set; }

    public DelegateCommand<PasswordBox> Connexion { get; }
    public DelegateCommand Inscription { get; }

    #endregion

    #region Constructeur

    public ConnexionViewModel(ContextFactory factory, IRegionManager manager)
    {
      Factory = factory;
      Manager = manager;

      if (S.Default.ConnexionAutomatique == 1)
        _ConnexionAutommatique();

      Connexion = new DelegateCommand<PasswordBox>(_Connexion);
      Inscription = new DelegateCommand(_Inscription);
    }

    #endregion

    private void _ConnexionAutommatique()
    {
      Utilisateur utilisateur;

      using (var context = Factory.GetContext())
        utilisateur = context.Utilisateurs.SingleOrDefault(u => u.Nom == S.Default.Utilisateur);

      Navigate(utilisateur);
    }

    private void _Connexion(PasswordBox password)
    {
      Utilisateur utilisateur;

      using (var context = Factory.GetContext())
        utilisateur = context.Utilisateurs.SingleOrDefault(u => u.Nom == Username);

      HasErrors = !(utilisateur is Utilisateur && B.BCrypt.Verify(password.Password, utilisateur.MotDePasse, true));

      if (!HasErrors)
      {
        S.Default.Utilisateur = utilisateur.Nom;
        S.Default.MotDePasse = B.BCrypt.HashPassword(utilisateur.MotDePasse, Utilisateur.PasswordWorkFactor, true);
        Navigate(utilisateur);
      }
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

    #endregion

    #region Private Fields

    private bool errors;

    #endregion
  }
}
