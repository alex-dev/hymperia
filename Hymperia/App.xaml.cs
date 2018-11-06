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
using Hymperia.Facade.Views;
using Hymperia.Facade.Views.Editeur;
using Hymperia.Facade.Views.Editeur.PropertiesEditor;
using Hymperia.Model.Properties;
using Prism.Ioc;
using Prism.Ninject;

namespace Hymperia.Facade
{
  public partial class App : PrismExtensionApplication
  {
    /// <summary>Permet d'enregistrer des types injectables au kernel de Ninject.</summary>
    protected override void RegisterTypes(IContainerRegistry registry)
    {
      registry.RegisterInstance((Point3DToPointConverter)Resources["Point3DToPoint"]);
      registry.RegisterInstance((TransformConverter)Resources["Transform"]);

      registry.RegisterForNavigation<AffichageProjets>(NavigationKeys.AffichageProjets);
      registry.RegisterForNavigation<Editeur>(NavigationKeys.Editeur);

      registry.RegisterForNavigation<EllipsoideEditor>(NavigationKeys.EllipsoideEditor);
      registry.RegisterForNavigation<ConeEditor>(NavigationKeys.ConeEditor);
      registry.RegisterForNavigation<CylindreEditor>(NavigationKeys.CylindreEditor);
      registry.RegisterForNavigation<PrismeEditor>(NavigationKeys.PrismeEditor);
    }

    /// <summary>Trouve la fenêtre via le kernel de Ninject.</summary>
    protected override Window CreateShell() => Container.Resolve<FenetrePrincipale>();

    /// <summary>Force le language de l'application.</summary>
    protected override void OnStartup(StartupEventArgs e)
    {
      var culture = CreateAppCulture();
      var language = XmlLanguage.GetLanguage(culture.IetfLanguageTag);

      Thread.CurrentThread.CurrentCulture = culture;
      Thread.CurrentThread.CurrentUICulture = culture;
      FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(language));

      Application.Current.Resources.MergedDictionaries.Add(CreateTheme());


      base.OnStartup(e);
    }

    /// <summary>Crée une culture basé sur la culture courante.</summary>
    [Obsolete("va être déplacer vers un service prochainement pour i18n.")]
    private CultureInfo CreateAppCulture()
    {
      var culture = CultureInfo.CreateSpecificCulture(Settings.Default.Culture);
      culture.NumberFormat.CurrencySymbol = "$";
      return culture;
    }

    [Obsolete("Sera un contrôle avec choix pour l'utilisateur dans une version prochaine.")]
    private ResourceDictionary CreateTheme() => new ResourceDictionary
    {
      Source = new Uri($"pack://application:,,,/WPFResources/Themes/{Settings.Default.Theme}.xaml", UriKind.Absolute)
    };
  }
}
