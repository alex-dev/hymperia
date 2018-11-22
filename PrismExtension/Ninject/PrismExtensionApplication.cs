using System.Windows.Controls;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;

namespace Prism.Ninject
{
  public abstract class PrismExtensionApplication : PrismApplication
  {
    //protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings mappings)
    //{
    //  base.ConfigureRegionAdapterMappings(mappings);

    //  if (mappings is RegionAdapterMappings)
    //  {
    //    mappings.RegisterMapping(typeof(TabControl), Container.Resolve<TabControlRegionAdapter>());
    //  }
    //}

    protected override void RegisterRequiredTypes(IContainerRegistry registry)
    {
      base.RegisterRequiredTypes(registry);
      registry.RegisterSingleton<ICommandAggregator, CommandAggregator>();
    }
  }
}
