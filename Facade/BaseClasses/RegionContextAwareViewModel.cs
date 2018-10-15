using JetBrains.Annotations;
using Prism.Mvvm;

namespace Hymperia.Facade.BaseClasses
{
  public abstract class RegionContextAwareViewModel : BindableBase
  {
    [CanBeNull]
    public object RegionContext
    {
      get => context;
      set => SetProperty(ref context, value, OnRegionContextChanged);
    }

    protected virtual void OnRegionContextChanged() { }

    #region Private FIelds

    private object context;

    #endregion
  }
}
