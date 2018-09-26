using System.Windows;
using Prism.Ioc;
using Prism.Ninject;
using Hymperia.Facade.Services;
using Hymperia.Facade.Views;
using Hymperia.Facade.Views.Editeur;

namespace Hymperia.Facade
{
  public partial class App : PrismApplication
  {
    /// <summary>Permet d'enregistrer des types injectables au kernel de Ninject.</summary>
    protected override void RegisterTypes(IContainerRegistry registry)
    {
      registry.Register<ContextFactory>();
      registry.Register<PointValueConverter>();
      registry.Register<ConvertisseurFormes>();
      registry.RegisterForNavigation<Editeur>("Editeur");
    }

    /// <summary>Trouve la fenêtre via le kernel de Ninject.</summary>
    protected override Window CreateShell() => Container.Resolve<FenetrePrincipale>();
  }
}
