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
using Hymperia.Facade.DependencyObjects.Manipulators;
using JetBrains.Annotations;

namespace Hymperia.Facade.DependencyObjects
{
  public class Viewport : HelixViewport3D
  {
    #region Dependancy Properties

    public static readonly DependencyProperty SelectedItemsProperty;
    public static readonly DependencyProperty SelectionModeProperty;

    #endregion

    #region Properties

    [CanBeNull]
    [ItemNotNull]
    public BulkObservableCollection<MeshElement3D> SelectedItems
    {
      get => (BulkObservableCollection<MeshElement3D>)GetValue(SelectedItemsProperty);
      set => SetValue(SelectedItemsProperty, value);
    }

    public SelectionMode SelectionMode
    {
      get => (SelectionMode)GetValue(SelectionModeProperty);
      set => SetValue(SelectionModeProperty, value);
    }

    #endregion

    #region Constructors

    static Viewport()
    {
      SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(BulkObservableCollection<MeshElement3D>), typeof(Viewport),
        new PropertyMetadata(BindToSelectedItem));
      SelectionModeProperty = DependencyProperty.Register("SelectionMode", typeof(SelectionMode), typeof(Viewport));
    }

    public Viewport() : base()
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

    private void SelectionHandler(object sender, VisualsSelectedEventArgs args) =>
      SelectedItems.AddRange(args.SelectedVisuals.Cast<MeshElement3D>());

    #endregion

    #region Selected Items Changed

    #region Update Visual Elements

    private void SelectMaterial(IEnumerable<MeshElement3D> models)
    {
      foreach (MeshElement3D model in models)
      {
        CachedMaterials[model] = (model.Material as MaterialGroup);

        var material = CachedMaterials[model].Clone();
        material?.Children?.Add(SelectedMaterial);
        material.Freeze();

        model.Material = material;
      }
    }

    private void UnselectMaterial(IEnumerable<MeshElement3D> models)
    {
      foreach (MeshElement3D model in models)
      {
        model.Material = CachedMaterials[model];
      }
    }

    private void AddManipulator(MeshElement3D model)
    {
      try
      {
        Manipulator = CreateManipulator();
        Manipulator.Bind(model);
        Children.Add(Manipulator);
      }
      catch (NotImplementedException) { }
    }

    private Manipulators.CombinedManipulator CreateManipulator()
    {
      switch (SelectionMode)
      {
        case SelectionMode.Deplacement:
          return new MovementManipulator();
        case SelectionMode.Transformation:
          return new ResizeManipulator();
        default:
          throw new NotImplementedException("You forgot to implement stuff!!");
      }
    }

    private void RemoveManipulator()
    {
      Manipulator?.Unbind();
      Children.Remove(Manipulator);
    }

    #endregion

    private void SelectedItemsChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
      if (sender == SelectedItems)
      {
        SelectMaterial(args.NewItems?.OfType<MeshElement3D>() ?? Enumerable.Empty<MeshElement3D>());
        UnselectMaterial(args.Action != NotifyCollectionChangedAction.Reset
          ? (IEnumerable<MeshElement3D>)args.OldItems ?? Enumerable.Empty<MeshElement3D>()
          : from mesh in Children.OfType<MeshElement3D>()
            where (mesh.Material as MaterialGroup)?.Children?.Contains(SelectedMaterial) ?? false
            select mesh);

        RemoveManipulator();

        if (SelectedItems.Count == 1)
        {
          AddManipulator(SelectedItems.Single());
        }
      }
    }

    protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
    {
      base.OnItemsSourceChanged(oldValue, newValue);
      Children.Add(Sunlight);
      Children.Add(GridLines);
    }

    private void OnSelectedItemsChanged(ObservableCollection<MeshElement3D> newvalue, ObservableCollection<MeshElement3D> oldvalue)
    {
      RemoveManipulator();
      CachedMaterials.Clear();
      newvalue.CollectionChanged += SelectedItemsChanged;
    }

    private static void BindToSelectedItem(DependencyObject self, DependencyPropertyChangedEventArgs args)
    {
      if (args.Property == SelectedItemsProperty)
      {
        ((Viewport)self).OnSelectedItemsChanged(
          (ObservableCollection<MeshElement3D>)args.NewValue,
          (ObservableCollection<MeshElement3D>)args.OldValue);
      }
    }

    #endregion

    #region Private Fields

    private readonly IDictionary<MeshElement3D, MaterialGroup> CachedMaterials;
    private readonly SunLight Sunlight;
    private readonly GridLinesVisual3D GridLines;
    private readonly Material SelectedMaterial;
    private Manipulators.CombinedManipulator Manipulator;

    #endregion
  }
}
