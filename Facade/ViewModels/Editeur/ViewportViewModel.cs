using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Data;
using JetBrains.Annotations;
using HelixToolkit.Wpf;
using Hymperia.Facade.BaseClasses;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Facade.Services;
using Hymperia.Model.Modeles;

namespace Hymperia.Facade.ViewModels.Editeur
{
  public class ViewportViewModel : RegionContextAwareViewModel
  {
    #region Attributes

    #region Fields

    private BulkObservableCollection<MeshElement3D> formes;
    private BulkObservableCollection<MeshElement3D> selected;
    private ICommand ajouter;
    private ICommand supprimer;

    #endregion

    #region Binding

    /// <summary>Les formes affichables.</summary>
    /// <remarks><see cref="null"/> si le projet est en attente.</remarks>
    [CanBeNull]
    [ItemNotNull]
    public BulkObservableCollection<MeshElement3D> Formes
    {
      get => formes;
      private set => SetProperty(ref formes, value);
    }

    /// <summary>Le projet travaillé par l'éditeur.</summary>
    [NotNull]
    [ItemNotNull]
    public BulkObservableCollection<MeshElement3D> FormesSelectionnees
    {
      get => selected;
      private set => SetProperty(ref selected, value);
    }

    #endregion

    #region Commands

    public ICommand AjouterForme
    {
      get => ajouter;
      private set => SetProperty(ref ajouter, value);
    }
    
    public ICommand SupprimerForme
    {
      get => supprimer;
      set => SetProperty(ref supprimer, value);
    }

    #endregion

    #endregion

    #region Services

    [NotNull]
    private readonly ConvertisseurWrappers ConvertisseurWrappers;

    #endregion

    public ViewportViewModel([NotNull] ConvertisseurWrappers wrappers)
    {
      ConvertisseurWrappers = wrappers;
    }

    #region Methods

    #region Region Interactions

    protected override void OnRegionContextChanged()
    {
      if (!(RegionContext is EditeurViewModel context))
      {
        throw new InvalidCastException("RegionContext is not EditeurViewModel.");
      }

      context.PropertyChanged += (sender, args) =>
      {
        switch (args.PropertyName)
        {
          case nameof(context.Formes):
            UpdateFormes();
            break;
          case nameof(context.FormesSelectionnees):
            UpdateFormesSelectionnees();
            break;
        }
      };

      UpdateFormes();
      UpdateFormesSelectionnees();
      AjouterForme = context.AjouterForme;
      SupprimerForme = context.SupprimerForme;

      base.OnRegionContextChanged();
    }

    private void UpdateFormes()
    {
      if (!(RegionContext is EditeurViewModel context))
      {
        throw new InvalidCastException("RegionContext is not EditeurViewModel.");
      }

      if (context.Formes is null)
      {
        Formes = null;
      }
      else
      {
        var enumerable = from forme in context.Formes
                         select ConvertisseurWrappers.ConvertirLier(forme);

        context.Formes.CollectionChanged += OnFormesChanged;
        Formes = new BulkObservableCollection<MeshElement3D>(enumerable);
      }
    }

    private void UpdateFormesSelectionnees()
    {
      if (!(RegionContext is EditeurViewModel context))
      {
        throw new InvalidCastException("RegionContext is not EditeurViewModel.");
      }

      var enumerable = from FormeWrapper<Forme> wrapper in context.FormesSelectionnees
                       join mesh in Formes on wrapper equals BindingOperations.GetBinding(mesh, MeshElement3D.TransformProperty).Source
                       select mesh;

      context.FormesSelectionnees.CollectionChanged += OnFormesSelectionneesChanged;
      FormesSelectionnees = new BulkObservableCollection<MeshElement3D>(enumerable);
    }

    private void OnFormesChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
      if (sender == Formes)
      {
        var newitems = from FormeWrapper<Forme> forme in args.NewItems
                       select ConvertisseurWrappers.ConvertirLier(forme);
        var olditems = from FormeWrapper<Forme> wrapper in args.OldItems
                       join mesh in Formes on wrapper equals BindingOperations.GetBinding(mesh, MeshElement3D.TransformProperty).Source
                       select mesh;

        switch (args.Action)
        {
          case NotifyCollectionChangedAction.Add:
            Formes.AddRange(newitems);
            break;
          case NotifyCollectionChangedAction.Remove:
            Formes.RemoveRange(olditems);
            break;
          case NotifyCollectionChangedAction.Replace:
            Formes[Formes.IndexOf(olditems.Single())] = newitems.Single();
            break;
          case NotifyCollectionChangedAction.Reset:
            Formes.Clear();
            break;
        }
      }
    }

    private void OnFormesSelectionneesChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
      if (sender == FormesSelectionnees)
      {
        var newitems = from FormeWrapper<Forme> forme in args.NewItems
                       select ConvertisseurWrappers.ConvertirLier(forme);
        var olditems = from FormeWrapper<Forme> wrapper in args.OldItems
                       join mesh in Formes on wrapper equals BindingOperations.GetBinding(mesh, MeshElement3D.TransformProperty).Source
                       select mesh;

        switch (args.Action)
        {
          case NotifyCollectionChangedAction.Add:
            FormesSelectionnees.AddRange(newitems);
            break;
          case NotifyCollectionChangedAction.Remove:
            FormesSelectionnees.RemoveRange(olditems);
            break;
          case NotifyCollectionChangedAction.Replace:
            FormesSelectionnees[FormesSelectionnees.IndexOf(olditems.Single())] = newitems.Single();
            break;
          case NotifyCollectionChangedAction.Reset:
            FormesSelectionnees.Clear();
            break;
        }
      }
    }

    #endregion

    #endregion
  }
}
