using System.Windows;
using Prism.Ioc;
using Prism.Ninject;
using Hymperia.Facade.Views;
using Hymperia.Facade.Views.Editeur;
using Hymperia.Facade.ViewModels.Editor;

namespace Hymperia.Facade
{
  public partial class App : PrismApplication
  {
    /// <summary>Permet d'enregistrer des types injectables au kernel de Ninject.</summary>
    protected override void RegisterTypes(IContainerRegistry registry)
    {
      //registry.RegisterForNavigation<ProjetEditeur>("Editeur");
    }

    /// <summary>Trouve la fenêtre via le kernel de Ninject.</summary>
    protected override Window CreateShell()
    {
      
      return Container.Resolve<FenetrePrincipale>();
    }
  }
}
