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
      ChangementLangue = container.Resolve<ChangementLangue>();
      ChangementMotDePasse = container.Resolve<ChangementMotDePasse>();
      ConnexionAutomatique = container.Resolve<ConnexionAutomatique>();

      Loaded += RegisterViews;
      InitializeComponent();
    }

    #endregion

    #region Views Registration

    private void RegisterViews(object sender, RoutedEventArgs e)
    {
      Loaded -= RegisterViews;

      ChangementLangueRegion = Manager.Regions[RegionKeys.ChangementLangueRegion];
      ChangementMotDePasseRegion = Manager.Regions[RegionKeys.ChangementMotDePasseRegion];
      ConnexionAutomatiqueRegion = Manager.Regions[RegionKeys.ConnexionAutomatiqueRegion];

      ChangementLangueRegion.Add(ChangementLangue, ViewKeys.ChangementLangue);
      ChangementMotDePasseRegion.Add(ChangementMotDePasse, ViewKeys.ChangementMotDePasse);
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
      ChangementLangueRegion?.Activate(ChangementLangueRegion?.GetView(ViewKeys.ChangementLangue));
      ChangementMotDePasseRegion?.Activate(ChangementMotDePasseRegion?.GetView(ViewKeys.ChangementMotDePasse));
      ConnexionAutomatiqueRegion?.Activate(ConnexionAutomatiqueRegion?.GetView(ViewKeys.ConnexionAutomatique));
    }

    private void OnDeactivation()
    {
      ChangementMotDePasseRegion?.Deactivate();
      ChangementLangueRegion?.Deactivate();
      ConnexionAutomatiqueRegion?.Deactivate();
    }

    private bool isActive;

    #endregion

    #region Services

    private readonly IRegionManager Manager;
    private IRegion ChangementLangueRegion;
    private IRegion ChangementMotDePasseRegion;
    private IRegion ConnexionAutomatiqueRegion;

    #endregion

    #region Views

    private readonly ChangementLangue ChangementLangue;
    private readonly ChangementMotDePasse ChangementMotDePasse;
    private readonly ConnexionAutomatique ConnexionAutomatique;

    #endregion
  }
}
