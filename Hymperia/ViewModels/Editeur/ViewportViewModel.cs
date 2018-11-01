using System;
using System.Collections;
using System.Collections.Generic;
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
  [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable",
    Justification = @"Disposable field is only used for blocking reentrancy and doesn't manage any disposable resource.")]
  public class ViewportViewModel : BindableBase
  {
    #region Properties

    #region Binding

    /// <summary>Les formes affichables.</summary>
    [NotNull]
    public SelectionMode? SelectionMode
    {
      get => mode;
      private set => SetProperty(ref mode, value);
    }

    /// <summary>Les formes affichables.</summary>
    /// <remarks><see cref="null"/> si le projet est en attente.</remarks>
    [CanBeNull]
    [ItemNotNull]
    public BulkObservableCollection<MeshElement3D> Formes { get; } =
      new BulkObservableCollection<MeshElement3D>();

    /// <summary>Le projet travaillé par l'éditeur.</summary>
    [CanBeNull]
    [ItemNotNull]
    public BulkObservableCollection<MeshElement3D> FormesSelectionnees { get; } =
      new BulkObservableCollection<MeshElement3D>();

    #endregion

    #region Commands

    public AddFormeCommand AjouterForme { get; }
    public DelegateCommand<ICollection<MeshElement3D>> SupprimerFormes { get; }
    private DeleteFormesCommand InnerSupprimerFormes { get; }

    #endregion

    #endregion

    #region Constructors

    public ViewportViewModel([NotNull] NotifyCollectionChangedCopyFactory copy, [NotNull] ConvertisseurWrappers wrappers, [NotNull] ICommandAggregator commands, [NotNull] IEventAggregator events)
    {
      NotifyCollectionChangedCopyFactory = copy;
      ConvertisseurWrappers = wrappers;

      AjouterForme = commands.GetCommand<AddFormeCommand>();
      SupprimerFormes = new DelegateCommand<ICollection<MeshElement3D>>(_SupprimerFormes, CanSupprimerFormes);
      InnerSupprimerFormes = commands.GetCommand<DeleteFormesCommand>();

      SelectedChanged = events.GetEvent<SelectedFormesChanged>();
      SelectedChanged.Subscribe(OnSelectedChanged, FilterSelectedChanged);
      events.GetEvent<SelectionModeChanged>().Subscribe(OnSelectionModeChanged);
      events.GetEvent<FormesChanged>().Subscribe(OnFormesChanged);

      Formes.CollectionChanged += OnFormesChanged;
      FormesSelectionnees.CollectionChanged += OnFormesSelectionneesChanged;
    }

    #endregion

    #region Command SupprimerForme

    private void _SupprimerFormes(ICollection<MeshElement3D> meshes)
    {
      var formes = (from mesh in meshes
                    let wrapper = (FormeWrapper)BindingOperations.GetMultiBinding(mesh, MeshElement3D.TransformProperty)
                      ?.Bindings?.OfType<Binding>()?.First()?.Source
                    where wrapper is FormeWrapper
                    select wrapper).ToArray();

      if (InnerSupprimerFormes.CanExecute(formes))
        InnerSupprimerFormes.Execute(formes);
    }

    private bool CanSupprimerFormes(ICollection<MeshElement3D> meshes) => meshes.Any();

    #endregion

    #region Inner Event Handlers

    protected virtual void OnFormesChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.Action == NotifyCollectionChangedAction.Reset)
        FormesSelectionnees.Clear();
      else if (e.OldItems is IEnumerable old)
        FormesSelectionnees.RemoveRange(old.Cast<MeshElement3D>());
    }

    protected virtual void OnFormesSelectionneesChanged(object sender, NotifyCollectionChangedEventArgs e) =>
      RaiseSelectedChanged(NotifyCollectionChangedCopyFactory.Copy<MeshElement3D, FormeWrapper>(e, ConvertisseurWrappers.Convertir));

    #endregion

    #region Aggregated Event Handlers

    protected virtual void OnSelectionModeChanged(SelectionMode? mode) => SelectionMode = mode;

    private void OnFormesChanged(NotifyCollectionChangedEventArgs e)
    {
      var newitems = from FormeWrapper forme in (IEnumerable)e.NewItems ?? Enumerable.Empty<FormeWrapper>()
                     select ConvertisseurWrappers.Convertir(forme);
      var olditems = from FormeWrapper wrapper in (IEnumerable)e.OldItems ?? Enumerable.Empty<FormeWrapper>()
                     join MeshElement3D mesh in Formes ?? Enumerable.Empty<MeshElement3D>()
                       on wrapper equals ConvertisseurWrappers.Convertir(mesh)
                     select mesh;

      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          Formes.AddRange(newitems); break;
        case NotifyCollectionChangedAction.Remove:
          Formes.RemoveRange(olditems); break;
        case NotifyCollectionChangedAction.Replace:
          Formes.ReplaceRange(olditems, newitems); break;
        case NotifyCollectionChangedAction.Reset:
          Formes.Clear(); break;
      }
    }

    protected virtual void OnSelectedChanged(NotifyCollectionChangedEventArgs e)
    {
      if (IsBusy())
        return;

      var newitems = from FormeWrapper wrapper in (IEnumerable)e.NewItems ?? Enumerable.Empty<MeshElement3D>()
                     join MeshElement3D mesh in Formes ?? Enumerable.Empty<MeshElement3D>()
                       on wrapper equals ConvertisseurWrappers.Convertir(mesh)
                     select mesh;
      var olditems = from FormeWrapper wrapper in (IEnumerable)e.OldItems ?? Enumerable.Empty<MeshElement3D>()
                     join MeshElement3D mesh in FormesSelectionnees ?? Enumerable.Empty<MeshElement3D>()
                       on wrapper equals ConvertisseurWrappers.Convertir(mesh)
                     select mesh;

      using (Monitor.Enter())
        switch (e.Action)
        {
          case NotifyCollectionChangedAction.Add:
            FormesSelectionnees.AddRange(newitems); break;
          case NotifyCollectionChangedAction.Remove:
            FormesSelectionnees.RemoveRange(olditems); break;
          case NotifyCollectionChangedAction.Replace:
            FormesSelectionnees.ReplaceRange(olditems, newitems); break;
          case NotifyCollectionChangedAction.Reset:
            FormesSelectionnees.Clear(); break;
        }
    }

    protected virtual bool FilterSelectedChanged(NotifyCollectionChangedEventArgs e) => IsBusy();

    protected virtual void RaiseSelectedChanged(NotifyCollectionChangedEventArgs args)
    {
      if (IsBusy())
        return;

      using (Monitor.Enter())
        SelectedChanged.Publish(args);
    }

    #endregion

    #region Services

    [NotNull]
    private readonly NotifyCollectionChangedCopyFactory NotifyCollectionChangedCopyFactory;
    [NotNull]
    private readonly ConvertisseurWrappers ConvertisseurWrappers;
    [NotNull]
    private readonly SelectedFormesChanged SelectedChanged;

    #endregion

    #region Private Fields

    private SelectionMode? mode;

    #endregion

    #region Block Reentrancy

    /// <summary><see cref="true"/> si <paramref name="sender"/> est occupé à répondre à une requête de <see cref="this"/>.</summary>
    [Pure]
    protected bool IsBusy() => Monitor.Busy;

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
