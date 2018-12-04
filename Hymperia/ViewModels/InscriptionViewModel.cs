/**
* Auteur : Antoine Mailhot
* Date de création : 9 novembre 2018
*/

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows.Input;
using Hymperia.Facade.Constants;
using Hymperia.Facade.Properties;
using Hymperia.Facade.Services;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using B = BCrypt.Net;
using S = Hymperia.Model.Properties.Settings;

namespace Hymperia.Facade.ViewModels
{
  public class InscriptionViewModel : ValidatingBase
  {
    #region Properties

    [Required(
      ErrorMessageResourceName = nameof(Resources.RequiredUsername),
      ErrorMessageResourceType = typeof(Resources))]
    public string Username
    {
      get => username;
      set => SetProperty(ref username, value);
    }

    /*[Required(
      ErrorMessageResourceName = nameof(Resources.RequiredPassword),
      ErrorMessageResourceType = typeof(Resources))]*/
    [Compare(nameof(Verification),
      ErrorMessageResourceName = nameof(Resources.DontMatchPassword),
      ErrorMessageResourceType = typeof(Resources))]
    public string Password
    {
      get => password;
      set
      {
        if (SetProperty(ref password, value))
          ValidateProperty<string>(nameof(Verification));
      }
    }


    /*[Required(
      ErrorMessageResourceName = nameof(Resources.RequiredVerification),
      ErrorMessageResourceType = typeof(Resources))]*/
    [Compare(nameof(Password),
      ErrorMessageResourceName = nameof(Resources.DontMatchPassword),
      ErrorMessageResourceType = typeof(Resources))]
    public string Verification
    {
      get => verification;
      set
      {
        if (SetProperty(ref verification, value))
          ValidateProperty<string>(nameof(Password));
      }
    }

    public ICommand NavigateBack { get; }
    public DelegateCommand Inscription { get; }
    public DelegateCommand ValidationUsername { get; }

    #endregion

    #region Constructeur

    public InscriptionViewModel(ContextFactory factory, IRegionManager manager)
    {
      NavigateBack = new DelegateCommand(_NavigateBack);
      Inscription = new DelegateCommand(_Inscription);
      ValidationUsername = new DelegateCommand(_ValidateUsername);

      Factory = factory;
      Manager = manager;
    }

    #endregion

    #region Navigation Commands

    private void _NavigateBack() =>
      Manager.Regions[RegionKeys.ContentRegion].NavigationService.Journal.GoBack();

    #endregion

    #region Inscription Commands

    private async void _Inscription()
    {
      if (!await Validate())
        return;

      var utilisateur = await CreateUtilisateur();
      Navigate(utilisateur);
    }

    private async void _ValidateUsername() => await ValidateUsername();

    #endregion

    #region Queries

    private async Task<bool> UsernameExists(string username)
    {
      using (var context = Factory.GetContext())
        return await context.Utilisateurs.AnyAsync(utilisateur => utilisateur.Nom == username);
    }

    private async Task<Utilisateur> CreateUtilisateur()
    {
      var utilisateur = new Utilisateur(Username, B.BCrypt.HashPassword(Password, Utilisateur.PasswordWorkFactor, true));

      using (var context = Factory.GetContext())
      {
        context.Utilisateurs.Add(utilisateur);
        await context.SaveChangesAsync();
      }

      return utilisateur;
    }

    #endregion

    #region Navigation

    private void Navigate(Utilisateur utilisateur) =>
      Manager.RequestNavigate(RegionKeys.ContentRegion, NavigationKeys.AffichageProjets, new NavigationParameters
      {
        { NavigationParameterKeys.Utilisateur, utilisateur }
      });

    #endregion

    #region ValidationBase

    private async Task<bool> ValidateUsername() => await ValidateProperty<string>(async v =>
    {
      if (await UsernameExists(v))
        Errors.SetErrors(
          nameof(Username),
          new ValidationResult[] { new ValidationResult(Resources.UsedUsername, new string[] { nameof(Username) }) });
    }, nameof(Username));

    protected override async Task ValidateAsync()
    {
      await ValidateUsername();
      ValidateProperty<string>(nameof(Password));
      ValidateProperty<string>(nameof(Verification));
    }

    #endregion

    #region Services

    [NotNull]
    private readonly ContextFactory Factory;
    [NotNull]
    private readonly IRegionManager Manager;

    #endregion

    #region Private Fields

    private string username = string.Empty;
    private string password = string.Empty;
    private string verification = string.Empty;

    #endregion
  }
}
