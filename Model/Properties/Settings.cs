using Hymperia.Model.Modeles;

namespace Hymperia.Model.Properties
{
  public sealed partial class Settings
  {
    public void SetSettingsFromUtilisateur(Utilisateur utilisateur)
    {
      Default.Culture = utilisateur.Langue;
      Default.Theme = utilisateur.Theme;
      Default.Save();
    }

    public void SetMainConnectionString(string server, string database, string user, string password) =>
      MainDatabase = $"Server={ server }; SslMode=Preferred; Database={ database }; Username={ user }; Password={ password };";

    public void SetLocalizationConnectionString(string server, string database, string user, string password) =>
      LocalizationDatabase = $"Server={ server }; SslMode=Preferred; Database={ database }_localization; Username={ user }; Password={ password };";
  }
}
