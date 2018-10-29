using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Hymperia.Facade.BaseClasses;
using Hymperia.Facade.EventAggregatorMessages;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Facade.Properties;
using Hymperia.Facade.Services;
using Hymperia.Model;
using Hymperia.Model.Modeles;
using Hymperia.Model.Modeles.JsonObject;
using JetBrains.Annotations;
using MoreLinq;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace Hymperia.Facade.ViewModels.Editeur
{
  public class EditeurViewModel : BindableBase, IProjetViewModel, IEditeurViewModel
  {
    #region Properties

    #region Binding

    /// <summary>Le projet travaillé par l'éditeur.</summary>
    /// <remarks><see cref="null"/> si le projet est en attente.</remarks>
    [CanBeNull]
    public Projet Projet
    {
      get => projet;
      set
      {
        if (Revert.CanExecute(true))
        {
          Revert.Execute(true);
        }

        Loading = QueryProjet(value, UpdateFormes);
      }
    }

    /// <summary>Les formes éditables.</summary>
    /// <remarks><see cref="null"/> si le projet est en attente.</remarks>
    [CanBeNull]
    [ItemNotNull]
    public BulkObservableCollection<FormeWrapper> Formes
    {
      get => formes;
      private set => SetProperty(ref formes, value, () => FormesSelectionnees.Clear());
    }

    /// <summary>Le projet travaillé par l'éditeur.</summary>
    [NotNull]
    [ItemNotNull]
    public BulkObservableCollection<FormeWrapper> FormesSelectionnees
    {
      get => selected;
      private set => SetProperty(ref selected, value);
    }

    public SelectionMode SelectedSelectionMode
    {
      get => selection;
      set => SetProperty(ref selection, value);
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

    public ICommand AjouterForme { get; }
    public ICommand SupprimerForme { get; }
    public ICommand Sauvegarder { get; }
    public ICommand Revert { get; }

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

    [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors",
    Justification = @"The call is known and needed to perform proper initialization.")]
    public EditeurViewModel([NotNull] ContextFactory factory, [NotNull] IEventAggregator aggregator, [NotNull] ConvertisseurFormes formes)
    {
      ContextFactory = factory;
      aggregator.GetEvent<SelectedFormeChanged>().Subscribe(forme => SelectedForme = forme);
      aggregator.GetEvent<SelectedMateriauChanged>().Subscribe(async materiau => SelectedMateriau = await QueryMateriau(materiau));
      ConvertisseurFormes = formes;
      AjouterForme = new DelegateCommand<Point>(_AjouterForme, PeutAjouterForme)
        .ObservesProperty(() => Projet)
        .ObservesProperty(() => SelectedForme)
        .ObservesProperty(() => SelectedMateriau);
      SupprimerForme = new DelegateCommand(_SupprimerForme, PeutSupprimerForme)
        .ObservesProperty(() => FormesSelectionnees);
      Sauvegarder = new DelegateCommand(() => Loading = _Sauvegarder())
        .ObservesCanExecute(() => IsModified);
      Revert = new DelegateCommand<bool?>(leaving => Loading = _Revert(leaving ?? false))
        .ObservesCanExecute(() => IsModified);

      FormesSelectionnees = new BulkObservableCollection<FormeWrapper>();
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
      var wrapper = ConvertisseurFormes.Convertir(forme);

      Projet.AjouterForme(forme);
      Formes.Add(wrapper);
      HasBeenModified(null, null);
    }

    private bool PeutAjouterForme(Point point) =>
      point is Point && Projet is Projet
      && SelectedMateriau is Materiau
      && SelectedForme is Type && SelectedForme.IsSubclassOf(typeof(Forme));

    #endregion

    #region Command SupprimerForme

    private void _SupprimerForme()
    {
      var formes = from forme in FormesSelectionnees
                   select forme.Forme;

      foreach (var forme in formes)
      {
        Projet.SupprimerForme(forme);
      }

      Formes.RemoveRange(FormesSelectionnees);
      FormesDeleted.AddRange(formes);
      FormesSelectionnees.Clear();
    }

    private bool PeutSupprimerForme() => Projet is Projet && FormesSelectionnees.Count > 0;

    #endregion

    #region Command Sauvegarder

    private async Task _Sauvegarder()
    {
      var projet = Projet;
      var deleted = FormesDeleted;

      await (Loading ?? Task.CompletedTask);

      using (var context = ContextFactory.GetContext())
      {
        Populate(context, projet, deleted);
        ResetChangeTracker(projet, deleted);

        await context.SaveChangesAsync();
      }
    }

    #endregion

    #region Command Revert

    private async Task _Revert(bool leaving)
    {
      var projet = Projet;
      var deleted = FormesDeleted;

      await (Loading ?? Task.CompletedTask);

      using (var context = ContextFactory.GetContext())
      {
        context.UnloadFormes(projet);

        if (!leaving)
        {
          context.LoadFormes(projet);
        }
      }
    }

    #endregion

    #region Queries

    private async Task QueryProjet(Projet _projet, Action onChanged)
    {
      SetProperty(ref projet, null, onChanged, nameof(Projet));

      if (_projet is Projet)
      {
        await (Loading ?? Task.CompletedTask);

        using (var context = ContextFactory.GetContext())
        {
          context.Attach(_projet);

          await context.LoadFormesAsync(_projet);
        }

        SetProperty(ref projet, _projet, onChanged, nameof(Projet));
      }
    }

    public async Task<Materiau> QueryMateriau(int key)
    {
      await (Loading ?? Task.CompletedTask);

      using (var context = ContextFactory.GetContext())
      {
        return await context.Materiaux.FindAsync(key);
      }
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
    }

    private void FormeHasChanged(object sender, PropertyChangedEventArgs e)
    {
      HasBeenModified(null, null);
    }

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

    private void HasBeenModified(object sender, NotifyCollectionChangedEventArgs e) => IsModified = true;

    #endregion

    #region Services

    [NotNull]
    private readonly ContextFactory ContextFactory;

    [NotNull]
    private readonly ConvertisseurFormes ConvertisseurFormes;

    #endregion

    #region Private Fields

    private Projet projet;
    private BulkObservableCollection<FormeWrapper> formes;
    private BulkObservableCollection<FormeWrapper> selected;
    private SelectionMode selection;
    private Type forme;
    private Materiau materiau;
    private Task loading;
    private bool isLoading;
    private bool isModified;

    #endregion
  }
}
