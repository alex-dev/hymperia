using Prism.Mvvm;

namespace Hymperia.Facade.BaseClasses
{
  public abstract class RegionContextAwareViewModel : BindableBase
  {
    private object context;

    public object RegionContext
    {
      get => context;
      set => SetProperty(ref context, value, OnRegionContextChanged);
    }

    protected virtual void OnRegionContextChanged() { }
  }
}
