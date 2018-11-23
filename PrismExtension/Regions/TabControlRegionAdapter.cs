using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Controls;
using Prism.Regions.Behaviors;

namespace Prism.Regions
{
  /// <summary>
  /// Adapter that creates a new <see cref="Region"/> and binds all
  /// the views to the adapted <see cref="Selector"/>.
  /// It also keeps the <see cref="IRegion.ActiveViews"/> and the selected items
  /// of the <see cref="Selector"/> in sync.
  /// </summary>
  public class TabControlRegionAdapter : RegionAdapterBase<TabControl>
  {
    /// <inheritdoc />
    public TabControlRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
        : base(regionBehaviorFactory) { }

    /// <inheritdoc />
    protected override void Adapt(IRegion region, TabControl target) { }

    protected override void AttachBehaviors(IRegion region, TabControl target)
    {
      if (region == null)
        throw new ArgumentNullException(nameof(region));

      // Add the behavior that syncs the items source items with the rest of the items
      region.Behaviors.Add(SelectorItemsSourceSyncBehavior.BehaviorKey, new SelectorItemsSourceSyncBehavior()
      {
        HostControl = target
      });

      base.AttachBehaviors(region, target);
    }

    /// <inheritdoc />
    protected override IRegion CreateRegion() => new SingleActiveRegion();
  }
}