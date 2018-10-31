using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using Hymperia.Facade.BaseClasses;
using JetBrains.Annotations;

namespace Hymperia.Facade.DependencyObjects
{
  /// <summary>Extension de <see cref="HelixViewport3D"/> pour supporter la sélection.</summary>
  public class SelectionViewport : HelixViewport3D
  {
    #region Dependancy Properties

    /// <seealso cref="SelectedItems"/>
    public static readonly DependencyProperty SelectedItemsProperty =
      DependencyProperty.Register(nameof(SelectedItems), typeof(BulkObservableCollection<MeshElement3D>), typeof(SelectionViewport),
        new PropertyMetadata(new BulkObservableCollection<MeshElement3D>(), OnSelectedItemsChanged));

    #endregion

    #region Properties

    /// <summary>Les formes sélectionnées dans le viewport.</summary>
    [NotNull]
    [ItemNotNull]
    public BulkObservableCollection<MeshElement3D> SelectedItems
    {
      get => (BulkObservableCollection<MeshElement3D>)GetValue(SelectedItemsProperty);
      set => SetValue(SelectedItemsProperty, value);
    }

    #endregion

    #region Constructors

    public SelectionViewport()
    {
      SelectedMaterial.Freeze();
      SelectedItems.CollectionChanged += OnSelectedItemsCollectionChanged;

      InputBindings.Add(new MouseBinding(
        new PointSelectionCommand(Viewport, CreateHandler(true, true)),
        new MouseGesture(MouseAction.LeftClick, ModifierKeys.Alt)));
      InputBindings.Add(new MouseBinding(
        new PointSelectionCommand(Viewport, CreateHandler(false, false)),
        new MouseGesture(MouseAction.LeftClick, ModifierKeys.Control)));
      InputBindings.Add(new MouseBinding(
        new RectangleSelectionCommand(Viewport, CreateHandler(false, true)),
        new MouseGesture(MouseAction.LeftClick, ModifierKeys.Shift)));
    }

    #endregion

    #region Selection Handlers

    /// <summary>Crée un handler autour de <see cref="SelectionHandler(object, VisualsSelectedEventArgs)"/>.</summary>
    /// <param name="single">Si le handler n'accepte qu'une seule forme sélectionnée ou plusieurs.</param>
    /// <param name="clear">Si le handler doit vider <see cref="SelectedItems"/> ou pas.</param>
    [NotNull]
    private EventHandler<VisualsSelectedEventArgs> CreateHandler(bool single, bool clear) => (sender, e) =>
    {
      e = new VisualsSelectedEventArgs(
        e.SelectedVisuals.OfType<MeshElement3D>().Distinct().Except(SelectedItems).ToList<Visual3D>(),
        e.AreSortedByDistanceAscending);

      if (clear)
        SelectedItems.Clear();

      if (e.SelectedVisuals.Count > 0)
      {
        if (single)
          e = e.AreSortedByDistanceAscending
            ? new VisualsSelectedEventArgs(new List<Visual3D> { e.SelectedVisuals.First() }, true)
            : new VisualsSelectedEventArgs(new List<Visual3D> { e.SelectedVisuals.Last() }, false);

        SelectionHandler(sender, e);
      }
    };


    private void SelectionHandler([NotNull] object sender, [NotNull] VisualsSelectedEventArgs args) =>
      SelectedItems.AddRange(args.SelectedVisuals.Cast<MeshElement3D>());

    #endregion

    #region Items Changed

    protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
      base.OnItemsChanged(e);

      if (e.Action == NotifyCollectionChangedAction.Reset)
      {
        Children.Add(Sunlight);
        Children.Add(GridLines);
      }
    }

    #endregion

    #region Selected Items Changed

    #region Update Visual Elements

    private void SelectMaterial([NotNull][ItemNotNull] IEnumerable<MeshElement3D> models)
    {
      foreach (var model in models)
      {
        CachedMaterials[model] = (MaterialGroup)model.Material;

        var material = CachedMaterials[model].Clone();
        material.Children.Add(SelectedMaterial);
        material.Freeze();

        model.Material = material;
      }
    }

    private void UnselectMaterial([NotNull][ItemNotNull] IEnumerable<MeshElement3D> models)
    {
      foreach (var model in models)
        model.Material = CachedMaterials[model];
    }

    #endregion

    protected virtual void OnSelectedItemsCollectionChanged([NotNull] object sender, [NotNull] NotifyCollectionChangedEventArgs e)
    {
      SelectMaterial(e.NewItems?.OfType<MeshElement3D>() ?? Enumerable.Empty<MeshElement3D>());
      UnselectMaterial(e.Action != NotifyCollectionChangedAction.Reset
        ? e.OldItems?.Cast<MeshElement3D>() ?? Enumerable.Empty<MeshElement3D>()
        : from mesh in Children.OfType<MeshElement3D>()
          where (mesh.Material as MaterialGroup)?.Children?.Contains(SelectedMaterial) ?? false
          select mesh);
    }

    protected virtual void OnSelectedItemsChanged(
      [ItemNotNull] ObservableCollection<MeshElement3D> oldvalue,
      [ItemNotNull] ObservableCollection<MeshElement3D> newvalue)
    {
      CachedMaterials.Clear();
      oldvalue?.Remove(OnSelectedItemsCollectionChanged);
      newvalue?.Add(OnSelectedItemsCollectionChanged);
    }

    private static void OnSelectedItemsChanged([NotNull] DependencyObject sender, [NotNull] DependencyPropertyChangedEventArgs e) =>
      ((SelectionViewport)sender).OnSelectedItemsChanged(
        (ObservableCollection<MeshElement3D>)e.OldValue,
        (ObservableCollection<MeshElement3D>)e.NewValue);

    #endregion

    #region Private Fields

    [NotNull]
    private readonly SunLight Sunlight = new SunLight();
    [NotNull]
    private readonly GridLinesVisual3D GridLines =
      new GridLinesVisual3D { Width = 100, Length = 100, MajorDistance = 1, Thickness = 0.01 };
    [NotNull]
    private readonly IDictionary<MeshElement3D, MaterialGroup> CachedMaterials =
      new Dictionary<MeshElement3D, MaterialGroup> { };
    [NotNull]
    private readonly Material SelectedMaterial = new DiffuseMaterial(Brushes.Red.Clone());

    #endregion
  }
}
