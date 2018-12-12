using System;
using System.ComponentModel;
using System.Configuration;

namespace Hymperia.Model.Properties
{
  public static partial class Resources
  {
    public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;

    static Resources()
    {
      // Allow the StaticPropertyChangedEventManager to find the handler and static bindings to works.
      StaticPropertyChanged += (sender, e) => { };
      Settings.Default.SettingChanging += OnLanguageChanged;
    }

    private static void OnLanguageChanged(object sender, SettingChangingEventArgs e)
    {
      if (e.SettingName == nameof(Settings.Default.Culture))
        RaiseStaticPropertiesChanged();
    }

    private static void RaiseStaticPropertyChanged(string name) =>
      StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(name));
  }
}
