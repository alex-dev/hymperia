using Prism.Commands;
using Prism.Ioc;

namespace Prism.Ninject
{
  public abstract class PrismExtensionApplication : PrismApplication
  {
    protected override void RegisterRequiredTypes(IContainerRegistry registry)
    {
      base.RegisterRequiredTypes(registry);
      registry.RegisterSingleton<ICommandAggregator, CommandAggregator>();
    }
  }
}
