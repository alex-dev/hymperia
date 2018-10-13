using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using R = Prism.Regions;

namespace Hymperia.Facade.BaseClasses
{
  public abstract class RegionContextAwareUserControl : UserControl
  {
    public static readonly DependencyProperty RegionContextProperty;

    public object RegionContext
    {
      get => GetValue(RegionContextProperty);
      set => SetValue(RegionContextProperty, value);
    }

    static RegionContextAwareUserControl()
    {
      RegionContextProperty = DependencyProperty.Register("RegionContext", typeof(object), typeof(RegionContextAwareUserControl), new PropertyMetadata(RegionContextChanged));
    }

    protected RegionContextAwareUserControl()
    {
      Monitor = new Monitor_();
      RegionContext = R.RegionContext.GetObservableContext(this).Value;
      R.RegionContext.GetObservableContext(this).PropertyChanged += RegionContextChanged;
    }

    private static void RegionContextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args) =>
      ((RegionContextAwareUserControl)sender).RegionContextChanged(args);

    protected virtual void RegionContextChanged(object sender, PropertyChangedEventArgs args)
    {
      if (!IsBusy())
      {
        using (var monitor = Monitor.Enter())
        {
          RegionContext = R.RegionContext.GetObservableContext(this).Value;
        }
      }
    }

    protected virtual void RegionContextChanged(DependencyPropertyChangedEventArgs args)
    {
      if (!IsBusy())
      {
        using (var monitor = Monitor.Enter())
        {
          R.RegionContext.GetObservableContext(this).Value = RegionContext;
        }
      }
    }

    #region Block Reentrancy

    protected bool IsBusy() => Monitor.Busy;

    private class Monitor_ : IDisposable
    {
      public Monitor_ Enter()
      {
        Busy = true;
        return this;
      }
      public void Dispose() => Busy = false;

      public bool Busy { get; private set; }
    }

    private readonly Monitor_ Monitor;

    #endregion
  }
}
