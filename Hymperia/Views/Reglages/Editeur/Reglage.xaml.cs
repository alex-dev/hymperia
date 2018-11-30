/*
 * Auteur : Antoine Mailhot
 * Date de création : 28 novembre 2018
*/

using System;
using System.Windows;
using System.Windows.Controls;
using Hymperia.Facade.Constants;
using Hymperia.Facade.Extensions;
using Prism;
using Prism.Ioc;
using Prism.Regions;

namespace Hymperia.Facade.Views.Reglages.Editeur
{
  public partial class Reglage : UserControl, IActiveAware
  {
    #region Constructors

    public Reglage(IRegionManager manager, IContainerExtension container)
    {
      Manager = manager;
      AccesProjet = container.Resolve<AccesProjet>();
      RenommerProjet = container.Resolve<RenommerProjet>();

      Loaded += RegisterViews;
      InitializeComponent();
    }

    #endregion

    #region Views Registration

    private void RegisterViews(object sender, RoutedEventArgs e)
    {
      Loaded -= RegisterViews;

      AccesProjetRegion = Manager.Regions[RegionKeys.AccesProjetRegion];
      RenommerProjetRegion = Manager.Regions[RegionKeys.RenommerProjetRegion];

      AccesProjetRegion.Add(AccesProjet, ViewKeys.AccesProjet);
      RenommerProjetRegion.Add(RenommerProjet, ViewKeys.RenommerProjet);
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
      AccesProjetRegion?.Activate(AccesProjetRegion?.GetView(ViewKeys.AccesProjet));
      RenommerProjetRegion?.Activate(RenommerProjetRegion?.GetView(ViewKeys.RenommerProjet));
    }

    private void OnDeactivation()
    {
      AccesProjetRegion?.Deactivate();
      RenommerProjetRegion?.Deactivate();
    }

    private bool isActive;

    #endregion

    #region Services

    private readonly IRegionManager Manager;
    private IRegion AccesProjetRegion;
    private IRegion RenommerProjetRegion;

    #endregion

    #region Views

    private readonly AccesProjet AccesProjet;
    private readonly RenommerProjet RenommerProjet;

    #endregion
  }
}
