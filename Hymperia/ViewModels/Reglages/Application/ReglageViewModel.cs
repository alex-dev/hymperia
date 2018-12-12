/*
 * Auteur : Antoine Mailhot
 * Date de création : 22 novembre 2018
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Hymperia.Facade.CommandAggregatorCommands;
using Hymperia.Facade.Constants;
using Hymperia.Facade.EventAggregatorMessages;
using Hymperia.Facade.Loaders;
using Hymperia.Facade.Properties;
using Hymperia.Facade.Services;
using Hymperia.Facade.Titles;
using Hymperia.Model;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Prism;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Titles;
using S = Hymperia.Model.Properties.Settings;

namespace Hymperia.Facade.ViewModels.Reglages.Application
{
  public sealed class ReglageViewModel : BindableBase, INavigationAware, IActiveAware, IDisposable
  {
    #region Properties

    #region Binding

    public Utilisateur Utilisateur
    {
      get => utilisateur;
      set => UtilisateurLoader.Loading = QueryUtilisateur(value, OnUtilisateurChange);
    }

    #endregion

    #region Interaction Requests

    public InteractionRequest<IConfirmation> ConfirmationRequest { get; } = new InteractionRequest<IConfirmation>();
    public InteractionRequest<INotification> InformationRequest { get; } = new InteractionRequest<INotification>();

    #endregion

    #region Commands

    public ICommand NavigateBack { get; }

    public ICommand Sauvegarder { get; }

    #endregion

    #region Asynchronous Loading

    public AsyncLoader<Utilisateur> UtilisateurLoader { get; } = new AsyncLoader<Utilisateur>();
    public AsyncLoader SaveLoader { get; } = new AsyncLoader();

    #endregion

    #endregion

    #region Constructors

    public ReglageViewModel([NotNull] ContextFactory factory, [NotNull] IRegionManager manager, [NotNull] ICommandAggregator commands, [NotNull] IEventAggregator events,
      [NotNull] ITitleAggregator titles)
    {
      NavigateBack = new DelegateCommand(_NavigateBack);
      Sauvegarder = new DelegateCommand(InteractionSauvegarder);

      ContextFactory = factory;
      Manager = manager;
      PreSauvegarder = commands.GetCommand<PreSauvegarderReglageApplication>();
      UtilisateurChanged = events.GetEvent<ReglageUtilisateurChanged>();
      MainWindowTitle = titles.GetTitle<MainWindowTitle>();
    }

    #endregion

    #region Navigation Commands

    private void _NavigateBack() =>
      Manager.Regions[RegionKeys.ContentRegion].NavigationService.Journal.GoBack();

    #endregion

    #region Command Sauvegarder

    private ICollection<string> _PreSauvegarder()
    {
      var errors = new List<string>();
      PreSauvegarder.Execute(errors);
      return errors;
    }

    private async Task _Sauvegarder()
    {
      S.Default.Save();
      using (await AsyncLock.Lock(ContextWrapper.Context))
        await ContextWrapper.Context.SaveChangesAsync();
    }

    #endregion

    #region Command InformationSauvegarde

    private void InteractionSauvegarder()
    {
      async Task ExecuteReussite(IConfirmation context)
      {
        if (context.Confirmed)
          await _Sauvegarder();
      }

      var errors = _PreSauvegarder();

      if (errors.Any())
        InformationRequest.Raise(new Notification
        {
          Title = Resources.Echec,
          Content = errors
        });
      else
        ConfirmationRequest.Raise(new Confirmation
        {
          Title = Resources.Reussite,
          Content = Resources.SaveDataInfo
        }, context => SaveLoader.Loading = ExecuteReussite(context));
    }

    #endregion

    #region Queries

    private async Task<Utilisateur> QueryUtilisateur(Utilisateur _utilisateur, Action onChanged)
    {
      Utilisateur value = null;
      SetProperty(ref utilisateur, null, onChanged, nameof(Utilisateur));

      if (_utilisateur is Utilisateur)
      {
        using (await AsyncLock.Lock(ContextWrapper.Context))
          value = await ContextWrapper.Context.Utilisateurs.FindByIdAsync(_utilisateur.Id);

        SetProperty(ref utilisateur, value, onChanged, nameof(Utilisateur));
      }

      return value;
    }

    #endregion

    #region UtilisateurChanged

    private void OnUtilisateurChange()
    {
      MainWindowTitle.SetTitle(Utilisateur);
      RaiseUtilisateurChanged();
    }

    private void RaiseUtilisateurChanged() => UtilisateurChanged.Publish(Utilisateur);

    #endregion

    #region INavigationAware 

    public bool IsNavigationTarget(NavigationContext context) => context.Parameters[NavigationParameterKeys.Utilisateur] is Utilisateur;

    public void OnNavigatedTo(NavigationContext context)
    {
      // Retitling delayed to OnProjetChanged after querying.
      Utilisateur = (Utilisateur)context.Parameters[NavigationParameterKeys.Utilisateur];
      S.Default.Reload();
    }

    public void OnNavigatedFrom(NavigationContext context) => Utilisateur = null;

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
        ContextWrapper = ContextFactory.GetReglageUtilisateurContext();
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
    private readonly IRegionManager Manager;
    [NotNull]
    private readonly PreSauvegarderReglageApplication PreSauvegarder;
    [NotNull]
    private readonly ReglageUtilisateurChanged UtilisateurChanged;
    [NotNull]
    private readonly MainWindowTitle MainWindowTitle;

    [NotNull]
    private ContextFactory.IContextWrapper<DatabaseContext> ContextWrapper;

    #endregion

    #region Private Fields

    private Utilisateur utilisateur;
    private bool isActive;
    private CancellationTokenSource disposeToken;

    #endregion
  }
}
