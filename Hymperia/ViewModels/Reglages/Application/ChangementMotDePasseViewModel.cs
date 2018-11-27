/*
 * Auteur : Antoine Mailhot
 * Date de création : 21 novembre 2018
 */

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hymperia.Facade.CommandAggregatorCommands;
using Hymperia.Facade.EventAggregatorMessages;
using Hymperia.Facade.Properties;
using Hymperia.Facade.Services;
using Hymperia.Model;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using B = BCrypt.Net;
using S = Hymperia.Model.Properties.Settings;


namespace Hymperia.Facade.ViewModels.Reglages.Application
{
  public class ChangementMotDePasseViewModel : ValidatingBase, INotifyDataErrorInfo
  {
    #region Properties

    public string OldPassword
    {
      get => oldpassword;
      set => SetProperty(ref oldpassword, value);
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

    public Utilisateur Utilisateur { get; set; }

    #endregion

    #region Constructeur

    public ChangementMotDePasseViewModel(ICommandAggregator commands, IEventAggregator events)
    {
      commands.GetCommand<PreSauvegarderReglageApplication>().RegisterCommand(new DelegateCommand<List<string>>(PreSauvegarderChangementMotDePasse));
      events.GetEvent<ReglageUtilisateurChanged>().Subscribe(OnUtilisateurChanged);
    }

    #endregion

    private async void PreSauvegarderChangementMotDePasse(List<string> erreurs)
    {
      if (!await Validate())
      {
        erreurs.AddRange(Errors.GetErrors().Values
          .SelectMany(list => list.Select(item => item.ErrorMessage)).Distinct());
        return;
      }

      Utilisateur.MotDePasse = B.BCrypt.HashPassword(Password, Utilisateur.PasswordWorkFactor, true);

      if (S.Default.ConnexionAutomatique)
        S.Default.MotDePasse = Utilisateur.MotDePasse;
    }

    private void OnUtilisateurChanged(Utilisateur utilisateur) => Utilisateur = utilisateur;

    #region ValidationBase

    private bool ValidatationAncientMotDePasse() => ValidateProperty<string>(v =>
    {
      if (!B.BCrypt.Verify(OldPassword, Utilisateur.MotDePasse, true))
        Errors.SetErrors(
          nameof(OldPassword),
          new ValidationResult[] { new ValidationResult(Resources.InvalidCredential, new string[] { nameof(OldPassword) }) });
    }, nameof(OldPassword));

    protected override async Task ValidateAsync()
    {
      ValidateProperty<string>(nameof(Password));
      ValidateProperty<string>(nameof(Verification));
      ValidatationAncientMotDePasse();
    }

    #endregion

    #region Services

    [NotNull]
    private ContextFactory.IContextWrapper<DatabaseContext> ContextWrapper;

    #endregion

    #region Private Fields

    private string password = "";
    private string verification = "";
    private string oldpassword = "";

    #endregion
  }
}
