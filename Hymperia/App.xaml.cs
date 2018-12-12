/* 
 * Auteur : Alexandre
 * Date de création : 6 septembre 2018
*/

using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using Hymperia.Facade.Constants;
using Hymperia.Facade.Converters;
using Hymperia.Facade.Services;
using Hymperia.Facade.Views;
using Hymperia.Facade.Views.Editeur;
using Hymperia.Model.Properties;
using Prism.Ioc;
using Prism.Ninject;
using A = Hymperia.Facade.Views.Reglages.Application;
using E = Hymperia.Facade.Views.Reglages.Editeur;
using B = Hymperia.Facade.Views.Reglages.BD;
using System.Configuration;

namespace Hymperia.Facade
{
  public partial class App : PrismExtensionApplication
  {
    private ResourceDictionary CurrentTheme;

    /// <summary>Permet d'enregistrer des types injectables au kernel de Ninject.</summary>
    protected override void RegisterTypes(IContainerRegistry registry)
    {
      registry.RegisterInstance((Point3DToPointConverter)Resources["Point3DToPoint"]);
      registry.RegisterInstance((TransformConverter)Resources["Transform"]);

      registry.RegisterSingleton<ContextFactory>();
      registry.RegisterSingleton<ConvertisseurFormes>();
      registry.RegisterSingleton<ConvertisseurMateriaux>();
      registry.RegisterSingleton<ConvertisseurWrappers>();
      registry.RegisterSingleton<NotifyCollectionChangedCopyFactory>();

      registry.RegisterForNavigation<Connexion>(NavigationKeys.Connexion);
      registry.RegisterForNavigation<AffichageProjets>(NavigationKeys.AffichageProjets);
      registry.RegisterForNavigation<Editeur>(NavigationKeys.Editeur);
      registry.RegisterForNavigation<Inscription>(NavigationKeys.Inscription);
      registry.RegisterForNavigation<A.Reglage>(NavigationKeys.ReglageUtilisateur);
      registry.RegisterForNavigation<E.Reglage>(NavigationKeys.ReglageEditeur);
      registry.RegisterForNavigation<B.Reglage>(NavigationKeys.ReglageBD);
    }

    /// <summary>Trouve la fenêtre via le kernel de Ninject.</summary>
    protected override Window CreateShell() => Container.Resolve<FenetrePrincipale>();

    /// <summary>Force le language de l'application.</summary>
    protected override void OnStartup(StartupEventArgs e)
    {
      Settings.Default.SettingChanging += OnSettingChanged;

      SetAppCulture(Settings.Default.Culture);
      SetTheme(Settings.Default.Theme);

      base.OnStartup(e);
    }

    private void OnSettingChanged(object sender, SettingChangingEventArgs e)
    {
      if (e.Cancel)
        return;

      switch (e.SettingName)
      {
        case nameof(Settings.Default.Culture):
          SetAppCulture((string)e.NewValue); break;
        case nameof(Settings.Default.Theme):
          SetTheme((string)e.NewValue); break;
      }
    }


    private CultureInfo SetAppCulture(string name)
    {
      var culture = CultureInfo.CreateSpecificCulture(name);
      culture.NumberFormat.CurrencySymbol = "$";
      var language = XmlLanguage.GetLanguage(culture.IetfLanguageTag);

      Thread.CurrentThread.CurrentCulture = culture;
      Thread.CurrentThread.CurrentUICulture = culture;

      foreach (Window window in Windows)
      {
        if (window.Language == language)
          continue;

        window.Language = language;
        window.UpdateLayout();
      }

      return culture;
    }

    private ResourceDictionary SetTheme(string name)
    {
      var theme = new ResourceDictionary
      {
        Source = new Uri($"pack://application:,,,/WPFResources/Themes/{name}.xaml", UriKind.Absolute)
      };

      Resources.MergedDictionaries.Remove(CurrentTheme);
      Resources.MergedDictionaries.Add(theme);

      return CurrentTheme = theme;
    }
  }
}
