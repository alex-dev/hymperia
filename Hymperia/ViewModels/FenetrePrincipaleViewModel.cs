using System;
using System.Windows.Input;
using Hymperia.Facade.Constants;
using Hymperia.Facade.Services;
using Hymperia.Facade.Titles;
using Hymperia.Model.Properties;
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
      const string inner = "Invalid connection strings";

      Manager.RequestNavigate(RegionKeys.ContentRegion, NavigationKeys.Connexion);

      try
      {
        if (string.IsNullOrWhiteSpace(Settings.Default.MainDatabase)
          || string.IsNullOrWhiteSpace(Settings.Default.LocalizationDatabase))
          throw new InvalidOperationException(inner);

        await (App.Current as App).UpdateDatabases();
      }
      catch (Exception e) when (e is MySqlException
        || (e is InvalidOperationException && e.Message == inner))
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
