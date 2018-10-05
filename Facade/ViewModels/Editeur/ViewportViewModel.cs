using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using JetBrains.Annotations;
using HelixToolkit.Wpf;
using Hymperia.Facade.BaseClasses;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Facade.Services;
using System.Collections.Generic;

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
      private set
      {
        value.CollectionChanged += FormesSelectionneesChanged;
        SetProperty(ref selected, value);
      }
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
        throw new InvalidCastException($"{ nameof(RegionContext) } is not { nameof(EditeurViewModel) }.");
      }

      context.PropertyChanged += (sender, args) =>
      {
        if (sender == RegionContext)
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
        throw new InvalidCastException($"{ nameof(RegionContext) } is not { nameof(EditeurViewModel) }.");
      }

      if (context.Formes is null)
      {
        Formes = null;
      }
      else
      {
        var enumerable = from forme in context.Formes
                         select ConvertisseurWrappers.Convertir(forme);

        context.Formes.CollectionChanged += OnFormesChanged;
        Formes = new BulkObservableCollection<MeshElement3D>(enumerable);
      }
    }

    private void UpdateFormesSelectionnees()
    {
      if (!(RegionContext is EditeurViewModel context))
      {
        throw new InvalidCastException($"{ nameof(RegionContext) } is not { nameof(EditeurViewModel) }.");
      }

      if (Formes is null && context.FormesSelectionnees.Count > 0)
      {
        throw new InvalidOperationException($"Cannot use an empty { nameof(Formes) } list with a non-empty { nameof(context.FormesSelectionnees) }.");
      }

      if (Formes is null)
      {
        FormesSelectionnees = new BulkObservableCollection<MeshElement3D>();
      }
      else
      {
        var enumerable = from FormeWrapper wrapper in context.FormesSelectionnees
                         join mesh in Formes ?? new BulkObservableCollection<MeshElement3D> { }
                           on wrapper equals BindingOperations.GetMultiBinding(mesh, MeshElement3D.TransformProperty)?.Bindings?.OfType<Binding>()?.First()?.Source
                         select mesh;

        context.FormesSelectionnees.CollectionChanged += OnFormesSelectionneesChanged;
        FormesSelectionnees = new BulkObservableCollection<MeshElement3D>(enumerable);
      }
    }

    private void OnFormesChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
      if (sender == ((EditeurViewModel)RegionContext).Formes)
      {
        var newitems = from FormeWrapper forme in args.NewItems ?? new List<FormeWrapper> { }
                       select ConvertisseurWrappers.Convertir(forme);
        var olditems = from FormeWrapper wrapper in args.OldItems ?? new List<FormeWrapper> { }
                       join mesh in Formes ?? new BulkObservableCollection<MeshElement3D> { }
                         on wrapper equals BindingOperations.GetMultiBinding(mesh, MeshElement3D.TransformProperty)?.Bindings?.OfType<Binding>()?.First()?.Source
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
      if (sender == ((EditeurViewModel)RegionContext).FormesSelectionnees)
      {
        var newitems = from FormeWrapper forme in args.NewItems ?? new List<FormeWrapper> { }
                       select ConvertisseurWrappers.Convertir(forme);
        var olditems = from FormeWrapper wrapper in args.OldItems ?? new List<FormeWrapper> { }
                       join mesh in Formes ?? new BulkObservableCollection<MeshElement3D> { }
                         on wrapper equals BindingOperations.GetMultiBinding(mesh, MeshElement3D.TransformProperty)?.Bindings?.OfType<Binding>()?.First()?.Source
                       select mesh;

        try
        {
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
        catch (InvalidOperationException) { /* Reentrant. Donc, on catch et on oublie. */ }
      }
    }

    private void FormesSelectionneesChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
      if (sender == FormesSelectionnees)
      {
        var newitems = from MeshElement3D mesh in args.NewItems?.OfType<MeshElement3D>() ?? new List<MeshElement3D> { }
                       join wrapper in ((EditeurViewModel)RegionContext).Formes ?? new BulkObservableCollection<FormeWrapper> { }
                         on BindingOperations.GetMultiBinding(mesh, MeshElement3D.TransformProperty)?.Bindings?.OfType<Binding>()?.First()?.Source equals wrapper
                       select wrapper;
        var olditems = from MeshElement3D mesh in args.OldItems?.OfType<MeshElement3D>() ?? new List<MeshElement3D> { }
                       join wrapper in ((EditeurViewModel)RegionContext).FormesSelectionnees ?? new BulkObservableCollection<FormeWrapper> { }
                         on BindingOperations.GetMultiBinding(mesh, MeshElement3D.TransformProperty)?.Bindings?.OfType<Binding>()?.First()?.Source equals wrapper
                       select wrapper;

        switch (args.Action)
        {
          case NotifyCollectionChangedAction.Add:
            ((EditeurViewModel)RegionContext).FormesSelectionnees.AddRange(newitems);
            break;
          case NotifyCollectionChangedAction.Remove:
            ((EditeurViewModel)RegionContext).FormesSelectionnees.RemoveRange(olditems);
            break;
          case NotifyCollectionChangedAction.Replace:
            ((EditeurViewModel)RegionContext).FormesSelectionnees[((EditeurViewModel)RegionContext).FormesSelectionnees.IndexOf(olditems.Single())] = newitems.Single();
            break;
          case NotifyCollectionChangedAction.Reset:
            ((EditeurViewModel)RegionContext).FormesSelectionnees.Clear();
            break;
        }
      }
    }

    #endregion

    #endregion
  }
}
