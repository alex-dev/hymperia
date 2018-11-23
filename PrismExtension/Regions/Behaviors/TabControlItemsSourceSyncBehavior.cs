using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Prism.Regions.Behaviors
{
  /// <summary> Defines an attached behavior for <see cref="TabControl"/> copying <see cref="SelectorItemsSourceSyncBehavior"/>
  /// to find headers for <see cref="TabItem"/>.</summary>
  /// <seealso cref="SelectorItemsSourceSyncBehavior"/>
  public class TabControlItemsSourceSyncBehavior : RegionBehavior, IHostAwareRegionBehavior
  {
    /// <seealso cref="SelectorItemsSourceSyncBehavior.BehaviorKey"/>
    public static readonly string BehaviorKey = nameof(TabControlItemsSourceSyncBehavior);

    /// <seealso cref="SelectorItemsSourceSyncBehavior.HostControl"/>
    public DependencyObject HostControl
    {
      get => host;
      set => host = value as TabControl;
    }

    /// <seealso cref="SelectorItemsSourceSyncBehavior.OnAttach"/>
    protected override void OnAttach()
    {
      if (!(host.ItemsSource is null)
        || !(BindingOperations.GetBinding(host, ItemsControl.ItemsSourceProperty) is null))
        throw new InvalidOperationException();

      SynchronizeItems();
      
      Region.Views.CollectionChanged += OnViewsChanged;
      Region.ActiveViews.CollectionChanged += OnActiveViewsChanged;
      host.SelectionChanged += OnHostControlSelectionChanged;
    }

    private void SynchronizeItems()
    {
      var items = new object[host.Items.Count];
      host.Items.CopyTo(items, 0);

      foreach (object view in Region.Views)
        host.Items.Add(view);

      foreach (object item in items)
        Region.Add(item);
    }

    private void OnViewsChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      foreach (object item in e.NewItems?.Cast<object>() ?? Enumerable.Empty<object>())
        host.Items.Add(item);
      foreach (object item in e.OldItems?.Cast<object>() ?? Enumerable.Empty<object>())
        host.Items.Remove(item);
    }

    private void OnActiveViewsChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (UpdatingActiveViewsInHostControlSelectionChanged)
        // If we are updating the ActiveViews collection in the HostControlSelectionChanged, that 
        // means the user has set the SelectedItem or SelectedItems himself and we don't need to do that here now
        return;

      if (e.Action == NotifyCollectionChangedAction.Add)
      {
        if (!(host.SelectedItem is null)
          && host.SelectedItem != e.NewItems[0]
          && Region.ActiveViews.Contains(host.SelectedItem))
          Region.Deactivate(host.SelectedItem);

        host.SelectedItem = e.NewItems[0];
      }
      else if (e.Action == NotifyCollectionChangedAction.Remove &&
               e.OldItems.Contains(host.SelectedItem))
      {
        host.SelectedItem = null;
      }
    }

    private void OnHostControlSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      try
      {
        // Record the fact that we are now updating active views in the HostControlSelectionChanged method. 
        // This is needed to prevent the ActiveViews_CollectionChanged() method from firing. 
        UpdatingActiveViewsInHostControlSelectionChanged = true;

        if (e.OriginalSource == sender)
        {
          foreach (object item in e.RemovedItems)
            // check if the view is in both Views and ActiveViews collections (there may be out of sync)
            if (Region.Views.Contains(item) && Region.ActiveViews.Contains(item))
              Region.Deactivate(item);

          foreach (object item in e.AddedItems)
            if (Region.Views.Contains(item) && !Region.ActiveViews.Contains(item))
              Region.Activate(item);
        }
      }
      finally
      {
        UpdatingActiveViewsInHostControlSelectionChanged = false;
      }
    }

    private bool UpdatingActiveViewsInHostControlSelectionChanged;
    private TabControl host;
  }
}
