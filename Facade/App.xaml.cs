using System.Windows;
using Ninject.Extensions.NamedScope;
using Prism.Ioc;
using Prism.Ninject;
using Prism.Ninject.Ioc;
using Hymperia.Facade.Views;
using Hymperia.Facade.Views.Editeur;
using Hymperia.Model;

namespace Hymperia.Facade
{
  public partial class App : PrismApplication
  {
    /// <summary>Permet d'enregistrer des types injectables au kernel de Ninject.</summary>
    protected override void RegisterTypes(IContainerRegistry registry)
    {
      registry.RegisterForNavigation<ProjetEditeur>("Editeur");
      (registry as NinjectContainerExtension)?.Instance?.Bind<DatabaseContext>().ToSelf().InCallScope();
    }

    /// <summary>Trouve la fenêtre via le kernel de Ninject.</summary>
    protected override Window CreateShell() => Container.Resolve<FenetrePrincipale>();
  }
}
