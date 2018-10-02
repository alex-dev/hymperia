using Prism.Mvvm;

namespace Hymperia.Facade.ViewModels
{
  public abstract class RegionContextBase : BindableBase
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
