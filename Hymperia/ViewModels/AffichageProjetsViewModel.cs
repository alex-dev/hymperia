/*
 * Auteur : Antoine Mailhot 
 * Date de création : 2018-10-19
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Hymperia.Facade.Collections;
using Hymperia.Facade.Constants;
using Hymperia.Facade.Loaders;
using Hymperia.Facade.Properties;
using Hymperia.Facade.Services;
using Hymperia.Model;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Prism;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;

namespace Hymperia.Facade.ViewModels
{
  public sealed class AffichageProjetsViewModel : BindableBase, IActiveAware, IDisposable
  {
    #region Properties

    #region Binding

    [CanBeNull]
    public Utilisateur Utilisateur
    {
      get => utilisateur;
      set => UtilisateurLoader.Loading = QueryUtilisateur(value, UpdateProjets);
    }

    [CanBeNull]
    public BulkObservableCollection<Projet> Projets
    {
      get => projets;
      set => SetProperty(ref projets, value);
    }

    #endregion

    #region Commands

    public ICommand NavigateToProjet { get; private set; }
    public ICommand SupprimerProjet { get; private set; }
    public ICommand AjouterProjet { get; private set; }

    #endregion

    #region Interaction Requests

    public InteractionRequest<IConfirmation> SupprimerProjetRequest { get; } = new InteractionRequest<IConfirmation>();
    public InteractionRequest<IConfirmation> AjouterProjetRequest { get; } = new InteractionRequest<IConfirmation>();

    #endregion

    #region Asynchronous Loading

    public AsyncLoader<Utilisateur> UtilisateurLoader { get; } = new AsyncLoader<Utilisateur>();
    public AsyncLoader<Projet> AjouterProjetLoader { get; } = new AsyncLoader<Projet>();
    public AsyncLoader SupprimerProjetLoader { get; } = new AsyncLoader();

    #endregion

    #endregion

    #region Constructors

    public AffichageProjetsViewModel([NotNull] ContextFactory factory, [NotNull] IRegionManager manager)
    {
      NavigateToProjet = new DelegateCommand<Projet>(_NavigateToProjet);
      SupprimerProjet = new DelegateCommand<IList>(
        projets => _SupprimerProjets(projets?.Cast<Projet>()),
        projets => CanSupprimerProjets(projets?.Cast<Projet>()));
      AjouterProjet = new DelegateCommand(_AjouterProjet);

      ContextFactory = factory;
      Manager = manager;
    }

    #endregion

    #region Command NavigateToProjet

    private void _NavigateToProjet(Projet projet)
    {
      // Force la création d'un context d'éditeur pour la durée de la navigation.
      // De cette façon, toutes les vues peuvent garantir que leur contexte est le même.
      using (ContextFactory.GetEditorContext())
        Manager.RequestNavigate("ContentRegion", NavigationKeys.Editeur, new NavigationParameters
        {
          { NavigationParameterKeys.Projet, projet }
        });
    }

    #endregion

    #region Command AjouterProjet

    private void _AjouterProjet()
    {
      void Execute(IConfirmation context)
      {
        if (context.Confirmed)
        {
          AjouterProjetLoader.Loading = ConfirmedCreerProjet(context.Content.ToString().Trim());
          AjouterProjetLoader.Loading.ContinueWith(
            result => _NavigateToProjet(result.Result),
            default,
            TaskContinuationOptions.OnlyOnRanToCompletion,
            TaskScheduler.FromCurrentSynchronizationContext());
        }
      }

      AjouterProjetRequest.Raise(new Confirmation
      {
        Title = Resources.AjouterProjet,
      }, Execute);
    }

    private async Task<Projet> ConfirmedCreerProjet(string nom)
    {
      if (string.IsNullOrWhiteSpace(nom))
        throw new ArgumentNullException(nameof(nom));

      var projet = Utilisateur.CreerProjet(nom);

      using (await AsyncLock.Lock(Context))
        await Context.SaveChangesAsync();

      return projet;
    }

    #endregion

    #region Command SupprimerProjet

    private void _SupprimerProjets(IEnumerable<Projet> projets)
    {
      void Execute(IConfirmation context)
      {
        if (context.Confirmed)
        {
          SupprimerProjetLoader.Loading = ConfirmedSupprimerProjets(projets);
        }
      }

      SupprimerProjetRequest.Raise(new Confirmation
      {
        Title = Resources.SupprimerProjets,
        Content = projets
      }, Execute);
    }

    private async Task ConfirmedSupprimerProjets(IEnumerable<Projet> projets)
    {
      using (await AsyncLock.Lock(Context))
      {
        foreach (var projet in projets)
        {
          bool removal = Utilisateur.EstPropietaireDe(projet);
          Utilisateur.RetirerProjet(projet);

          // Ici, on prend le contrôle de la suppression pour garantir la suppression totale du projet et laisser à
          // l'ORM la tâche de cleanup en retirant les acces dépendants en cascade.
          if (removal)
            Context.Remove(projet);
        }

        Projets.RemoveRange(projets);
        await Context.SaveChangesAsync();
      }
    }

    private bool CanSupprimerProjets(IEnumerable<Projet> projets) => projets is IEnumerable<Projet> && projets.Count() > 0;

    #endregion

    #region Queries

    private async Task<Utilisateur> QueryUtilisateur(Utilisateur _utilisateur, Action onChanged)
    {
      Utilisateur value = null;
      SetProperty(ref utilisateur, null, onChanged, nameof(Utilisateur));

      if (_utilisateur is Utilisateur)
      {
        using (await AsyncLock.Lock(Context))
          value = await Context.Utilisateurs.IncludeAcces().FindByIdAsync(_utilisateur.Id);

        SetProperty(ref utilisateur, value, onChanged, nameof(Utilisateur));
      }

      return value;
    }

    #endregion

    #region On Utilisateur Changed

    private void UpdateProjets() => Projets = Utilisateur is Utilisateur
        ? new BulkObservableCollection<Projet>(from acces in Utilisateur.Acces select acces.Projet)
        : null;

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
      DisposeContext(Context);
      Context = ContextFactory.GetContext();
    }

    private void OnDeactivation()
    {
      DisposeContext(Context);
      Context = null;
    }

#pragma warning restore 4014

    #endregion

    #region IDisposable

#pragma warning disable 4014 // Justification: The async call is meant to release resources after making sure every async calls running ended.

    public void Dispose() => DisposeContext(Context);

#pragma warning restore 4014

    public async Task DisposeContext(DatabaseContext context)
    {
      if (context is null)
        return;

      using (await AsyncLock.Lock(context))
        context.Dispose();
    }

    #endregion

    #region Services

    [NotNull]
    private readonly ContextFactory ContextFactory;
    [NotNull]
    private readonly IRegionManager Manager;
    [NotNull]
    private DatabaseContext Context;

    #endregion

    #region Private Fields

    private Utilisateur utilisateur;
    private BulkObservableCollection<Projet> projets;
    private bool isActive;

    #endregion
  }
}
