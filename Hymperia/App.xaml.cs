using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using Hymperia.Facade.Converters;
using Hymperia.Facade.Converters.PointsToHeightConverters;
using Hymperia.Facade.Views;
using Hymperia.Facade.Views.Editeur;
using Prism.Ioc;
using Prism.Ninject;

namespace Hymperia.Facade
{
  public partial class App : PrismApplication
  {
    /// <summary>Permet d'enregistrer des types injectables au kernel de Ninject.</summary>
    protected override void RegisterTypes(IContainerRegistry registry)
    {
      registry.RegisterInstance((HeightToPointsConverter)Resources["HeightToPoints"]);
      registry.RegisterInstance((PointsToHeightConverter)Resources["PointsToHeight"]);
      registry.RegisterInstance((Point3DToPointConverter)Resources["Point3DToPoint"]);
      registry.RegisterInstance((TransformConverter)Resources["Transform"]);
      registry.RegisterForNavigation<Editeur>("Editeur");
    }

    /// <summary>Trouve la fenêtre via le kernel de Ninject.</summary>
    protected override Window CreateShell() => Container.Resolve<FenetrePrincipale>();

    /// <summary>Force le language de l'application.</summary>
    protected override void OnStartup(StartupEventArgs args)
    {
      var culture = CreateAppCulture();
      var language = XmlLanguage.GetLanguage(culture.IetfLanguageTag);

      FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(language));
      base.OnStartup(args);
    }

    /// <summary>Crée une culture basé sur la culture courante.</summary>
    [Obsolete("va être déplacer vers un service prochainement pour i18n.")]
    private CultureInfo CreateAppCulture()
    {
      var culture = new CultureInfo(Thread.CurrentThread.CurrentCulture.LCID);
      culture.NumberFormat.CurrencySymbol = "$";
      return culture;
    }
  }
}
