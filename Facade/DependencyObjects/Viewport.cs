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

namespace Hymperia.Facade.DependencyObjects
{
  public class Viewport : HelixViewport3D
  {
    #region Dependancy Properties

    public static readonly DependencyProperty SelectedItemsProperty;

    #endregion

    #region Properties


    public BulkObservableCollection<MeshElement3D> SelectedItems
    {
      get => (BulkObservableCollection<MeshElement3D>)GetValue(SelectedItemsProperty);
      set => SetValue(SelectedItemsProperty, value);
    }

    #endregion

    #region Private Defaults

    private readonly SunLight Sunlight;
    private readonly GridLinesVisual3D GridLines;
    private readonly Material SelectedMaterial;
    private MovementManipulator MoveManipulator;
    //private ResizeManipulator SizeManipulator;

    #endregion

    static Viewport()
    {
      var metadata = new PropertyMetadata(new PropertyChangedCallback(BindToSelectedItem));
      SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(BulkObservableCollection<MeshElement3D>), typeof(Viewport), metadata);
    }

    public Viewport() : base()
    {
      var diffuse = Brushes.Red.Clone();

      Sunlight = new SunLight();
      GridLines = new GridLinesVisual3D { Width = 1000, Length = 1000, /*MinorDistance = 0.1,*/ MajorDistance = 1, Thickness = 0.01 };
      SelectedMaterial = new DiffuseMaterial(diffuse);
      InputBindings.Add(new MouseBinding(new PointSelectionCommand(Viewport, CreateHandler(true, true)), new MouseGesture(MouseAction.LeftClick, ModifierKeys.Alt)));
      InputBindings.Add(new MouseBinding(new PointSelectionCommand(Viewport, CreateHandler(false, false)), new MouseGesture(MouseAction.LeftClick, ModifierKeys.Control)));
      InputBindings.Add(new MouseBinding(new RectangleSelectionCommand(Viewport, CreateHandler(false, true)), new MouseGesture(MouseAction.LeftClick, ModifierKeys.Shift)));
    }

    #region Methods

    #region Selection Handlers

    private EventHandler<VisualsSelectedEventArgs> CreateHandler(bool single, bool clear) => (sender, args) =>
    {
      if (clear)
      {
        SelectedItems.Clear();
      }

      if (args.SelectedVisuals.Count > 0)
      {
        //var test = BindingOperations.GetBinding(args.SelectedVisuals.OfType<MeshElement3D>().First(), MeshElement3D.FillProperty);

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
      SelectedItems.AddRange(args.SelectedVisuals.OfType<MeshElement3D>().Distinct());

    private void SelectMaterial(IEnumerable<MeshElement3D> models)
    {
      foreach (MeshElement3D model in models)
      {
        MaterialGroup material = (model.Material as MaterialGroup).Clone();
        material?.Children?.Add(SelectedMaterial);
        material.Freeze();

        model.Material = material;
      }
    }

    private void UnselectMaterial(IEnumerable<MeshElement3D> models)
    {
      foreach (MeshElement3D model in models)
      {
        MaterialGroup material = (model.Material as MaterialGroup).Clone();
        material?.Children?.Remove(SelectedMaterial);
        material.Freeze();

        model.Material = material;
      }
    }

    private void AddPropertyManipulator(IEnumerable<MeshElement3D> models)
    {
      foreach (MeshElement3D model in models)
      {
        MoveManipulator = new MovementManipulator();
        MoveManipulator.Bind(model);
        this?.Children?.Add(MoveManipulator);
      }
    }

    private void RemovePropertyManipulator(IEnumerable<MeshElement3D> models)
    {
      foreach (MeshElement3D model in models)
      {
        MoveManipulator.Unbind();
        this?.Children?.Remove(MoveManipulator);
      }
    }

    #endregion

    private void SelectedItemsChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
      if (sender == SelectedItems)
      {
        SelectMaterial(args.NewItems?.OfType<MeshElement3D>() ?? Enumerable.Empty<MeshElement3D>());
        UnselectMaterial(args.Action == NotifyCollectionChangedAction.Reset
          ? Children.OfType<MeshElement3D>()
          : args.OldItems?.OfType<MeshElement3D>() ?? Enumerable.Empty<MeshElement3D>());

        if (SelectedItems.Distinct().Count() == 1)
        {
          AddPropertyManipulator(args.NewItems?.OfType<MeshElement3D>() ?? Enumerable.Empty<MeshElement3D>());
          RemovePropertyManipulator(args.Action == NotifyCollectionChangedAction.Reset
            ? Children.OfType<MeshElement3D>()
            : args.OldItems?.OfType<MeshElement3D>() ?? Enumerable.Empty<MeshElement3D>());
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
  }
}
