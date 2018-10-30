using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Data;
using HelixToolkit.Wpf;
using Hymperia.Facade.BaseClasses;
using Hymperia.Facade.CommandAggregatorCommands;
using Hymperia.Facade.EventAggregatorMessages;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Facade.Services;
using JetBrains.Annotations;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace Hymperia.Facade.ViewModels.Editeur
{
  public class ViewportViewModel : BindableBase
  {
    #region Properties

    #region Binding

    /// <summary>Les formes affichables.</summary>
    [CanBeNull]
    public SelectionMode? SelectionMode
    {
      get => mode;
      private set => SetProperty(ref mode, value);
    }

    /// <summary>Les formes affichables.</summary>
    /// <remarks><see cref="null"/> si le projet est en attente.</remarks>
    [CanBeNull]
    [ItemNotNull]
    public BulkObservableCollection<MeshElement3D> Formes
    {
      get => formes;
      private set => SetProperty(ref formes, value, () => FormesSelectionnees.Clear());
    }

    /// <summary>Le projet travaillé par l'éditeur.</summary>
    [CanBeNull]
    [ItemNotNull]
    public BulkObservableCollection<MeshElement3D> FormesSelectionnees { get; } = new BulkObservableCollection<MeshElement3D>();

    #endregion

    #region Commands

    public AddFormeCommand AjouterForme { get; }
    public DeleteFormeCommand SupprimerForme { get; }

    #endregion

    #endregion

    #region Constructors

    public ViewportViewModel([NotNull] ConvertisseurWrappers wrappers, [NotNull] ICommandAggregator commands, [NotNull] IEventAggregator events)
    {
      ConvertisseurWrappers = wrappers;

      AjouterForme = commands.GetCommand<AddFormeCommand>();
      SupprimerForme = commands.GetCommand<DeleteFormeCommand>();
      SelectedChanged = events.GetEvent<SelectedFormesChanged>();
      events.GetEvent<SelectionModeChanged>().Subscribe(OnSelectionModeChanged);
      events.GetEvent<FormesChanged>().Subscribe(OnFormesChanged);
    }

    #endregion

    #region EventHandlers Handler

    private void OnSelectionModeChanged(SelectionMode? mode) => SelectionMode = mode;

    private void OnFormesChanged(NotifyCollectionChangedEventArgs e)
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

    private void OnSelectedChanged(object sender, NotifyCollectionChangedEventArgs e)
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

    private void RaiseSelectedChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (IsBusy())
        return;

      /*var formes = ((EditeurViewModel)RegionContext).Formes;
      var selection = ((EditeurViewModel)RegionContext).FormesSelectionnees;
      var newitems = from MeshElement3D mesh in (IEnumerable)e.NewItems ?? Enumerable.Empty<MeshElement3D>()
                     join FormeWrapper wrapper in formes ?? Enumerable.Empty<FormeWrapper>()
                       on BindingOperations.GetMultiBinding(mesh, MeshElement3D.TransformProperty)?.Bindings?.OfType<Binding>()?.First()?.Source equals wrapper
                     select wrapper;
      var olditems = from MeshElement3D mesh in (IEnumerable)e.OldItems ?? Enumerable.Empty<MeshElement3D>()
                     join FormeWrapper wrapper in selection ?? Enumerable.Empty<FormeWrapper>()
                       on BindingOperations.GetMultiBinding(mesh, MeshElement3D.TransformProperty)?.Bindings?.OfType<Binding>()?.First()?.Source equals wrapper
                     select wrapper;*/


      using (Monitor.Enter())
      {
        SelectedChanged.Publish(e);
      }
    }

    #endregion

    #region Services

    [NotNull]
    private readonly ConvertisseurWrappers ConvertisseurWrappers;
    [NotNull]
    private readonly SelectedFormesChanged SelectedChanged;

    #endregion

    #region Private Fields

    private BulkObservableCollection<MeshElement3D> formes;
    private SelectionMode? mode;

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
