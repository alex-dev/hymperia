using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using JetBrains.Annotations;
using Prism.Commands;
using Prism.Mvvm;
using Hymperia.Model;
using Hymperia.Facade.BaseClasses;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Facade.Services;
using Hymperia.Model.Modeles;
using System.ComponentModel;

namespace Hymperia.Facade.ViewModels.Editeur
{
  public class EditeurViewModel : BindableBase, IProjetViewModel, IEditeurViewModel
  {
    #region Attributes

    #region Private Fields

    private Materiau DefaultMateriau;

    #endregion

    #region Fields

    private Projet projet;
    private BulkObservableCollection<FormeWrapper> formes;
    private BulkObservableCollection<FormeWrapper> selected;
    private SelectionMode selection;
    private Type forme;
    private Materiau materiau;

    #endregion

    #region Private Properties

    [NotNull]
    private Type Forme => SelectedForme ?? typeof(PrismeRectangulaire);

    [CanBeNull]
    private Materiau Materiau => SelectedMateriau ?? DefaultMateriau;

    [CanBeNull]
    [ItemNotNull]
    private ICollection<FormeWrapper> FormesChanged { get; set; }

    [CanBeNull]
    [ItemNotNull]
    private ICollection<FormeWrapper> FormesDeleted { get; set; }

    #endregion

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

    #region Services

    [NotNull]
    private readonly ContextFactory ContextFactory;

    [NotNull]
    private readonly ConvertisseurFormes ConvertisseurFormes;

    #endregion

    public EditeurViewModel([NotNull] ContextFactory factory, [NotNull] ConvertisseurFormes formes)
    {
      ContextFactory = factory;
      ConvertisseurFormes = formes;
      AjouterForme = new DelegateCommand(_AjouterForme, PeutAjouterForme).ObservesProperty(() => Projet)
        .ObservesProperty(() => SelectedForme).ObservesProperty(() => SelectedMateriau);
      SupprimerForme = new DelegateCommand(_SupprimerForme, PeutSupprimerForme).ObservesProperty(() => FormesSelectionnees);
      Sauvegarder = new DelegateCommand(_Sauvegarder, PeutSauvegarder).ObservesProperty(() => Projet);
      FormesSelectionnees = new BulkObservableCollection<FormeWrapper>();
      QueryDefaultMateriau();
    }

    #region Methods

    #region Command AjouterForme

    private Forme CreerForme()
    {
      throw new NotImplementedException();
    }

    private void _AjouterForme()
    {
      var forme = CreerForme();
      Projet.AjouterForme(forme);
      Formes.Add(ConvertisseurFormes.Convertir(forme));
    }

    // TODO: Add Check
    private bool PeutAjouterForme() => Projet is Projet && Materiau is Materiau;

    #endregion

    #region Command SupprimerForme

    private void _SupprimerForme()
    {
      var formes = (from forme in FormesSelectionnees
                    select forme.Forme).ToArray();

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

    #region Inner Events Handler

    private async Task QueryDefaultMateriau()
    {
      using (var context = ContextFactory.GetContext())
      {
        DefaultMateriau = await context.Materiaux.FirstAsync();
      }
    }

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

    private void UpdateFormes()
    {
      if (Projet is null)
      {
        Formes = null;
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

    #endregion
  }
}
