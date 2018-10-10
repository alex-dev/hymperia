using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Prism.Common;
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

    private static void RegionContextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
    {
      if (!((RegionContextAwareUserControl)sender).Monitor.Busy)
      {
        using (var monitor = ((RegionContextAwareUserControl)sender).Monitor.Enter())
        {
          R.RegionContext.GetObservableContext(sender).Value = sender.GetValue(RegionContextProperty);
        }
      }
    }

    protected RegionContextAwareUserControl()
    {
      Monitor = new Monitor_();
      RegionContext = R.RegionContext.GetObservableContext(this).Value;
      R.RegionContext.GetObservableContext(this).PropertyChanged += RegionContextChanged;
    }

    private void RegionContextChanged(object sender, PropertyChangedEventArgs args)
    {
      if (!Monitor.Busy)
      {
        using (var monitor = Monitor.Enter())
        {
          RegionContext = R.RegionContext.GetObservableContext(this).Value;
        }
      }
    }

    #region Block Reentrancy

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
