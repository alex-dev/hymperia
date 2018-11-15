/**
* Auteur : Antoine Mailhot
* Date de création : 9 novembre 2018
*/

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Hymperia.Facade.Constants;
using Hymperia.Facade.Extensions;
using Hymperia.Facade.Services;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using B = BCrypt.Net;

namespace Hymperia.Facade.ViewModels
{
  public class InscriptionViewModel : ValidatingBase, INotifyDataErrorInfo
  {
    #region Properties

    [Required(ErrorMessage = "R1")]
    public string Username {
      get => username;
      set => SetProperty(ref username, value);
    }

    [Required(ErrorMessage = "R2")]
    [Compare(nameof(Verification), ErrorMessage = "dont match")]
    public string Password
    {
      get => password;
      set
      {
        if (SetProperty(ref password, value))
          ValidateProperty(Verification, nameof(Verification));
      }
    }


    [Required(ErrorMessage = "R3")]
    [Compare(nameof(Password), ErrorMessage = "dont match2")]
    public string Verification
    {
      get => verification;
      set
      {
        if (SetProperty(ref verification, value))
          ValidateProperty(Password, nameof(Password));
      }
    }

    public DelegateCommand Inscription { get; }

    #endregion

    #region Constructeur

    public InscriptionViewModel(ContextFactory factory, IRegionManager manager)
    {
      Factory = factory;
      Manager = manager;
      Inscription = new DelegateCommand(_Inscription);
    }

    #endregion

    private async void _Inscription()
    {
      if (!await ValidateAsync())
        return;

      var utilisateur = await CreateUtilisateur();
      Navigate(utilisateur);
    }

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
      Manager.RequestNavigate("ContentRegion", NavigationKeys.AffichageProjets, new NavigationParameters
      {
        { NavigationParameterKeys.Utilisateur, utilisateur }
      });

    #endregion

    #region ValidationBase

    private async Task<ValidationResult> ValidateUsernameAsync() =>
      await UsernameExists(Username)
        ? new ValidationResult("", new string[] { nameof(Username) })
        : ValidationResult.Success;

    protected override async Task<bool> ValidateAsync()
    {
      var usernameExists = await ValidateUsernameAsync();

      Errors.ClearErrors();
      InternalValidate();

      if (usernameExists != ValidationResult.Success)
        Errors.SetErrors(
          nameof(Username),
          Errors.GetErrors(nameof(Username)).ContinueWith(usernameExists));

      RaiseAllErrorsChanged();

      return !HasErrors;
    }

    protected override void RaiseAllErrorsChanged()
    {
      RaiseErrorsChanged(nameof(Username));
      RaiseErrorsChanged(nameof(Password));
      RaiseErrorsChanged(nameof(Verification));
    }

    #endregion

    #region Services

    [NotNull]
    private readonly ContextFactory Factory;
    [NotNull]
    private readonly IRegionManager Manager;

    #endregion

    #region Private Fields

    private string username;
    private string password;
    private string verification;

    #endregion

  }
}
