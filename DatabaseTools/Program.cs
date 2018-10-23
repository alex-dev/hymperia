using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using Hymperia.Model;
using Hymperia.Model.Modeles;
using Microsoft.Extensions.CommandLineUtils;

namespace Hymperia.DatabaseTools
{
  internal static class Program
  {
    private const string Culture = "en-US";
    public const string MainConfiguration = DatabaseContext.ConfigurationName;
    public const string LocalizationConfiguration = LocalizationContext.ConfigurationName;

    public static int Main(string[] args)
    {
      SetAppCulture(CultureInfo.CreateSpecificCulture(Culture));
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

    private static void SetAppCulture(CultureInfo culture)
    {
      Thread.CurrentThread.CurrentCulture = culture;
      Thread.CurrentThread.CurrentUICulture = culture;
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

    private static void Materiaux(CommandLineApplication command)
    {
      var option = command.Option("-m|--migration", "Réinitialise la base de données.", CommandOptionType.NoValue);
      var localize = command.Option("-l|--localize", $"Localise les valeurs selon la base de données de localisation.", CommandOptionType.SingleValue);
      option.ShowInHelpText = true;
      command.Description = $"Retourne les objets de la table 'Materiaux' la base de donnée ciblée par la connection { MainConfiguration } du fichier connections.config.";
      command.ShowInHelpText = true;
      command.HelpOption("-h|--help");

      command.OnExecute(async () =>
      {
        await Deploy.Migrate(option.HasValue());
        var materiaux = await Query.QueryMateriaux();

        if (localize.HasValue())
        {
          Printer.PrintLocalizedMateriau(from materiau in materiaux
                                         join local in await Query.QueryLocalizedMateriaux(localize.Value())
                                           on materiau.Nom equals local.StringKey
                                         select Tuple.Create(materiau, local.Nom));
        }
        else
        {
          Printer.Print(materiaux);
        }

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
        await Deploy.Migrate(option.HasValue());
        Printer.Print(await Query.QueryUtilisateurs());
        return 0;
      });
    }
  }
}
