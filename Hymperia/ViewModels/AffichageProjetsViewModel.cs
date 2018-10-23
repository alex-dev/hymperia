/*
 * Auteur : Antoine Mailhot 
 * Date de création : 2018-10-19
*/ 

using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Hymperia.Facade.BaseClasses;
using Hymperia.Facade.Services;
using Hymperia.Model;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Prism.Commands;
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

    public AffichageProjetsViewModel([NotNull] ContextFactory factory, [NotNull] IRegionManager manager)
    {
      ContextFactory = factory;
      Manager = manager;
      NavigateToProjet = new DelegateCommand<Projet>(_NavigateToProjet);
    }

    #endregion

    #region Command NavigateToProjet

    private void _NavigateToProjet(Projet projet) =>
      Manager.RequestNavigate("ContentRegion", NavigationKeys.Editeur, new NavigationParameters
      {
        { NavigationParameterKeys.Projet, projet }
      });

    #endregion

    #region Queries

    private async Task QueryUtilisateur(Utilisateur _utilisateur, Action onChanged)
    {
      SetProperty(ref utilisateur, null, onChanged, nameof(Utilisateur));

      if (_utilisateur is Utilisateur)
      {
        await (Loading ?? Task.CompletedTask);

        using (var context = ContextFactory.GetContext())
        {
          context.Attach(_utilisateur);

          await context.LoadProjetsAsync(_utilisateur);
        }

        SetProperty(ref utilisateur, _utilisateur, onChanged, nameof(Utilisateur));
      }
    }

    #endregion

    #region On Utilisateur Changed

    private void UpdateProjets() => Projets = Utilisateur is Utilisateur
        ? new BulkObservableCollection<Projet>(from acces in Utilisateur.Acces select acces.Projet)
        : null;

    #endregion

    #region Services

    [NotNull]
    private readonly ContextFactory ContextFactory;
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
