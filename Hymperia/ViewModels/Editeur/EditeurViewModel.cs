using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Hymperia.Facade.BaseClasses;
using Hymperia.Facade.CommandAggregatorCommands;
using Hymperia.Facade.EventAggregatorMessages;
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
  public class EditeurViewModel : BindableBase, IActiveAware, IDisposable
  {
    #region Properties

    #region Binding

    /// <summary>Le projet travaillé par l'éditeur.</summary>
    /// <remarks><see cref="null"/> si le projet est en attente.</remarks>
    [CanBeNull]
    public Projet Projet
    {
      get => projet;
      set => ProjetLoader.Loading = QueryProjet(value, UpdateFormes);
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
        var old = formes ?? new Collection<FormeWrapper>();

        formes?.Remove(RaiseFormesChanged);
        value?.Add(RaiseFormesChanged);
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

    #endregion

    #region Commands

    public AddFormeCommand AjouterForme { get; }
    public DeleteFormesCommand SupprimerFormes { get; }
    public ICommand Sauvegarder { get; }

    #endregion

    #region Asynchronous Loading

    public AsyncLoader<Projet> ProjetLoader { get; } = new AsyncLoader<Projet>();
    public AsyncLoader<Materiau> MateriauLoader { get; } = new AsyncLoader<Materiau>();
    public AsyncLoader SaveLoader { get; } = new AsyncLoader();

    private Task Loaders => Task.WhenAll(
      ProjetLoader.Loading ?? Task.CompletedTask,
      MateriauLoader.Loading ?? Task.CompletedTask,
      SaveLoader.Loading ?? Task.CompletedTask);

    #endregion

    #endregion

    #region Constructors

    private readonly ProjetChanged ProjetChanged;

    public EditeurViewModel([NotNull] ContextFactory factory, [NotNull] ConvertisseurFormes formes, [NotNull] ICommandAggregator commands, [NotNull] IEventAggregator events)
    {
      ContextFactory = factory;
      ConvertisseurFormes = formes;

      AjouterForme = (AddFormeCommand)new AddFormeCommand(_AjouterForme, PeutAjouterForme)
        .ObservesProperty(() => Projet)
        .ObservesProperty(() => SelectedForme)
        .ObservesProperty(() => SelectedMateriau);
      SupprimerFormes = new DeleteFormesCommand(_SupprimerFormes, CanSupprimerFormes);
      Sauvegarder = new DelegateCommand(() => SaveLoader.Loading = _Sauvegarder())
        .ObservesCanExecute(() => IsModified);

      commands.RegisterCommand(AjouterForme);
      commands.RegisterCommand(SupprimerFormes);

      events.GetEvent<SelectedFormeChanged>().Subscribe(forme => SelectedForme = forme);
      events.GetEvent<SelectedMateriauChanged>().Subscribe(async materiau => SelectedMateriau = await QueryMateriau(materiau));
      ProjetChanged = events.GetEvent<ProjetChanged>();
      FormesChanged = events.GetEvent<FormesChanged>();
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
      point is Point && Projet is Projet
      && SelectedMateriau is Materiau
      && ((SelectedForme as Type)?.IsSubclassOf(typeof(Forme)) ?? false);

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

    private bool CanSupprimerFormes(ICollection<FormeWrapper> wrappers) => wrappers.Any();

    #endregion

    #region Command Sauvegarder

    private async Task _Sauvegarder()
    {
      await Loaders;
      await Context.SaveChangesAsync();
    }

    #endregion

    #region Queries

    private async Task<Projet> QueryProjet(Projet _projet, Action onChanged)
    {
      Projet value = null;

      SetProperty(ref projet, null, onChanged, nameof(Projet));

      if (_projet is Projet)
      {
        await Loaders;
        value = await Context.Projets.IncludeFormes().FindByIdAsync(_projet.Id);

        SetProperty(ref projet, value, onChanged, nameof(Projet));
      }

      return value;
    }

    private async Task<Materiau> QueryMateriau(int key)
    {
      await Loaders;
      return SelectedMateriau = await Context.Materiaux.FindAsync(key);
    }

    #endregion

    #region Formes Changed Event Handlers

    private void UpdateFormes()
    {
      if (Projet is null)
      {
        Formes.ForEach(wrapper => wrapper.PropertyChanged -= FormeHasChanged);
        Formes = null;
        ResetChangeTracker();
      }
      else
      {
        var enumerable = (from forme in Projet.Formes
                          select ConvertisseurFormes.Convertir(forme))
          .DeferredForEach(wrapper => wrapper.PropertyChanged += FormeHasChanged);

        Formes = new BulkObservableCollection<FormeWrapper>(enumerable);
        ResetChangeTracker();
      }

      ProjetChanged.Publish(Projet);
    }

    private void FormeHasChanged(object sender, PropertyChangedEventArgs e) => HasBeenModified(null, null);

    #endregion

    #region Changes Tracking

    [CanBeNull]
    [ItemNotNull]
    private BulkObservableCollection<Forme> FormesDeleted { get; set; }

    private bool IsModified
    {
      get => isModified;
      set => SetProperty(ref isModified, value);
    }

    private void Populate(DatabaseContext context, Projet projet, ICollection<Forme> deleted)
    {
      var materiaux = (from forme in Projet.Formes
                       select forme.Materiau).DistinctBy(materiau => materiau.Id);

      context.AttachRange(materiaux);
      context.Update(Projet);
      context.RemoveRange(FormesDeleted);
    }

    private void ResetChangeTracker()
    {
      FormesDeleted?.Remove(HasBeenModified);
      FormesDeleted = new BulkObservableCollection<Forme>();
      FormesDeleted.CollectionChanged += HasBeenModified;

      IsModified = false;
    }

    private void ResetChangeTracker(Projet projet, ICollection<Forme> deleted)
    {
      if (Projet == projet && FormesDeleted == deleted)
      {
        ResetChangeTracker();
      }
    }

    private void HasBeenModified(object sender, NotifyCollectionChangedEventArgs e)
    {
      IsModified = true;
      ProjetChanged.Publish(Projet);
    }

    #endregion

    #region Aggregated Event Handlers

    private void RaiseFormesChanged(Collection<FormeWrapper> old) =>
      RaiseFormesChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, Formes, old));

    protected virtual void RaiseFormesChanged(object sender, NotifyCollectionChangedEventArgs e) => FormesChanged.Publish(e);

    protected virtual void RaiseSelectionModeChanged() => SelectionModeChanged.Publish(SelectedSelectionMode);

    #endregion

    #region IActiveAware

    public event EventHandler IsActiveChanged;

    public bool IsActive
    {
      get => isActive;
      set
      {
        isActive = value;

        if (value)
          OnActivation();
        else
          OnDeactivation();

        IsActiveChanged?.Invoke(this, EventArgs.Empty);
      }
    }

    protected virtual void OnActivation()
    {
      Context?.Dispose();
      Context = ContextFactory.GetContext();
    }

    protected virtual void OnDeactivation()
    {
      Context?.Dispose();
      Context = null;
    }

    #endregion

    #region IDisposable

    public void Dispose() => Context?.Dispose();

    #endregion

    #region Services

    [NotNull]
    private readonly ContextFactory ContextFactory;
    [NotNull]
    private readonly ConvertisseurFormes ConvertisseurFormes;
    [NotNull]
    private readonly FormesChanged FormesChanged;
    [NotNull]
    private readonly SelectionModeChanged SelectionModeChanged;

    [NotNull]
    private DatabaseContext Context;

    #endregion

    #region Private Fields

    private Projet projet;
    private BulkObservableCollection<FormeWrapper> formes;
    private readonly BulkObservableCollection<FormeWrapper> selected;
    private SelectionMode selection;
    private Type forme;
    private Materiau materiau;
    private readonly Task loading;
    private readonly bool isLoading;
    private bool isModified;
    private bool isActive;

    #endregion
  }
}
