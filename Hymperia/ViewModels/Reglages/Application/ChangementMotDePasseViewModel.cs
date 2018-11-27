/*
 * Auteur : Antoine Mailhot
 * Date de création : 21 novembre 2018
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Hymperia.Facade.CommandAggregatorCommands;
using Hymperia.Facade.EventAggregatorMessages;
using Hymperia.Facade.Properties;
using Hymperia.Facade.Services;
using Hymperia.Model;
using JetBrains.Annotations;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
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
      set
      {
        SetProperty(ref oldpassword, value);
      }
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

    public Model.Modeles.Utilisateur Utilisateur
    {
      get => utilisateur;
      set { SetProperty(ref utilisateur, value); }
    }

    public List<string> Erreurs
    {
      get => erreurs;
      set { SetProperty(ref erreurs, value); }
    }

    public DelegateCommand ChangementMotDePasse { get; }

    #endregion

    #region Constructeur

    public ChangementMotDePasseViewModel(ContextFactory context, ICommandAggregator commands, IEventAggregator events)
    {
      ContextFactory = context;
      commands.GetCommandOrCreate<PreSauvegarderReglageApplication>().RegisterCommand(new DelegateCommand(PreSauvegarderChangementMotDePasse));
      events.GetEvent<ReglageUtilisateurChanged>().Subscribe(OnUtilisateurChanged);
      ErreursChanged = events.GetEvent<ReglageErreursChanged>();

      ChangementMotDePasse = new DelegateCommand(_ChangementMotDePasse);
    }

    #endregion

    private void PreSauvegarderChangementMotDePasse()
    {
      if (ValidatationAncientMotDePasse())
        _ChangementMotDePasse();
      else
      {
        Erreurs.Add(Resources.InvalidCredential);
        RaiseErreursChanged();
      }
    }

    private void RaiseErreursChanged() => ErreursChanged.Publish(Erreurs);

    private void OnUtilisateurChanged(Model.Modeles.Utilisateur utilisateur) => Utilisateur = utilisateur;

    private void OnErreursChanged(List<string> erreurs) => Erreurs = erreurs;

    private async void _ChangementMotDePasse()
    {
      if (!await Validate())
      {
        Erreurs.Add(Resources.InvalidCredential);
        RaiseErreursChanged();
        return;
      }

      await ModificationUtilisateur();
    }

    #region Queries
    private async Task ModificationUtilisateur()
    {
      Utilisateur.MotDePasse = B.BCrypt.HashPassword(Password, Model.Modeles.Utilisateur.PasswordWorkFactor, true);

      using (var context = ContextFactory.GetReglageUtilisateurContext())
      {
        context.Context.Utilisateurs.Update(Utilisateur);
      }

      S.Default.MotDePasse = Utilisateur.MotDePasse;
      OnUtilisateurChanged(Utilisateur);
    }

    private bool ValidatationAncientMotDePasse()
    {
      return B.BCrypt.Verify(OldPassword, Utilisateur.MotDePasse, true);
    }

    #endregion

    #region ValidationBase

    protected override async Task ValidateAsync()
    {
      ValidateProperty<string>(nameof(Password));
      ValidateProperty<string>(nameof(Verification));
    }

    #endregion

    #region IActiveAware

    public event EventHandler IsActiveChanged;

    public bool IsActive
    {
      get => isActive;
      set
      {
        if (isActive == value)
          return;

        isActive = value;

        if (value)
          OnActivation();
        else
          OnDeactivation();

        IsActiveChanged?.Invoke(this, EventArgs.Empty);
      }
    }

#pragma warning disable 4014 // Justification: The async call is meant to release resources after making sure every async calls running ended.

    private void OnActivation()
    {
      if (ContextWrapper is null)
        ContextWrapper = ContextFactory.GetEditeurContext();
      else
        CancelDispose();
    }

    private void OnDeactivation() => DisposeContext();


#pragma warning restore 4014

    #endregion

    #region IDisposable

#pragma warning disable 4014 // Justification: The async call is meant to release resources after making sure every async calls running ended.

    public void Dispose() => DisposeContext();

#pragma warning restore 4014

    private async Task DisposeContext()
    {
      if (ContextWrapper is null)
        return;

      disposeToken = new CancellationTokenSource();
      using (await AsyncLock.Lock(ContextWrapper.Context, disposeToken.Token))
      {
        if (disposeToken.IsCancellationRequested)
          return;

        ContextWrapper.Dispose();
        ContextWrapper = null;
      }
    }

    private void CancelDispose() => disposeToken.Cancel();

    #endregion

    #region Services

    [NotNull]
    private readonly ContextFactory ContextFactory;
    [NotNull]
    private ContextFactory.IContextWrapper<DatabaseContext> ContextWrapper;
    [NotNull]
    private readonly ReglageErreursChanged ErreursChanged;

    #endregion

    #region Private Fields

    private Model.Modeles.Utilisateur utilisateur;
    private string password = "";
    private string verification = "";
    private bool isActive;
    private CancellationTokenSource disposeToken;
    private string oldpassword = "";
    private List<string> erreurs = new List<string>();

    #endregion
  }
}
