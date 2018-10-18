using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    public static readonly DependencyProperty SelectedItemsProperty;

    #endregion

    #region Properties

    /// <summary>Les formes sélectionnées dans le viewport.</summary>
    [CanBeNull]
    [ItemNotNull]
    public BulkObservableCollection<MeshElement3D> SelectedItems
    {
      get => (BulkObservableCollection<MeshElement3D>)GetValue(SelectedItemsProperty);
      set => SetValue(SelectedItemsProperty, value);
    }

    #endregion

    #region Constructors

    static SelectionViewport()
    {
      SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(BulkObservableCollection<MeshElement3D>), typeof(SelectionViewport),
        new PropertyMetadata(new BulkObservableCollection<MeshElement3D>(), OnSelectedItemsChanged));
    }

    public SelectionViewport() : base()
    {
      Sunlight = new SunLight();
      GridLines = new GridLinesVisual3D { Width = 100, Length = 100, MajorDistance = 1, Thickness = 0.01 };
      SelectedMaterial = new DiffuseMaterial(Brushes.Red.Clone());
      CachedMaterials = new Dictionary<MeshElement3D, MaterialGroup> { };

      SelectedMaterial.Freeze();

      InputBindings.Add(new MouseBinding(new PointSelectionCommand(Viewport, CreateHandler(true, true)), new MouseGesture(MouseAction.LeftClick, ModifierKeys.Alt)));
      InputBindings.Add(new MouseBinding(new PointSelectionCommand(Viewport, CreateHandler(false, false)), new MouseGesture(MouseAction.LeftClick, ModifierKeys.Control)));
      InputBindings.Add(new MouseBinding(new RectangleSelectionCommand(Viewport, CreateHandler(false, true)), new MouseGesture(MouseAction.LeftClick, ModifierKeys.Shift)));
    }

    #endregion

    #region Selection Handlers

    /// <summary>Crée un handler autour de <see cref="SelectionHandler(object, VisualsSelectedEventArgs)"/>.</summary>
    /// <param name="single">Si le handler n'accepte qu'une seule forme sélectionnée ou plusieurs.</param>
    /// <param name="clear">Si le handler doit vider <see cref="SelectedItems"/> ou pas.</param>
    [NotNull]
    private EventHandler<VisualsSelectedEventArgs> CreateHandler(bool single, bool clear) => (sender, args) =>
    {
      args = new VisualsSelectedEventArgs(
        args.SelectedVisuals.OfType<MeshElement3D>().Distinct().Except(SelectedItems).ToList<Visual3D>(),
        args.AreSortedByDistanceAscending);

      if (clear)
      {
        SelectedItems.Clear();
      }

      if (args.SelectedVisuals.Count > 0)
      {
        if (single)
        {
          args = args.AreSortedByDistanceAscending
            ? new VisualsSelectedEventArgs(new List<Visual3D> { args.SelectedVisuals.First() }, true)
            : new VisualsSelectedEventArgs(new List<Visual3D> { args.SelectedVisuals.Last() }, false);
        }

        SelectionHandler(sender, args);
      }
    };


    private void SelectionHandler([NotNull] object sender, [NotNull] VisualsSelectedEventArgs args) =>
      SelectedItems.AddRange(args.SelectedVisuals.Cast<MeshElement3D>());

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
      {
        model.Material = CachedMaterials[model];
      }
    }

    #endregion

    protected virtual void OnSelectedItemsCollectionChanged([NotNull] object sender, [NotNull] NotifyCollectionChangedEventArgs args)
    {
      SelectMaterial(args.NewItems?.OfType<MeshElement3D>() ?? Enumerable.Empty<MeshElement3D>());
      UnselectMaterial(args.Action != NotifyCollectionChangedAction.Reset
        ? (IEnumerable<MeshElement3D>)args.OldItems ?? Enumerable.Empty<MeshElement3D>()
        : from mesh in Children.OfType<MeshElement3D>()
          where (mesh.Material as MaterialGroup)?.Children?.Contains(SelectedMaterial) ?? false
          select mesh);
    }

    protected override void OnItemsSourceChanged([NotNull][ItemNotNull] IEnumerable oldValue, [NotNull][ItemNotNull] IEnumerable newValue)
    {
      base.OnItemsSourceChanged(oldValue, newValue);
      Children.Add(Sunlight);
      Children.Add(GridLines);
    }

    protected virtual void OnSelectedItemsChanged(
      [ItemNotNull] ObservableCollection<MeshElement3D> oldvalue,
      [NotNull][ItemNotNull] ObservableCollection<MeshElement3D> newvalue)
    {
      CachedMaterials.Clear();

      if (oldvalue is ObservableCollection<MeshElement3D>)
      {
        oldvalue.CollectionChanged -= OnSelectedItemsCollectionChanged;
      }

      newvalue.CollectionChanged += OnSelectedItemsCollectionChanged;
    }

    private static void OnSelectedItemsChanged([NotNull] DependencyObject self, [NotNull] DependencyPropertyChangedEventArgs args)
    {
      if (args.Property == SelectedItemsProperty)
      {
        ((SelectionViewport)self).OnSelectedItemsChanged(
          (ObservableCollection<MeshElement3D>)args.OldValue,
          (ObservableCollection<MeshElement3D>)args.NewValue);
      }
    }

    #endregion

    #region Private Fields

    private readonly SunLight Sunlight;
    private readonly GridLinesVisual3D GridLines;
    private readonly IDictionary<MeshElement3D, MaterialGroup> CachedMaterials;
    private readonly Material SelectedMaterial;

    #endregion
  }
}
