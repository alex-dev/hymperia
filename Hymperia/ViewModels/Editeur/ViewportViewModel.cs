using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using HelixToolkit.Wpf;
using Hymperia.Facade.BaseClasses;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Facade.Services;
using JetBrains.Annotations;

namespace Hymperia.Facade.ViewModels.Editeur
{
  public class ViewportViewModel : RegionContextAwareViewModel
  {
    #region Properties

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
    [CanBeNull]
    [ItemNotNull]
    public BulkObservableCollection<MeshElement3D> FormesSelectionnees
    {
      get => selected;
      private set
      {
        selected?.Remove(FormesSelectionneesChanged);
        value.CollectionChanged += FormesSelectionneesChanged;
        SetProperty(ref selected, value);
      }
    }

    #endregion

    #region Commands

    public ICommand AjouterForme { get; private set; }
    public ICommand SupprimerForme { get; private set; }

    #endregion

    #endregion

    #region Constructors

    public ViewportViewModel([NotNull] ConvertisseurWrappers wrappers)
    {
      Monitor = new SimpleMonitor();
      ConvertisseurWrappers = wrappers;
      FormesSelectionnees = new BulkObservableCollection<MeshElement3D>();
    }

    #endregion

    #region Region Interactions

    [Obsolete]
    protected override void OnRegionContextChanged()
    {
      if (!(RegionContext is IEditeurViewModel context))
        throw new InvalidCastException($"{ nameof(RegionContext) } is not { nameof(IEditeurViewModel) }.");

      context.PropertyChanged += (sender, args) =>
      {
        if (sender == RegionContext)
        {
          switch (args.PropertyName)
          {
            case nameof(context.Formes):
              UpdateFormes(); break;
            case nameof(context.FormesSelectionnees):
              UpdateFormesSelectionnees(); break;
          }
        }
      };

      UpdateFormes();
      UpdateFormesSelectionnees();
      AjouterForme = context.AjouterForme;
      SupprimerForme = context.SupprimerForme;

      base.OnRegionContextChanged();
    }

    [Obsolete]
    private void UpdateFormes()
    {
      if (!(RegionContext is IEditeurViewModel context))
        throw new InvalidCastException($"{ nameof(RegionContext) } is not { nameof(IEditeurViewModel) }.");

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

    [Obsolete]
    private void UpdateFormesSelectionnees()
    {
      if (!(RegionContext is IEditeurViewModel context))
        throw new InvalidCastException($"{ nameof(RegionContext) } is not { nameof(IEditeurViewModel) }.");

      if (Formes is null && context.FormesSelectionnees.Count > 0)
      {
        throw new InvalidOperationException($"Cannot use an empty { nameof(Formes) } list with a non-empty { nameof(context.FormesSelectionnees) }.");
      }

      if (Formes is null)
      {
        FormesSelectionnees.Clear();
      }
      else
      {
        var enumerable = from FormeWrapper wrapper in context.FormesSelectionnees
                         join MeshElement3D mesh in Formes ?? Enumerable.Empty<MeshElement3D>()
                           on wrapper equals BindingOperations.GetMultiBinding(mesh, MeshElement3D.TransformProperty)?.Bindings?.OfType<Binding>()?.First()?.Source
                         select mesh;

        FormesSelectionnees = new BulkObservableCollection<MeshElement3D>(enumerable);
        context.FormesSelectionnees.CollectionChanged += OnFormesSelectionneesChanged;
      }
    }

    private void OnFormesChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      var newitems = from FormeWrapper forme in (IEnumerable)e.NewItems ?? Enumerable.Empty<FormeWrapper>()
                     select ConvertisseurWrappers.Convertir(forme);
      var olditems = from FormeWrapper wrapper in (IEnumerable)e.OldItems ?? Enumerable.Empty<FormeWrapper>()
                     join MeshElement3D mesh in Formes ?? Enumerable.Empty<MeshElement3D>()
                       on wrapper equals BindingOperations.GetMultiBinding(mesh, MeshElement3D.TransformProperty)?.Bindings?.OfType<Binding>()?.First()?.Source
                     select mesh;

      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          Formes.AddRange(newitems); break;
        case NotifyCollectionChangedAction.Remove:
          Formes.RemoveRange(olditems); break;
        case NotifyCollectionChangedAction.Replace:
          Formes[Formes.IndexOf(olditems.Single())] = newitems.Single(); break;
        case NotifyCollectionChangedAction.Reset:
          Formes.Clear(); break;
      }
    }

    private void OnFormesSelectionneesChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (IsBusy())
        return;

      var newitems = from FormeWrapper wrapper in (IEnumerable)e.NewItems ?? Enumerable.Empty<MeshElement3D>()
                     join MeshElement3D mesh in Formes ?? Enumerable.Empty<MeshElement3D>()
                       on wrapper equals BindingOperations.GetMultiBinding(mesh, MeshElement3D.TransformProperty)?.Bindings?.OfType<Binding>()?.First()?.Source
                     select mesh;
      var olditems = from FormeWrapper wrapper in (IEnumerable)e.OldItems ?? Enumerable.Empty<MeshElement3D>()
                     join MeshElement3D mesh in FormesSelectionnees ?? Enumerable.Empty<MeshElement3D>()
                       on wrapper equals BindingOperations.GetMultiBinding(mesh, MeshElement3D.TransformProperty)?.Bindings?.OfType<Binding>()?.First()?.Source
                     select mesh;

      using (Monitor.Enter())
      {
        switch (e.Action)
        {
          case NotifyCollectionChangedAction.Add:
            FormesSelectionnees.AddRange(newitems); break;
          case NotifyCollectionChangedAction.Remove:
            FormesSelectionnees.RemoveRange(olditems); break;
          case NotifyCollectionChangedAction.Replace:
            FormesSelectionnees[FormesSelectionnees.IndexOf(olditems.Single())] = newitems.Single(); break;
          case NotifyCollectionChangedAction.Reset:
            FormesSelectionnees.Clear(); break;
        }
      }
    }

    private void FormesSelectionneesChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (IsBusy())
        return;

      var formes = ((EditeurViewModel)RegionContext).Formes;
      var selection = ((EditeurViewModel)RegionContext).FormesSelectionnees;
      var newitems = from MeshElement3D mesh in (IEnumerable)e.NewItems ?? Enumerable.Empty<MeshElement3D>()
                     join FormeWrapper wrapper in formes ?? Enumerable.Empty<FormeWrapper>()
                       on BindingOperations.GetMultiBinding(mesh, MeshElement3D.TransformProperty)?.Bindings?.OfType<Binding>()?.First()?.Source equals wrapper
                     select wrapper;
      var olditems = from MeshElement3D mesh in (IEnumerable)e.OldItems ?? Enumerable.Empty<MeshElement3D>()
                     join FormeWrapper wrapper in selection ?? Enumerable.Empty<FormeWrapper>()
                       on BindingOperations.GetMultiBinding(mesh, MeshElement3D.TransformProperty)?.Bindings?.OfType<Binding>()?.First()?.Source equals wrapper
                     select wrapper;

      using (Monitor.Enter())
      {
        switch (e.Action)
        {
          case NotifyCollectionChangedAction.Add:
            selection.AddRange(newitems); break;
          case NotifyCollectionChangedAction.Remove:
            selection.RemoveRange(olditems); break;
          case NotifyCollectionChangedAction.Replace:
            selection[selection.IndexOf(olditems.Single())] = newitems.Single(); break;
          case NotifyCollectionChangedAction.Reset:
            selection.Clear(); break;
        }
      }
    }

    #endregion

    #region Services

    [NotNull]
    private readonly ConvertisseurWrappers ConvertisseurWrappers;

    #endregion

    #region Private Fields

    private BulkObservableCollection<MeshElement3D> formes;
    private BulkObservableCollection<MeshElement3D> selected;

    #endregion

    #region Block Reentrancy

    /// <summary><see cref="true"/> si <paramref name="sender"/> est occupé à répondre à une requête de <see cref="this"/>.</summary>
    [Pure]
    protected bool IsBusy() => Monitor.Busy;
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable",
      Justification = @"Disposable field is only used for blocking reentrancy and doesn't manage any disposable resource.")]
    [NotNull]
    private readonly SimpleMonitor Monitor = new SimpleMonitor();

    private class SimpleMonitor : IDisposable
    {
      public bool Busy { get; private set; }

      [NotNull]
      public SimpleMonitor Enter()
      {
        Busy = true;
        return this;
      }

      [Pure]
      public void Dispose() => Busy = false;
    }

    #endregion
  }
}
