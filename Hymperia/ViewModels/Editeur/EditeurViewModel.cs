using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Hymperia.Facade.Collections;
using Hymperia.Facade.CommandAggregatorCommands;
using Hymperia.Facade.EventAggregatorMessages;
using Hymperia.Facade.Extensions;
using Hymperia.Facade.Loaders;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Facade.Properties;
using Hymperia.Facade.Services;
using Hymperia.Model;
using Hymperia.Model.Modeles;
using Hymperia.Model.Modeles.JsonObject;
using JetBrains.Annotations;
using MoreLinq;
using Prism;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace Hymperia.Facade.ViewModels.Editeur
{
  public sealed class EditeurViewModel : BindableBase, IActiveAware, IDisposable
  {
    #region Properties

    #region Binding

    ///<summary>Le <see cref="Acces.Droit"/> de l'utilisateur sur <see cref="Projet"/>.</summary>
    public Acces.Droit Droit
    {
      get => droit;
      set => SetProperty(ref droit, value, RaiseAccesChanged);
    }

    public bool CanModify => Droit >= Acces.Droit.LectureEcriture;
    public bool Own => Droit >= Acces.Droit.Possession;

    /// <summary>Le projet travaillé par l'éditeur.</summary>
    /// <remarks><see cref="null"/> si le projet est en attente.</remarks>
    [CanBeNull]
    public Projet Projet
    {
      get => projet;
      set => ProjetLoader.Loading = QueryProjet(value, OnProjetChanged);
    }

    /// <summary>Les formes éditables.</summary>
    /// <remarks><see cref="null"/> si le projet est en attente.</remarks>
    [CanBeNull]
    [ItemNotNull]
    public BulkObservableCollection<FormeWrapper> Formes
    {
      get => formes;
      private set
      {
        var old = formes;

        old?.ForEach(wrapper => wrapper.PropertyChanged -= OnFormeChanged);
        old?.Remove(OnFormesCollectionChanged);

        value?.ForEach(wrapper => wrapper.PropertyChanged += OnFormeChanged);
        value?.Add(OnFormesCollectionChanged);

        SetProperty(ref formes, value, () => RaiseFormesChanged(old));
      }
    }

    public SelectionMode SelectedSelectionMode
    {
      get => selection;
      set => SetProperty(ref selection, value, RaiseSelectionModeChanged);
    }

    [CanBeNull]
    public Type SelectedForme
    {
      get => forme;
      set => SetProperty(ref forme, value);
    }

    [CanBeNull]
    public Materiau SelectedMateriau
    {
      get => materiau;
      set => SetProperty(ref materiau, value);
    }

    public bool HasChanged
    {
      get => changed;
      set => SetProperty(ref changed, value);
    }

    #endregion

    #region Commands

    public DelegateCommand<Point> AjouterForme { get; }
    public DelegateCommand<ICollection<FormeWrapper>> SupprimerFormes { get; }
    public ICommand Sauvegarder { get; }

    #endregion

    #region Asynchronous Loading

    public AsyncLoader<Projet> ProjetLoader { get; } = new AsyncLoader<Projet>();
    public AsyncLoader<Materiau> MateriauLoader { get; } = new AsyncLoader<Materiau>();
    public AsyncLoader SaveLoader { get; } = new AsyncLoader();

    #endregion

    #endregion

    #region Constructors

    public EditeurViewModel([NotNull] ContextFactory factory, [NotNull] ConvertisseurFormes formes, [NotNull] ICommandAggregator commands, [NotNull] IEventAggregator events)
    {
      AjouterForme = new DelegateCommand<Point>(_AjouterForme, PeutAjouterForme)
        .ObservesProperty(() => Projet)
        .ObservesProperty(() => SelectedForme)
        .ObservesProperty(() => SelectedMateriau);
      SupprimerFormes = new DelegateCommand<ICollection<FormeWrapper>>(_SupprimerFormes, CanSupprimerFormes);
      Sauvegarder = new DelegateCommand(() => SaveLoader.Loading = _Sauvegarder())
        .ObservesCanExecute(() => HasChanged);

      ContextFactory = factory;
      ConvertisseurFormes = formes;

      commands.GetCommand<AddFormeCommand>().RegisterCommand(AjouterForme);
      commands.GetCommand<DeleteFormesCommand>().RegisterCommand(SupprimerFormes);

      events.GetEvent<SelectedFormeChanged>().Subscribe(forme => SelectedForme = forme);
      events.GetEvent<SelectedMateriauChanged>().Subscribe(materiau => SelectedMateriau = materiau);
      AccesChanged = events.GetEvent<AccesChanged>();
      FormesChanged = events.GetEvent<FormesChanged>();
      ProjetChanged = events.GetEvent<ProjetChanged>();
      SelectionModeChanged = events.GetEvent<SelectionModeChanged>();
    }

    #endregion

    #region Command AjouterForme

    private Forme CreerForme(Point point)
    {
      if (SelectedForme == typeof(PrismeRectangulaire))
        return new PrismeRectangulaire(SelectedMateriau) { Origine = point };
      else if (SelectedForme == typeof(Ellipsoide))
        return new Ellipsoide(SelectedMateriau) { Origine = point };
      else if (SelectedForme == typeof(Cone))
        return new Cone(SelectedMateriau) { Origine = point };
      else if (SelectedForme == typeof(Cylindre))
        return new Cylindre(SelectedMateriau) { Origine = point };
      else
        throw new InvalidOperationException(Resources.ImpossibleInstantiation(SelectedForme));
    }

    private void _AjouterForme(Point point)
    {
      var forme = CreerForme(point);

      Projet.AjouterForme(forme);
      Formes.Add(ConvertisseurFormes.Convertir(forme));
    }

    private bool PeutAjouterForme(Point point) =>
      Projet is Projet && Droit >= Acces.Droit.LectureEcriture
        && point is Point && SelectedMateriau is Materiau && ((SelectedForme as Type)?.IsSubclassOf(typeof(Forme)) ?? false);

    #endregion

    #region Command SupprimerForme

    private void _SupprimerFormes(ICollection<FormeWrapper> wrappers)
    {
      var formes = from forme in wrappers
                   select forme.Forme;

      foreach (var forme in formes)
        Projet.SupprimerForme(forme);

      Formes.RemoveRange(wrappers);
    }

    private bool CanSupprimerFormes(ICollection<FormeWrapper> wrappers) =>
      Projet is Projet && Droit >= Acces.Droit.LectureEcriture && wrappers.Any();

    #endregion

    #region Command Sauvegarder

    private async Task _Sauvegarder()
    {
      HasChanged = false;

      try
      {
        using (await AsyncLock.Lock(ContextWrapper.Context))
          await ContextWrapper.Context.SaveChangesAsync();
      }
      catch (Exception e)
      {
        HasChanged = true;
        throw e;
      }
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
          value = await ContextWrapper.Context.Projets.IncludeFormes().FindByIdAsync(_projet.Id);

        SetProperty(ref projet, value, onChanged, nameof(Projet));
      }

      return value;
    }

    #endregion

    #region FormesChanged Event Handlers

    private void OnProjetChanged()
    {
      Formes = Projet is null
        ? null
        : new BulkObservableCollection<FormeWrapper>(from forme in Projet.Formes
                                                     select ConvertisseurFormes.Convertir(forme));
      HasChanged = false;
      RaiseProjetChanged();
    }

    private void OnFormesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.Action == NotifyCollectionChangedAction.Reset)
        throw new InvalidOperationException("Detected invalid reset.");

      e.OldItems?.Cast<FormeWrapper>()?.ForEach(forme => forme.PropertyChanged -= OnFormeChanged);
      e.NewItems?.Cast<FormeWrapper>()?.ForEach(forme => forme.PropertyChanged += OnFormeChanged);

      RaiseFormesChanged(sender, e);
      FormesHasChanged();
    }

    void OnFormeChanged(object sender, PropertyChangedEventArgs e) => FormesHasChanged();

    private void FormesHasChanged()
    {
      HasChanged = true;
      RaiseProjetChanged();
    }

    #endregion

    #region Aggregated Event Handlers

    private void RaiseAccesChanged()
    {
      RaisePropertyChanged(nameof(CanModify));
      RaisePropertyChanged(nameof(Own));
      AccesChanged.Publish(Droit);
    }

    private void RaiseFormesChanged(Collection<FormeWrapper> old)
    {
      NotifyCollectionChangedEventArgs args;
      bool oldbool;
      bool newbool;

      if ((oldbool = old is null) & (newbool = Formes is null))
        throw new InvalidOperationException();

      if (!oldbool)
        args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, old);
      else if (!newbool)
        args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, Formes);
      else
        args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, Formes, old);

      RaiseFormesChanged(this, args);
    }

    private void RaiseFormesChanged(object sender, NotifyCollectionChangedEventArgs e) => FormesChanged.Publish(e);

    private void RaiseProjetChanged() => ProjetChanged.Publish(Projet);

    private void RaiseSelectionModeChanged() => SelectionModeChanged.Publish(SelectedSelectionMode);

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
    private readonly ConvertisseurFormes ConvertisseurFormes;
    [NotNull]
    private readonly AccesChanged AccesChanged;
    [NotNull]
    private readonly FormesChanged FormesChanged;
    [NotNull]
    private readonly ProjetChanged ProjetChanged;
    [NotNull]
    private readonly SelectionModeChanged SelectionModeChanged;

    [NotNull]
    private ContextFactory.IContextWrapper<DatabaseContext> ContextWrapper;

    #endregion

    #region Private Fields

    private Acces.Droit droit;
    private Projet projet;
    private BulkObservableCollection<FormeWrapper> formes;
    private SelectionMode selection;
    private Type forme;
    private Materiau materiau;
    private bool changed;
    private bool isActive;
    private CancellationTokenSource disposeToken;

    #endregion
  }
}
