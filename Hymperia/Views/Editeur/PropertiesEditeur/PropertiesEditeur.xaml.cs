using System;
using System.Windows;
using System.Windows.Controls;
using Hymperia.Facade.Constants;
using Hymperia.Facade.Extensions;
using Prism;
using Prism.Ioc;
using Prism.Regions;

namespace Hymperia.Facade.Views.Editeur.PropertiesEditeur
{
  public partial class PropertiesEditeur : UserControl, IActiveAware
  {
    #region Constructors

    public PropertiesEditeur(IRegionManager manager, IContainerExtension container)
    {
      Container = container;
      Manager = manager;

      Loaded += RegisterViews;
      InitializeComponent();
    }

    #endregion

    #region Views Registration

    private void RegisterViews(object sender, RoutedEventArgs e)
    {
      Loaded -= RegisterViews;

      PositionPropertiesRegion = Manager.Regions[RegionKeys.PositionPropertiesRegion];
      SpecificPropertiesRegion = Manager.Regions[RegionKeys.SpecificPropertiesRegion];

      PositionPropertiesRegion.Add(Container.Resolve<PositionEditeur>(), ViewKeys.PositionEditeur);
      SpecificPropertiesRegion.Add(Container.Resolve<ConeEditeur>(), ViewKeys.ConeEditor);
      SpecificPropertiesRegion.Add(Container.Resolve<CylindreEditeur>(), ViewKeys.CylindreEditor);
      SpecificPropertiesRegion.Add(Container.Resolve<EllipsoideEditeur>(), ViewKeys.EllipsoideEditor);
      SpecificPropertiesRegion.Add(Container.Resolve<PrismeEditeur>(), ViewKeys.PrismeRectangulaireEditor);

      PositionPropertiesRegion?.Deactivate();
      SpecificPropertiesRegion?.Deactivate();
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

    private void OnActivation() { }
    private void OnDeactivation()
    {
      PositionPropertiesRegion?.Deactivate();
      SpecificPropertiesRegion?.Deactivate();
    }

    private bool isActive;

    #endregion

    #region Services

    private readonly IContainerExtension Container;
    private readonly IRegionManager Manager;
    private IRegion PositionPropertiesRegion;
    private IRegion SpecificPropertiesRegion;

    #endregion
  }
}
