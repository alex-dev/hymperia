using Microsoft.Extensions.CommandLineUtils;

namespace Hymperia.ConsoleModelTest
{
  static class Program
  {

    /// <summary>Affiche l'ensemble de la collection demandée</summary>
    /// <param name="args">
    ///   <list type="number">
    ///     <item>Le type de la collection à afficher.</item>
    ///   </list>
    /// </param>
    public static int Main(string[] args)
    {
      var app = CreateApp();
      try
      {
        return app.Execute(args);
      }
      catch (CommandParsingException)
      {
        app.ShowHelp();
        return 0;
      }
    }

    private static CommandLineApplication CreateApp()
    {
      var app = new CommandLineApplication(true);
      app.HelpOption("-h|--help");
      app.Command("deployer", Migrate, true);
      app.Command("materiaux", Materiaux, true);
      app.Command("projets", Projets, true);
      app.Command("utilisateurs", Utilisateurs, true);

      app.OnExecute(() =>
      {
        app.ShowHelp();
        return 0;
      });

      return app;
    }

    private static void Migrate(CommandLineApplication command)
    {
      CommandOption option = command.Option("-i|--initialisation", "Initialise ou réinitialise la base de données.", CommandOptionType.NoValue);
      option.ShowInHelpText = true;
      command.Description = $"Déploie ou met à jour la base de données ciblée par la connection { Query.ConfigurationName } du fichier connections.config.";
      command.ShowInHelpText = true;
      command.HelpOption("-h|--help");

      command.OnExecute(async () =>
      {
        await Deploy.Migrate(option.HasValue());
        return 0;
      });
    }

    private static void Materiaux(CommandLineApplication command)
    {
      CommandOption option = command.Option("-m|--migration", "Réinitialise la base de données.", CommandOptionType.NoValue);
      option.ShowInHelpText = true;
      command.Description = $"Retourne les objets de la table 'Materiaux' la base de donnée ciblée par la connection { Query.ConfigurationName } du fichier connections.config.";
      command.ShowInHelpText = true;
      command.HelpOption("-h|--help");

      command.OnExecute(async () =>
      {
        await Deploy.Migrate(option.HasValue());
        Printer.Print(await Query.QueryMateriaux());
        return 0;
      });
    }

    private static void Projets(CommandLineApplication command)
    {
      CommandOption option = command.Option("-m|--migration", "Réinitialise la base de données.", CommandOptionType.NoValue);
      option.ShowInHelpText = true;
      command.Description = $"Retourne les objets de la table 'Projets' la base de donnée ciblée par la connection { Query.ConfigurationName } du fichier connections.config.";
      command.ShowInHelpText = true;
      command.HelpOption("-h|--help");

      command.OnExecute(async () =>
      {
        await Deploy.Migrate(option.HasValue());
        Printer.Print(await Query.QueryProjets());
        return 0;
      });
    }

    private static void Utilisateurs(CommandLineApplication command)
    {
      CommandOption option = command.Option("-m|--migration", "Réinitialise la base de données.", CommandOptionType.NoValue);
      option.ShowInHelpText = true;
      command.Description = $"Retourne les objets de la table 'Utilisateurs' la base de donnée ciblée par la connection { Query.ConfigurationName } du fichier connections.config.";
      command.ShowInHelpText = true;
      command.HelpOption("-h|--help");

      command.OnExecute(async () =>
      {
        await Deploy.Migrate(option.HasValue());
        Printer.Print(await Query.QueryUtilisateurs());
        return 0;
      });
    }
  }
}
