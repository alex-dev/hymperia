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
using Hymperia.Facade.ModelWrappers;
using Hymperia.Facade.Services;
using Hymperia.Model;
using Hymperia.Model.Modeles;
using Hymperia.Model.Modeles.JsonObject;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;

namespace Hymperia.Facade.ViewModels.Editeur
{
  public class EditeurViewModel : BindableBase, IProjetViewModel, IEditeurViewModel
  {
    #region Attributes

    #region Binding

    /// <summary>Le projet travaillé par l'éditeur.</summary>
    /// <remarks><see cref="null"/> si le projet est en attente.</remarks>
    [CanBeNull]
    public Projet Projet
    {
      get => projet;
      set => ProjetLoading = QueryProjet(value, UpdateFormes);
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

    public ICommand AjouterForme { get; private set; }
    public ICommand SupprimerForme { get; private set; }
    public ICommand Sauvegarder { get; private set; }
    public ICommand Revert { get; private set; }

    #endregion

    #region Asynchronous Loading

    [CanBeNull]
    private Task ProjetLoading
    {
      get => projetLoading;
      set => SetProperty(ref projetLoading, value, () =>
      {
        IsProjetLoading = true;
        value.ContinueWith(result => IsProjetLoading = false);
      });
    }

    [CanBeNull]
    private Task SaveLoading
    {
      get => saveLoading;
      set => SetProperty(ref saveLoading, value, () =>
      {
        IsSaveLoading = true;
        value.ContinueWith(result => IsSaveLoading = false);
      });
    }

    public bool IsProjetLoading
    {
      get => isProjetLoading;
      private set => SetProperty(ref isProjetLoading, value);
    }

    public bool IsSaveLoading
    {
      get => isSaveLoading;
      private set => SetProperty(ref isSaveLoading, value);
    }

    #endregion

    #endregion

    #region Constructors

    [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public EditeurViewModel([NotNull] ContextFactory factory, [NotNull] ConvertisseurFormes formes)
    {
      ContextFactory = factory;
      ConvertisseurFormes = formes;
      AjouterForme = new DelegateCommand<Point>(_AjouterForme, PeutAjouterForme)
        .ObservesProperty(() => Projet)
        .ObservesProperty(() => SelectedForme)
        .ObservesProperty(() => SelectedMateriau);
      SupprimerForme = new DelegateCommand(_SupprimerForme, PeutSupprimerForme)
        .ObservesProperty(() => FormesSelectionnees);
      Sauvegarder = new DelegateCommand(() => SaveLoading = _Sauvegarder())
        .ObservesCanExecute(() => IsModified);
      Revert = new DelegateCommand(() => ProjetLoading = SaveLoading = _Revert())
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
        throw new NotImplementedException("Seems like something broke. Blame the devs.");
    }

    private void _AjouterForme(Point point)
    {
      var forme = CreerForme(point);
      var wrapper = ConvertisseurFormes.Convertir(forme);

      Projet.AjouterForme(forme);
      FormesAdded.Add(wrapper);
      Formes.Add(wrapper);
    }

    private bool PeutAjouterForme(Point point) =>
      point is Point && Projet is Projet && SelectedMateriau is Materiau && SelectedForme.IsSubclassOf(typeof(Forme));

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
      FormesDeleted = FormesDeleted.Concat(FormesSelectionnees).ToArray();
      FormesSelectionnees.Clear();
    }

    private bool PeutSupprimerForme() => Projet is Projet && FormesSelectionnees.Count > 0;

    #endregion

    #region Command Sauvegarder

    private async Task _Sauvegarder()
    {
      await (SaveLoading ?? Task.FromCanceled(default));

      using (var context = ContextFactory.GetContext())
      {
        ConstructEntriesState(context);
        ResetChangeTracker();

        SaveLoading = context.SaveChangesAsync();
        await SaveLoading;
      }
    }

    #endregion

    #region Command Revert

    private async Task _Revert()
    {
      await (ProjetLoading ?? Task.FromCanceled(default));

      using (var context = ContextFactory.GetContext())
      {
        context.Attach(Projet);
        ProjetLoading = context.Entry(Projet).ReloadAsync();
        await ProjetLoading;
      }
    }

    #endregion

    #region Queries

    private async Task QueryProjet(Projet _projet, Action onChanged)
    {
      SetProperty(ref projet, null, onChanged, "Projet");

      if (_projet is Projet)
      {
        await (ProjetLoading ?? Task.FromCanceled(default));
        await (SaveLoading ?? Task.FromCanceled(default));

        using (var context = ContextFactory.GetContext())
        {
          context.Attach(_projet);
          ProjetLoading = context.Entry(_projet).CollectionFormes().LoadAsync();
          await ProjetLoading;
        }

        SetProperty(ref projet, _projet, onChanged, "Projet");
      }
    }

    #endregion

    #region Formes Changed Event Handlers

    private void UpdateFormes()
    {
      if (Projet is null)
      {
        Formes = null;
        FormesAdded = null;
        FormesChanged = null;
        FormesDeleted = null;
      }
      else
      {
        FormeWrapper Attach(FormeWrapper forme)
        {
          forme.PropertyChanged += FormeHasChanged;
          return forme;
        }

        var enumerable = from forme in Projet.Formes
                         select Attach(ConvertisseurFormes.Convertir(forme));

        Formes = new BulkObservableCollection<FormeWrapper>(enumerable);
        ResetChangeTracker();
      }
    }

    private void FormeHasChanged(object sender, PropertyChangedEventArgs args)
    {
      if (sender is FormeWrapper forme && !FormesChanged.Contains(forme) && Formes.Contains(forme))
      {
        FormesChanged.Add(forme);
      }
    }

    #endregion

    #region Changes Tracking

    [CanBeNull]
    [ItemNotNull]
    private ICollection<FormeWrapper> FormesAdded { get; set; }

    [CanBeNull]
    [ItemNotNull]
    private ICollection<FormeWrapper> FormesChanged { get; set; }

    [CanBeNull]
    [ItemNotNull]
    private ICollection<FormeWrapper> FormesDeleted { get; set; }

    private bool IsModified
    {
      get => isModified;
      set => SetProperty(ref isModified, value);
    }

    private void ResetChangeTracker()
    {
      FormesAdded = new ObservableCollection<FormeWrapper>();
      ((ObservableCollection<FormeWrapper>)FormesAdded).CollectionChanged += HasBeenModified;

      FormesChanged = new ObservableCollection<FormeWrapper>();
      ((ObservableCollection<FormeWrapper>)FormesChanged).CollectionChanged += HasBeenModified;

      FormesDeleted = new ObservableCollection<FormeWrapper>();
      ((ObservableCollection<FormeWrapper>)FormesDeleted).CollectionChanged += HasBeenModified;

      IsModified = false;
    }

    private void ConstructEntriesState(DatabaseContext context)
    {
      var changed = FormesChanged;
      var deleted = FormesDeleted;

      context.Attach(Projet);

      // On charge les changements potentiels dans l'objet pour sauvegarder.
      // Les nouveaux objets (id == default) sont déjà marqué pour ajout.
      // Les objets connus (id != default) doivent être marqué pour changement
      // s'ils ont été modifiés.
      // Les objets supprimés doivent être retirés.
      foreach (var forme in changed)
      {
        context.Entry(forme.Forme).State = EntityState.Modified;
      }

      foreach (var forme in deleted)
      {
        context.Remove(forme.Forme);
      }
    }

    private void HasBeenModified(object sender, NotifyCollectionChangedEventArgs args) => IsModified = true;

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
    private Task saveLoading;
    private Task projetLoading;
    private bool isSaveLoading;
    private bool isProjetLoading;
    private bool isModified;

    #endregion
  }
}
