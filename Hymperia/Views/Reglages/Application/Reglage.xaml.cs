/*
 * Auteur : Antoine Mailhot
 * Date de création : 21 novembre 2018
*/

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Hymperia.Facade.Constants;
using Hymperia.Facade.Extensions;
using Prism;
using Prism.Ioc;
using Prism.Regions;

namespace Hymperia.Facade.Views.Reglages.Application
{
  public partial class Reglage : UserControl, IActiveAware
  {
    #region Constructors

    public Reglage(IRegionManager manager, IContainerExtension container)
    {
      Manager = manager;
      ChangementMotDePasse = container.Resolve<ChangementMotDePasse>();
      ChangementTheme = container.Resolve<ChangementTheme>();
      ConnexionAutomatique = container.Resolve<ConnexionAutomatique>();

      Loaded += RegisterViews;
      InitializeComponent();
    }

    #endregion

    #region Views Registration

    private void RegisterViews(object sender, RoutedEventArgs e)
    {
      Loaded -= RegisterViews;

      ChangementMotDePasseRegion = Manager.Regions[RegionKeys.ChangementMotDePasseRegion];
      ChangementThemeRegion = Manager.Regions[RegionKeys.ChangementTheme];
      ConnexionAutomatiqueRegion = Manager.Regions[RegionKeys.ConnexionAutomatiqueRegion];

      ChangementMotDePasseRegion.Add(ChangementMotDePasse, ViewKeys.ChangementMotDePasse);
      ChangementThemeRegion.Add(ChangementTheme, ViewKeys.ChangementTheme);
      ConnexionAutomatiqueRegion.Add(ConnexionAutomatique, ViewKeys.ConnexionAutomatique);
    }

    #endregion

    #region IActiveAware

    public event EventHandler IsActiveChanged;

    public bool IsActive
    {
      get => isActive;
      set
      {
        if (isActive == value)
          return;

        isActive = value;

        if (value)
          OnActivation();
        else
          OnDeactivation();

        IsActiveChanged?.Invoke(this, EventArgs.Empty);
      }
    }

    private void OnActivation()
    {
      ChangementMotDePasseRegion?.Activate(ChangementMotDePasseRegion?.GetView(ViewKeys.ChangementMotDePasse));
      ChangementThemeRegion?.Activate(ChangementThemeRegion?.GetView(ViewKeys.ChangementTheme));
      ConnexionAutomatiqueRegion?.Activate(ConnexionAutomatiqueRegion?.GetView(ViewKeys.ConnexionAutomatique));
    }

    private void OnDeactivation()
    {
      ChangementMotDePasseRegion?.Deactivate();
      ChangementThemeRegion?.Deactivate();
      ConnexionAutomatiqueRegion?.Deactivate();
    }

    private bool isActive;

    #endregion

    #region Services

    private readonly IRegionManager Manager;
    private IRegion ChangementMotDePasseRegion;
    private IRegion ChangementThemeRegion;
    private IRegion ConnexionAutomatiqueRegion;

    #endregion

    #region Views

    private readonly ChangementMotDePasse ChangementMotDePasse;
    private readonly ChangementTheme ChangementTheme;
    private readonly ConnexionAutomatique ConnexionAutomatique;

    #endregion
  }
}
