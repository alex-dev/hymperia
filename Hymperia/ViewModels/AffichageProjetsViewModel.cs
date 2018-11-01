/*
 * Auteur : Antoine Mailhot 
 * Date de création : 2018-10-19
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Hymperia.Facade.BaseClasses;
using Hymperia.Facade.Services;
using Hymperia.Model;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;

namespace Hymperia.Facade.ViewModels
{
  public class AffichageProjetsViewModel : BindableBase
  {
    #region Properties

    #region Binding

    [CanBeNull]
    public Utilisateur Utilisateur
    {
      get => utilisateur;
      set => Loading = QueryUtilisateur(value, UpdateProjets);
    }

    [CanBeNull]
    public BulkObservableCollection<Projet> Projets
    {
      get => projets;
      set => SetProperty(ref projets, value);
    }

    [NotNull]
    public System.Windows.Controls.SelectionMode SelectionMode
    {
      get => selection;
      set => SetProperty(ref selection, value);
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

    [CanBeNull]
    private Task Loading
    {
      get => loading;
      set => SetProperty(ref loading, value, () =>
      {
        IsLoading = true;
        value
          .ContinueWith(
            result => throw result.Exception.Flatten(),
            default,
            TaskContinuationOptions.OnlyOnFaulted,
            TaskScheduler.FromCurrentSynchronizationContext())
          .ContinueWith(result => IsLoading = false, TaskScheduler.FromCurrentSynchronizationContext());
      });
    }

    public bool IsLoading
    {
      get => isLoading;
      private set => SetProperty(ref isLoading, value);
    }

    #endregion

    #endregion

    #region Constructors

    public AffichageProjetsViewModel([NotNull] DatabaseContext context, [NotNull] IRegionManager manager)
    {
      Context = context;
      Manager = manager;
      NavigateToProjet = new DelegateCommand<Projet>(_NavigateToProjet);
      SupprimerProjet = new DelegateCommand<IList>(
        projets => _SupprimerProjets(projets?.Cast<Projet>()),
        projets => CanSupprimerProjets(projets?.Cast<Projet>()));
      AjouterProjet = new DelegateCommand(_AjouterProjet);
    }

    #endregion

    #region Command NavigateToProjet

    private void _NavigateToProjet(Projet projet) =>
      Manager.RequestNavigate("ContentRegion", NavigationKeys.Editeur, new NavigationParameters
      {
        { NavigationParameterKeys.Projet, projet }
      });

    #endregion

    #region Command SupprimerProjet

    private void _SupprimerProjets(IEnumerable<Projet> projets)
    {
      void Execute(IConfirmation context)
      {
        if (context.Confirmed)
        {
          Loading = ConfirmedSupprimerProjets(projets);
        }
      }

      SupprimerProjetRequest.Raise(new Confirmation
      {
        Title = "Supprimer les projets ?",
        Content = projets
      }, Execute);
    }

    private async Task ConfirmedSupprimerProjets(IEnumerable<Projet> projets)
    {
      await (Loading ?? Task.CompletedTask);

      foreach (var projet in projets)
      {
        if (Utilisateur.EstPropietaireDe(projet))
        {
          Context.Remove(projet);
        }
        else
        {
          Context.Remove(Utilisateur.Acces.First(acces => acces.Projet.Id == projet.Id));
        }

        Utilisateur.RetirerProjet(projet);
      }

      Projets.RemoveRange(projets);
      await Context.SaveChangesAsync();
    }

    private bool CanSupprimerProjets(IEnumerable<Projet> projets) => projets is IEnumerable<Projet> && projets.Count() > 0;

    #endregion

    #region Queries

    private async Task QueryUtilisateur(Utilisateur _utilisateur, Action onChanged)
    {
      SetProperty(ref utilisateur, null, onChanged, nameof(Utilisateur));

      if (_utilisateur is Utilisateur)
      {
        await (Loading ?? Task.CompletedTask);

        Context.Attach(_utilisateur);
        await Context.LoadProjetsAsync(_utilisateur);

        SetProperty(ref utilisateur, _utilisateur, onChanged, nameof(Utilisateur));
      }
    }

    #endregion

    #region Command AjouterProjet

    private void _AjouterProjet()
    {

      void Execute(IConfirmation context)
      {
        if (context.Confirmed)
        {
          Loading = ConfirmedCreerProjet(context.Content.ToString()).ContinueWith(
            result => _NavigateToProjet(result.Result),
            default,
            TaskContinuationOptions.OnlyOnRanToCompletion,
            TaskScheduler.FromCurrentSynchronizationContext());
        }
      }

      AjouterProjetRequest.Raise(new Confirmation
      {
        Title = "Ajouter un projet ?",
      }, Execute);
    }

    private async Task<Projet> ConfirmedCreerProjet(string nom)
    {
      Utilisateur.CreerProjet(nom);
      await Context.SaveChangesAsync();
      return await Context.Projets.SingleAsync(projet => projet.Nom == nom);
    }

    #endregion

    #region On Utilisateur Changed

    private void UpdateProjets() => Projets = Utilisateur is Utilisateur
        ? new BulkObservableCollection<Projet>(from acces in Utilisateur.Acces select acces.Projet)
        : null;

    #endregion

    #region Services

    [NotNull]
    private readonly DatabaseContext Context;
    [NotNull]
    private readonly IRegionManager Manager;

    #endregion

    #region Private Fields

    private Utilisateur utilisateur;
    private BulkObservableCollection<Projet> projets;
    private System.Windows.Controls.SelectionMode selection = System.Windows.Controls.SelectionMode.Single;
    private Task loading;
    private bool isLoading;


    #endregion
  }
}
