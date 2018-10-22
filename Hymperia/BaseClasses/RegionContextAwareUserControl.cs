using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using JetBrains.Annotations;
using R = Prism.Regions;

namespace Hymperia.Facade.BaseClasses
{
  [Obsolete("Cette base classes sera retiré lorsque l'application passera d'un modèle basé sur les régions à un modèle basé sur l'eventaggregator")]
  public abstract class RegionContextAwareUserControl : UserControl
  {
    #region Dependency Properties

    public static readonly DependencyProperty RegionContextProperty =
      DependencyProperty.Register("RegionContext", typeof(object), typeof(RegionContextAwareUserControl), new PropertyMetadata(RegionContextChanged));

    #endregion

    [CanBeNull]
    public object RegionContext
    {
      get => GetValue(RegionContextProperty);
      set => SetValue(RegionContextProperty, value);
    }

    #region Constructors

    protected RegionContextAwareUserControl()
    {
      Monitor = new SimpleMonitor();
      RegionContext = R.RegionContext.GetObservableContext(this).Value;
      R.RegionContext.GetObservableContext(this).PropertyChanged += RegionContextChanged;
    }

    #endregion

    #region Region Context Changes Handlers

    private static void RegionContextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) =>
      ((RegionContextAwareUserControl)sender).RegionContextChanged(e);

    [Obsolete]
    protected virtual void RegionContextChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!IsBusy())
      {
        using (var monitor = Monitor.Enter())
        {
          RegionContext = R.RegionContext.GetObservableContext(this).Value;
        }
      }
    }

    [Obsolete]
    protected virtual void RegionContextChanged(DependencyPropertyChangedEventArgs e)
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

    [Pure]
    protected bool IsBusy() => Monitor.Busy;
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable",
      Justification = @"Disposable field is only used for blocking reentrancy and doesn't manage any disposable resource.")]
    [NotNull]
    private readonly SimpleMonitor Monitor;

    private class SimpleMonitor : IDisposable
    {
      public bool Busy { get; private set; }

      [NotNull]
      public SimpleMonitor Enter()
      {
        Busy = true;
        return this;
      }

      [Pure]
      public void Dispose() => Busy = false;
    }

    #endregion

    #endregion
  }
}
