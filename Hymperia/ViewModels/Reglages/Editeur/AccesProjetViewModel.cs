/*
 * Auteur : Antoine Mailhot
 * Date de création : 29 novembre 2018
*/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hymperia.Facade.CommandAggregatorCommands;
using Hymperia.Facade.EventAggregatorMessages;
using Hymperia.Facade.Extensions;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Facade.Services;
using Hymperia.Model;
using Hymperia.Model.Modeles;
using JetBrains.Annotations;
using MoreLinq;
using Prism;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace Hymperia.Facade.ViewModels.Reglages.Editeur
{
  class AccesProjetViewModel : BindableBase, IActiveAware
  {
    #region Propriete

    public Projet Projet
    {
      get => projet;
      set => SetProperty(ref projet, value, OnProjetChanged);
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

    #endregion

    #region Constructeur

    public AccesProjetViewModel([NotNull] ContextFactory factory, [NotNull] ConvertisseurAcces acces, [NotNull] ICommandAggregator commands, [NotNull] IEventAggregator events)
    {
      ContextFactory = factory;
      Convertisseur = acces;

      commands.GetCommand<PreSauvegarderReglageEditeur>().RegisterCommand(new DelegateCommand(PreSauvegarder));
      events.GetEvent<ReglageProjetChanged>().Subscribe(OnProjetChanged);
    }

    #endregion

    #region CommandAcces

    #region AjouterAcces

    private void _AjouterAcces(string utilisateur)
    {
      //if()

    }

    #endregion

    #region ModifierAcces

    #endregion

    #region SupprimerAcces

    #endregion

    #endregion

    #region Sauvegarder

    private void PreSauvegarder()
    {

    }

    #endregion

    #region Query

    private IEnumerable<Acces> QueryAcces() =>
      from acces in ContextWrapper.Context.Acces.IncludeProjets().IncludeUtilisateurs()
      where acces.Projet == Projet
      select acces;


    private Utilisateur QueryUtilisateur(string nom) =>
      ContextWrapper.Context.Utilisateurs
        .Except(from acces in Acces
                select acces.Utilisateur)
        .SingleOrDefault(u => u.Nom == nom);

    #endregion

    #region OnPropertyChanged

    private void OnProjetChanged() => Acces = Projet is null
      ? null
      : new ObservableCollection<AccesWrapper>(from acces in QueryAcces()
                                               select Convertisseur.Convertir(acces));

    private void OnAccesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      e.OldItems?.Cast<AccesWrapper>()?.ForEach(acces => acces.Utilisateur.RetirerProjet(Projet));

      //e.NewItems?.Cast<FormeWrapper>()?.ForEach(forme => forme.PropertyChanged += OnFormeChanged);
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

    private Projet projet;
    private ObservableCollection<AccesWrapper> acces;
    private bool isActive;
    private CancellationTokenSource disposeToken;

    #endregion
  }
}
