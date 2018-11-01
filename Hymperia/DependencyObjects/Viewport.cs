using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using HelixToolkit.Wpf;
using Hymperia.Facade.BaseClasses;
using Hymperia.Facade.DependencyObjects.Manipulators;
using JetBrains.Annotations;

namespace Hymperia.Facade.DependencyObjects
{
  /// <summary>Extension de <see cref="HelixViewport3D"/> pour supporter les transformations via des <see cref="HelixToolkit.Wpf.Manipulator"/>.</summary>
  public class Viewport : SelectionViewport
  {
    #region Dependancy Properties

    /// <seealso cref="SelectionMode"/>
    public static readonly DependencyProperty SelectionModeProperty =
      DependencyProperty.Register(nameof(SelectionMode), typeof(SelectionMode), typeof(Viewport), new PropertyMetadata(SelectionMode.Deplacement, OnSelectionModeChanged));

    #endregion

    #region Properties

    /// <summary>Le <see cref="SelectionMode"/> que le viewport doit utiliser pour afficher ses <see cref="HelixToolkit.Wpf.Manipulator"/>.</summary>
    public SelectionMode SelectionMode
    {
      get => (SelectionMode)GetValue(SelectionModeProperty);
      set => SetValue(SelectionModeProperty, value);
    }

    #endregion


    #region Handle Manipulators

    private void AddManipulator([NotNull] MeshElement3D model)
    {
      Manipulator = CreateManipulator();
      Manipulator.Bind(model);
      Children.Add(Manipulator);
    }

    [NotNull]
    private Manipulators.CombinedManipulator CreateManipulator()
    {
      switch (SelectionMode)
      {
        case SelectionMode.Deplacement:
          return new MovementManipulator();
        case SelectionMode.Transformation:
          return new ResizeManipulator();
        default:
          throw new InvalidOperationException
            (Properties.Resources.ViewportManipulatorSupport(SelectionMode.Deplacement, SelectionMode.Transformation));
      }
    }

    private void RemoveManipulator()
    {
      Manipulator?.Unbind();
      Children.Remove(Manipulator);
    }

    private void ReloadManipulator()
    {
      RemoveManipulator();

      if (SelectedItems.Count != 1)
        return;

      var model = SelectedItems.Single();
      AddManipulator(model);
    }

    #endregion

    #region SelectedItems Changed

    /// inheritdoc/>
    protected override void OnSelectedItemsCollectionChanged([NotNull] object sender, [NotNull] NotifyCollectionChangedEventArgs e)
    {
      base.OnSelectedItemsCollectionChanged(sender, e);
      ReloadManipulator();
    }

    /// inheritdoc/>
    protected override void OnSelectedItemsChanged(
      [ItemNotNull] ObservableCollection<MeshElement3D> oldvalue,
      [NotNull][ItemNotNull] ObservableCollection<MeshElement3D> newvalue)

    {
      RemoveManipulator();
      base.OnSelectedItemsChanged(oldvalue, newvalue);
    }

    #endregion

    #region SelectionMode Changed

    private static void OnSelectionModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
      ((Viewport)d).ReloadManipulator();

    #endregion

    #region Private Fields

    private Manipulators.CombinedManipulator Manipulator;

    #endregion
  }
}
