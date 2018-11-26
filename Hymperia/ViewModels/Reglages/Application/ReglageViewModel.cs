/*
 * Auteur : Antoine Mailhot
 * Date de création : 22 novembre 2018
*/

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Hymperia.Facade.CommandAggregatorCommands;
using Hymperia.Facade.EventAggregatorMessages;
using Hymperia.Facade.Loaders;
using Hymperia.Facade.Services;
using Hymperia.Model;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Prism;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using S = Hymperia.Model.Properties.Settings;

namespace Hymperia.Facade.ViewModels.Reglages.Application
{
  public sealed class ReglageViewModel : BindableBase, IActiveAware, IDisposable
  {
    #region Properties

    #region Binding

    public Utilisateur Utilisateur
    {
      get => utilisateur;
      set => UtilisateurLoader.Loading = QueryUtilisateur(value, RaiseUtilisateurChanged);
    }

    public List<string> Erreurs
    {
      get => erreurs;
      set => RaiseErreursChanged();
    }

    #endregion

    #region Commands

    public ICommand Sauvegarder { get; }

    #endregion

    #region Asynchronous Loading

    public AsyncLoader<List<string>> ErreursLoader { get; } = new AsyncLoader<List<string>>();
    public AsyncLoader<Utilisateur> UtilisateurLoader { get; } = new AsyncLoader<Utilisateur>();
    public AsyncLoader SaveLoader { get; } = new AsyncLoader();

    #endregion

    #endregion

    #region Constructors

    public ReglageViewModel([NotNull] ContextFactory factory, [NotNull] ICommandAggregator commands, [NotNull] IEventAggregator events)
    {
      Sauvegarder = new DelegateCommand(() => SaveLoader.Loading = _Sauvegarder());

      ContextFactory = factory;

      PreSauvegarder = commands.GetCommandOrCreate<PreSauvegarderReglageApplication>();
      UtilisateurChanged = events.GetEvent<ReglageUtilisateurChanged>();
      ErreursChanged = events.GetEvent<ReglageErreursChanged>();
    }

    #endregion

    #region Command Sauvegarder

    private async Task _Sauvegarder()
    {
      PreSauvegarder.Execute(null);
      if (Erreurs?.Count > 0)
      {
        return;
      }
      else
      {
        S.Default.Save();
        using (await AsyncLock.Lock(ContextWrapper.Context))
          await ContextWrapper.Context.SaveChangesAsync();
      }
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

    private async Task<List<string>> CheckErreurs(List<string> _erreurs, Action onChanged)
    {
      SetProperty(ref erreurs,null, onChanged, nameof(Erreurs));
      return _erreurs;
    }

    #region UtilisateurChanged

    private void RaiseUtilisateurChanged() => UtilisateurChanged.Publish(Utilisateur);

    private void RaiseErreursChanged() => ErreursChanged.Publish(Erreurs);

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
    private readonly PreSauvegarderReglageApplication PreSauvegarder;
    [NotNull]
    private readonly ReglageUtilisateurChanged UtilisateurChanged;
    [NotNull]
    private readonly ReglageErreursChanged ErreursChanged;

    [NotNull]
    private ContextFactory.IContextWrapper<DatabaseContext> ContextWrapper;

    #endregion

    #region Private Fields

    private Utilisateur utilisateur;
    private bool isActive;
    private CancellationTokenSource disposeToken;
    private List<string> erreurs;

    #endregion
  }
}
