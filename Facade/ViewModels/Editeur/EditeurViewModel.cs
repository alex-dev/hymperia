using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
      set => QueryProjet(value, UpdateFormes);
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
      SupprimerForme = new DelegateCommand(_SupprimerForme, PeutSupprimerForme).ObservesProperty(() => FormesSelectionnees);
      Sauvegarder = new DelegateCommand(_Sauvegarder, PeutSauvegarder).ObservesProperty(() => Projet);
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

    private async void _Sauvegarder()
    {
      using (var context = ContextFactory.GetContext())
      {
        ConstructEntriesState(context);

        FormesAdded = new Collection<FormeWrapper> { };
        FormesChanged = new Collection<FormeWrapper> { };
        FormesDeleted = new Collection<FormeWrapper> { };

        await context.SaveChangesAsync();
      }
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
        context.Entry(forme.Forme).Reference(f => f.Materiau).IsModified = true;

        foreach (var property in context.Entry(forme.Forme).Properties)
        {
          property.IsModified = true;
        }
      }

      foreach (var forme in deleted)
      {
        context.Remove(forme.Forme);
      }
    }

    private bool PeutSauvegarder() => Projet is Projet;

    #endregion

    #region Queries

    private async Task QueryProjet(Projet _projet, Action onChanged)
    {
      SetProperty(ref projet, null, onChanged, "Projet");

      if (_projet is Projet)
      {
        using (var context = ContextFactory.GetContext())
        {
          context.Attach(_projet);
          await context.Entry(_projet).CollectionFormes().LoadAsync();
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
        FormesAdded = new Collection<FormeWrapper> { };
        FormesChanged = new Collection<FormeWrapper> { };
        FormesDeleted = new Collection<FormeWrapper> { };
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

    #endregion
  }
}
