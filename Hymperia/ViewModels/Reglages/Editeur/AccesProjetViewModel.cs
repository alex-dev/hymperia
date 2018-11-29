/*
 * Auteur : Antoine Mailhot
 * Date de création : 29 novembre 2018
*/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Hymperia.Facade.EventAggregatorMessages;
using Hymperia.Facade.Extensions;
using Hymperia.Facade.Loaders;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Facade.Properties;
using Hymperia.Facade.Services;
using Hymperia.Model;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Prism;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace Hymperia.Facade.ViewModels.Reglages.Editeur
{
  public class AccesProjetViewModel : ValidatingBase, IActiveAware
  {
    #region Propriete

    [Required(
      ErrorMessageResourceName = nameof(Resources.RequiredUsername),
      ErrorMessageResourceType = typeof(Resources))]
    public string Name
    {
      get => name;
      set => SetProperty(ref name, value);
    }

    public Projet Projet
    {
      get => projet;
      set => SetProperty(ref projet, value, () => AccesLoader.Loading = OnProjetChanged());
    }

    public ObservableCollection<AccesWrapper> Acces
    {
      get => acces;
      private set
      {
        acces?.Remove(OnAccesCollectionChanged);
        value?.Add(OnAccesCollectionChanged);
        SetProperty(ref acces, value);
      }
    }

    private Utilisateur Utilisateur { get; set; }

    #region Commands

    public ICommand AddAcces { get; }

    #endregion

    #region Async Loader

    public AsyncLoader<IEnumerable<AccesWrapper>> AccesLoader { get; } = new AsyncLoader<IEnumerable<AccesWrapper>>();

    #endregion

    #endregion

    #region Constructeur

    public AccesProjetViewModel([NotNull] ContextFactory factory, [NotNull] ConvertisseurAcces acces, [NotNull] IEventAggregator events)
    {
      AddAcces = new DelegateCommand(_AddAcces);

      ContextFactory = factory;
      Convertisseur = acces;

      events.GetEvent<ReglageProjetChanged>().Subscribe(OnProjetChanged);
    }

    #endregion

    #region Query

    private async Task<Acces[]> QueryAcces() =>
      await (from acces in ContextWrapper.Context.Acces.IncludeProjets().IncludeUtilisateurs()
             where acces.Projet == Projet
             select acces).ToArrayAsync().ConfigureAwait(false);


    private async Task<Utilisateur> QueryUtilisateur() =>
      await ContextWrapper.Context.Utilisateurs//.IncludeAcces()
        .Except(from acces in Acces
                select acces.Utilisateur)
        .SingleOrDefaultAsync(u => u.Nom == Name).ConfigureAwait(false);

    #endregion

    #region AddAcces Command

    private async void _AddAcces()
    {
      await ValidateUtilisateur();

      if (HasErrors)
        return;

      Utilisateur.RecevoirProjet(Projet, Model.Modeles.Acces.Droit.Lecture);
      Acces.Add(Convertisseur.Convertir(Utilisateur.Acces.Single(acces => acces.Projet == Projet)));
    }

    #endregion

    #region OnPropertyChanged

    private async Task<IEnumerable<AccesWrapper>> OnProjetChanged() => Acces = Projet is null
      ? null
      : new ObservableCollection<AccesWrapper>(from acces in await QueryAcces()
                                               select Convertisseur.Convertir(acces));

    private void OnAccesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) =>
      e.OldItems?.Cast<AccesWrapper>()?.ForEach(acces => acces.Utilisateur.RetirerProjet(Projet));

    #endregion

    #region Validation

    private bool ValidateUnique()
    {
      if ((from acces in Acces
           select acces.Utilisateur.Nom).Contains(Name))
        Errors.SetErrors(nameof(Name), new ValidationResult[]
        {
          new ValidationResult(Resources.UserExistProjet, new string[] { nameof(Name) })
        });


      RaisePropertyChanged(nameof(Name));
      return !HasErrors;
    }

    private async Task ValidateUtilisateur()
    {
      Errors.ClearErrors(nameof(Name));

      if (!ValidateProperty<string>(nameof(Name)) && !ValidateUnique())
        return;

      var user = await QueryUtilisateur();

      if (user is null)
        Errors.SetErrors(nameof(Name), new ValidationResult[]
        {
          new ValidationResult(Resources.UserNotExist, new string[] { nameof(Name) })
        });

      Utilisateur = user;
      RaiseErrorsChanged(nameof(Name));
    }

    #endregion

    #region Aggregated Event Handlers

    private void OnProjetChanged(Projet projet) => Projet = projet;

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
    private readonly ConvertisseurAcces Convertisseur;

    [NotNull]
    private ContextFactory.IContextWrapper<DatabaseContext> ContextWrapper;

    #endregion

    #region Private Fields

    private string name;
    private Projet projet;
    private ObservableCollection<AccesWrapper> acces;
    private bool isActive;
    private CancellationTokenSource disposeToken;

    #endregion
  }
}
