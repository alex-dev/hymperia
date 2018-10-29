namespace Hymperia.Model.Properties
{
  public sealed partial class Settings
  {
    public void SetMainConnectionString(string server, string database, string user, string password) =>
      MainDatabase = $"Server={ server }; SslMode=Preferred; Database={ database }; Username={ user }; Password={ password };";

    public void SetLocalizationConnectionString(string server, string database, string user, string password) =>
      LocalizationDatabase = $"Server={ server }; SslMode=Preferred; Database={ database }; Username={ user }; Password={ password };";

    /*private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
    {
      // Add code to handle the SettingChangingEvent event here.
    }

    private void SettingsSavingEventHandler(object sender, CancelEventArgs e)
    {
      // Add code to handle the SettingsSaving event here.
    }*/
  }
}
