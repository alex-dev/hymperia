/*
 * Auteur : Antoine Mailhot
 * Date de création : 28 novembre 2018
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
using Hymperia.Model;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Prism;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using S = Hymperia.Model.Properties.Settings;

namespace Hymperia.Facade.ViewModels.Reglages.Editeur
{
  public sealed class ReglageViewModel : BindableBase, INavigationAware, IActiveAware, IDisposable
  {
    #region Properties

    #region Binding

    public Projet Projet
    {
      get => projet;
      set => ProjetLoader.Loading = QueryProjet(value, RaiseProjetChanged);
    }

    #endregion

    #region Interaction Requests

    public InteractionRequest<IConfirmation> ConfirmationRequest { get; } = new InteractionRequest<IConfirmation>();
    public InteractionRequest<INotification> InformationRequest { get; } = new InteractionRequest<INotification>();

    #endregion

    #region Asynchronous Loading

    public AsyncLoader SupprimerProjetLoader { get; } = new AsyncLoader();

    #endregion

    #region Commands

    public ICommand NavigateBack { get; }
    public ICommand Delete { get; }
    public ICommand Sauvegarder { get; }

    #endregion

    #region Asynchronous Loading

    public AsyncLoader<Projet> ProjetLoader { get; } = new AsyncLoader<Projet>();
    public AsyncLoader SaveLoader { get; } = new AsyncLoader();

    #endregion

    #endregion

    #region Constructors

    public ReglageViewModel([NotNull] ContextFactory factory, [NotNull] IRegionManager manager, [NotNull] ICommandAggregator commands, [NotNull] IEventAggregator events)
    {
      NavigateBack = new DelegateCommand(_NavigateBack);
      Delete = new DelegateCommand(_SupprimerProjet);
      Sauvegarder = new DelegateCommand(InteractionSauvegarder);

      ContextFactory = factory;
      Manager = manager;

      PreSauvegarder = commands.GetCommand<PreSauvegarderReglageEditeur>();
      ProjetChanged = events.GetEvent<ReglageProjetChanged>();
    }

    #endregion

    #region Navigation Commands

    private void _NavigateBack() =>
      Manager.Regions[RegionKeys.ContentRegion].NavigationService.Journal.GoBack();

    #endregion

    #region Command Sauvegarder

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

    #region Command SupprimerProjet

    private void _SupprimerProjet()
    {
      void Execute(IConfirmation context)
      {
        if (context.Confirmed)
          SupprimerProjetLoader.Loading = ConfirmedSupprimerProjets();
      }

      ConfirmationRequest.Raise(new Confirmation
      {
        Title = Resources.TitleConfirmDelete,
        Content = Resources.SupprimerProjetContent
      }, Execute);
    }

    private async Task ConfirmedSupprimerProjets()
    {
      Utilisateur user;
      using (await AsyncLock.Lock(ContextWrapper.Context))
      {
        // Broken if more than one owner or no owner!
        user = await (from acces in ContextWrapper.Context.Acces.IncludeProjets().IncludeUtilisateurs().AsNoTracking()
                      where acces.Projet == Projet && acces.DroitDAcces == Acces.Droit.Possession
                      select acces.Utilisateur).SingleAsync();

        ContextWrapper.Context.Remove(Projet);
        await ContextWrapper.Context.SaveChangesAsync();
      }

      // Recreate a nav journal.
      Manager.Regions[RegionKeys.ContentRegion].NavigationService.Journal.Clear();
      Manager.RequestNavigate(RegionKeys.ContentRegion, NavigationKeys.Connexion);
      Manager.RequestNavigate(RegionKeys.ContentRegion, NavigationKeys.AffichageProjets, new NavigationParameters
      {
        { NavigationParameterKeys.Utilisateur, user }
      });
    }

    #endregion

    #region Queries

    private async Task<Projet> QueryProjet(Projet _projet, Action onChanged)
    {
      Projet value = null;
      SetProperty(ref projet, null, onChanged, nameof(Projet));

      if (_projet is Projet)
      {
        using (await AsyncLock.Lock(ContextWrapper.Context))
          value = await ContextWrapper.Context.Projets.FindByIdAsync(_projet.Id);

        SetProperty(ref projet, value, onChanged, nameof(Projet));
      }

      return value;
    }

    #endregion

    #region ProjetChanged

    private void RaiseProjetChanged() => ProjetChanged.Publish(Projet);

    #endregion

    #region INavigationAware 

    public bool IsNavigationTarget(NavigationContext context) => context.Parameters[NavigationParameterKeys.Projet] is Projet;
    public void OnNavigatedTo(NavigationContext context) => Projet = (Projet)context.Parameters[NavigationParameterKeys.Projet];
    public void OnNavigatedFrom(NavigationContext context) => Projet = null;

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
        ContextWrapper = ContextFactory.GetReglageEditeurContext();
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
    private readonly PreSauvegarderReglageEditeur PreSauvegarder;
    [NotNull]
    private readonly ReglageProjetChanged ProjetChanged;

    [NotNull]
    private ContextFactory.IContextWrapper<DatabaseContext> ContextWrapper;

    #endregion

    #region Private Fields

    private Projet projet;
    private bool isActive;
    private CancellationTokenSource disposeToken;

    #endregion
  }
}
