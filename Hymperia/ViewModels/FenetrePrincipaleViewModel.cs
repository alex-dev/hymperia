using System.Windows.Input;
using Hymperia.Facade.Constants;
using Hymperia.Facade.Services;
using Hymperia.Facade.Titles;
using MySql.Data.MySqlClient;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Titles;

namespace Hymperia.Facade.ViewModels
{
  public class FenetrePrincipaleViewModel : BindableBase
  {
    public string Title
    {
      get => title;
      set => SetProperty(ref title, value);
    }

    public ICommand Load { get; }

    public FenetrePrincipaleViewModel(ITitleAggregator titles, IRegionManager manager, ContextFactory factory)
    {
      Load = new DelegateCommand(_Load);

      Manager = manager;
      ContextFactory = factory;

      var title_ = titles.GetTitle<MainWindowTitle>();
      title_.TitleChanged += OnTitleChanged;
      title = title_.Title;
    }

    private async void _Load()
    {
      try
      {
        await (App.Current as App).UpdateDatabases();
        Manager.RequestNavigate(RegionKeys.ContentRegion, NavigationKeys.Connexion);
      }
      catch (MySqlException)
      {
        Manager.RequestNavigate(RegionKeys.ContentRegion, NavigationKeys.ReglageBD);
      }
    }

    private void OnTitleChanged(ITitle sender, TitleChangedEventArgs e) => Title = e.Title;

    private readonly IRegionManager Manager;
    private readonly ContextFactory ContextFactory;

    private string title;
  }
}
