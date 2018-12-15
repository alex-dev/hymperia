/*
 * Auteur : Antoine Mailhot
 * Date de création : 1 décembre 2018
*/

using System.Threading.Tasks;
using System.Windows.Input;
using Hymperia.Facade.Constants;
using Hymperia.Facade.Loaders;
using Hymperia.Facade.Properties;
using Hymperia.Facade.Services;
using JetBrains.Annotations;
using MySql.Data.MySqlClient;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using S = Hymperia.Model.Properties.Settings;

namespace Hymperia.Facade.ViewModels.Reglages.BD
{
  public sealed class ReglageViewModel : ValidatingBase
  {
    #region Properties

    #region Binding

    public string Host
    {
      get => host;
      set => SetProperty(ref host, value);
    }

    public string Database
    {
      get => database;
      set => SetProperty(ref database, value);
    }

    public string User
    {
      get => user;
      set => SetProperty(ref user, value);
    }

    public string Password
    {
      get => password;
      set => SetProperty(ref password, value);
    }

    #endregion

    #region Interaction Requests

    public InteractionRequest<INotification> InformationRequest { get; } = new InteractionRequest<INotification>();

    #endregion

    #region Commands

    public ICommand NavigateBack { get; }

    public ICommand Connexion { get; }

    #endregion

    #region Asynchronous Loading

    public AsyncLoader SaveLoader { get; } = new AsyncLoader();

    #endregion

    #endregion

    #region Constructors

    public ReglageViewModel([NotNull] ContextFactory factory, [NotNull] IRegionManager manager)
    {
      NavigateBack = new DelegateCommand(_NavigateBack);
      Connexion = new DelegateCommand(() => SaveLoader.Loading = Save());

      ContextFactory = factory;
      Manager = manager;
    }

    #endregion

    #region Navigation Commands

    private void _NavigateBack() =>
      Manager.Regions[RegionKeys.ContentRegion].NavigationService.Journal.GoBack();

    #endregion

    #region Command Connexion

    private async Task Save()
    {
      S.Default.SetMainConnectionString(Host, Database, User, Password);
      S.Default.SetLocalizationConnectionString(Host, Database, User, Password);

      try
      {
        await (App.Current as App).UpdateDatabases();
        S.Default.Save();
      }
      catch (MySqlException e)
      {
        S.Default.Reload();

        InformationRequest.Raise(new Notification
        {
          Title = Resources.Echec,
          Content = e.Message
        });
      }
    }

    #endregion

    #region Services

    [NotNull]
    private readonly ContextFactory ContextFactory;
    [NotNull]
    private readonly IRegionManager Manager;

    #endregion

    #region Private Fields

    private string host;
    private string database;
    private string user;
    private string password;

    #endregion
  }
}
