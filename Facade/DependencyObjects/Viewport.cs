using System;
using System.Collections;
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
    private readonly MaterialGroup SelectedMaterial;
    private readonly Brush Specular = Brushes.Red;

    #endregion

    static Viewport()
    {
      var metadata = new PropertyMetadata(new PropertyChangedCallback(BindToSelectedItem));
      SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(BulkObservableCollection<MeshElement3D>), typeof(Viewport), metadata);
    }

    public Viewport() : base()
    {
      Sunlight = new SunLight();
      GridLines = new GridLinesVisual3D { Width = 1000, Length = 1000, MinorDistance = 0.1, MajorDistance = 1, Thickness = 0.01 };
      SelectedMaterial = new MaterialGroup();
      Specular = Specular.Clone();
      Specular.Opacity = 0.5;
      SelectedMaterial.Children.Add(new SpecularMaterial(Specular, 85));
      InputBindings.Add(new MouseBinding(new RectangleSelectionCommand(Viewport, ClearSelectionHandler), new MouseGesture(MouseAction.LeftClick, ModifierKeys.Shift)));
      InputBindings.Add(new MouseBinding(new PointSelectionCommand(Viewport, ClearSelectionHandler), new MouseGesture(MouseAction.LeftClick)));
      InputBindings.Add(new MouseBinding(new PointSelectionCommand(Viewport, SelectionHandler), new MouseGesture(MouseAction.LeftClick, ModifierKeys.Control)));
    }

    #region Methods

    #region Selection Handlers

    private void ClearSelectionHandler(object sender, VisualsSelectedEventArgs args)
    {
      SelectedItems.Clear();
      SelectionHandler(sender, args);
    }

    private void SelectionHandler(object sender, VisualsSelectedEventArgs args) => SelectedItems.AddRange(args.SelectedVisuals.OfType<MeshElement3D>());

    private void SelectMaterial(IEnumerable<MeshElement3D> models)
    {
      foreach (MeshElement3D model in models)
      {
        //TODO A changé!!! 
        model.Fill = Brushes.Red;
        //(model.Material as MaterialGroup)?.Children.Add(SelectedMaterial.Children.First());
      }
    }

    private void UnselectMaterial(IEnumerable<MeshElement3D> models)
    {
      foreach (MeshElement3D model in models)
      {
        //TODO A changé!!!
        model.Fill = Brushes.Blue;
        //var b = model.Material.IsFrozen;
        //(model.Material as MaterialGroup)?.Children.Remove(SelectedMaterial.Children.First());
      }
    }

    #endregion

    private void SelectedItemsChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
      if (sender == SelectedItems)
      {
        SelectMaterial(args.NewItems?.OfType<MeshElement3D>()?.Distinct() ?? Enumerable.Empty<MeshElement3D>());
        UnselectMaterial(args.Action == NotifyCollectionChangedAction.Reset
          ? Children.OfType<MeshElement3D>()
          : args.OldItems?.OfType<MeshElement3D>()?.Distinct() ?? Enumerable.Empty<MeshElement3D>());
      }
    }

    protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
    {
      base.OnItemsSourceChanged(oldValue, newValue);
      foreach (MeshElement3D value in newValue)
      {
        value.Fill = Brushes.Blue;
      }

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
