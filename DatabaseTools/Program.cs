using System.Globalization;
using System.Threading;
using Hymperia.Model;
using Microsoft.Extensions.CommandLineUtils;

namespace Hymperia.DatabaseTools
{
  internal static class Program
  {
    private static CommandOption Culture;
    public const string MainConfiguration = DatabaseContext.ConfigurationName;
    public const string LocalizationConfiguration = LocalizationContext.ConfigurationName;

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

    private static void SetAppCulture(string culture)
    {
      var culture_ = CultureInfo.CreateSpecificCulture(culture);
      Thread.CurrentThread.CurrentCulture = culture_;
      Thread.CurrentThread.CurrentUICulture = culture_;
    }

    private static CommandLineApplication CreateApp()
    {
      var app = new CommandLineApplication(true);
      Culture = app.Option("-c|--culture", "Détermine la culture à utiliser pour l'affichage du résultat. Supporte : fr-CA, en-US.", CommandOptionType.SingleValue);
      app.HelpOption("-h|--help");
      app.Command("deployer", Migrate, true);
      app.Command("formes", Formes, true);
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
      var option = command.Option("-i|--initialisation", "Initialise ou réinitialise la base de données.", CommandOptionType.NoValue);
      option.ShowInHelpText = true;
      command.Description = $"Déploie ou met à jour les bases de données ciblées par les connections { MainConfiguration } et { LocalizationConfiguration } du fichier connections.config.";
      command.ShowInHelpText = true;
      command.HelpOption("-h|--help");

      command.OnExecute(async () =>
      {
        await Deploy.Migrate(option.HasValue());
        return 0;
      });
    }

    private static void Formes(CommandLineApplication command)
    {
      var option = command.Option("-m|--migration", "Réinitialise la base de données.", CommandOptionType.NoValue);
      option.ShowInHelpText = true;
      command.Description = $"Retourne les formes supportés par l'application";
      command.ShowInHelpText = true;
      command.HelpOption("-h|--help");

      command.OnExecute(async () =>
      {
        SetAppCulture(Culture.HasValue() ? Culture.Value() : "fr-CA");
        await Deploy.Migrate(option.HasValue());
        Printer.PrintFormes();
        return 0;
      });
    }


    private static void Materiaux(CommandLineApplication command)
    {
      var option = command.Option("-m|--migration", "Réinitialise la base de données.", CommandOptionType.NoValue);
      option.ShowInHelpText = true;
      command.Description = $"Retourne les objets de la table 'Materiaux' la base de donnée ciblée par la connection { MainConfiguration } du fichier connections.config.";
      command.ShowInHelpText = true;
      command.HelpOption("-h|--help");

      command.OnExecute(async () =>
      {
        SetAppCulture(Culture.HasValue() ? Culture.Value() : "fr-CA");
        await Deploy.Migrate(option.HasValue());
        Printer.Print(await Query.QueryMateriaux());
        return 0;
      });
    }

    private static void Projets(CommandLineApplication command)
    {
      var option = command.Option("-m|--migration", "Réinitialise la base de données.", CommandOptionType.NoValue);
      option.ShowInHelpText = true;
      command.Description = $"Retourne les objets de la table 'Projets' la base de donnée ciblée par la connection { MainConfiguration } du fichier connections.config.";
      command.ShowInHelpText = true;
      command.HelpOption("-h|--help");

      command.OnExecute(async () =>
      {
        SetAppCulture(Culture.HasValue() ? Culture.Value() : "fr-CA");
        await Deploy.Migrate(option.HasValue());
        Printer.Print(await Query.QueryProjets());
        return 0;
      });
    }

    private static void Utilisateurs(CommandLineApplication command)
    {
      var option = command.Option("-m|--migration", "Réinitialise la base de données.", CommandOptionType.NoValue);
      option.ShowInHelpText = true;
      command.Description = $"Retourne les objets de la table 'Utilisateurs' la base de donnée ciblée par la connection { MainConfiguration } du fichier connections.config.";
      command.ShowInHelpText = true;
      command.HelpOption("-h|--help");

      command.OnExecute(async () =>
      {
        SetAppCulture(Culture.HasValue() ? Culture.Value() : "fr-CA");
        await Deploy.Migrate(option.HasValue());
        Printer.Print(await Query.QueryUtilisateurs());
        return 0;
      });
    }
  }
}
