using System;
using JetBrains.Annotations;
using Prism.Mvvm;

namespace Hymperia.Facade.BaseClasses
{
  [Obsolete("Cette base classes sera retiré lorsque l'application passera d'un modèle basé sur les régions à un modèle basé sur l'eventaggregator")]
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
