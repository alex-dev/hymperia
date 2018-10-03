using System.Windows.Controls;
using System.Windows.Data;
using Prism.Common;
using R = Prism.Regions;
using System.Windows;

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
      RegionContextProperty = DependencyProperty.Register("RegionContext", typeof(object), typeof(RegionContextAwareUserControl));
    }

    protected RegionContextAwareUserControl()
    {
      Load += BindRegionContext;
    }
    
    private void BindRegionContext()
    {
      BindingOperations.SetBinding(R.RegionContext.GetObservableContext(this), ObservableObject<object>.ValueProperty, new Binding("RegionContext") { Source = this, Mode = BindingMode.OneWayToSource });
      /*R.RegionContext.GetObservableContext(this).PropertyChanged += (sender, args) =>
      {
        RegionContext = ((ObservableObject<object>)sender).Value;
      };
      RegionContext = R.RegionContext.GetObservableContext(this).Value;*/
    }
  }
}
