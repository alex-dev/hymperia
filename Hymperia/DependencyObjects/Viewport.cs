using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using HelixToolkit.Wpf;
using Hymperia.Facade.BaseClasses;
using Hymperia.Facade.DependencyObjects.Manipulators;

namespace Hymperia.Facade.DependencyObjects
{
  public class Viewport : SelectionViewport
  {
    #region Dependancy Properties

    public static readonly DependencyProperty SelectionModeProperty;

    #endregion

    #region Properties

    public SelectionMode SelectionMode
    {
      get => (SelectionMode)GetValue(SelectionModeProperty);
      set => SetValue(SelectionModeProperty, value);
    }

    #endregion

    #region Constructors

    static Viewport()
    {
      SelectionModeProperty = DependencyProperty.Register("SelectionMode", typeof(SelectionMode), typeof(Viewport));
    }

    public Viewport() : base() { }

    #endregion

    #region Selected Items Changed

    #region Handle Manipulators

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

    protected override void SelectedItemsChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
      base.SelectedItemsChanged(sender, args);

      if (sender == SelectedItems)
      {
        RemoveManipulator();

        if (SelectedItems.Count == 1)
        {
          AddManipulator(SelectedItems.Single());
        }
      }
    }

    protected override void OnSelectedItemsChanged(ObservableCollection<MeshElement3D> newvalue, ObservableCollection<MeshElement3D> oldvalue)
    {
      RemoveManipulator();
      base.OnSelectedItemsChanged(newvalue, oldvalue);
    }

    #endregion

    #region Private Fields

    private Manipulators.CombinedManipulator Manipulator;

    #endregion
  }
}
