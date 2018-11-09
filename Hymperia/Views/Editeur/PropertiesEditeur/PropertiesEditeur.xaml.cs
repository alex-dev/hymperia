using System;
using System.Windows;
using System.Windows.Controls;
using Hymperia.Facade.Constants;
using Hymperia.Facade.Extensions;
using Hymperia.Facade.ViewModels.Editeur;
using Prism;
using Prism.Ioc;
using Prism.Regions;

namespace Hymperia.Facade.Views.Editeur.PropertiesEditor
{
  public partial class PropertiesEditeur : UserControl, IActiveAware
  {
    #region Constructors

    public PropertiesEditeur(PropertiesEditeurViewModel context, IRegionManager manager, IContainerExtension container)
    {
      DataContext = context;
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

      ///////
      SpecificPropertiesRegion.Add(Container.Resolve<ConeEditor>(), ViewKeys.ConeEditor);
      SpecificPropertiesRegion.Add(Container.Resolve<CylindreEditor>(), ViewKeys.CylindreEditor);
      SpecificPropertiesRegion.Add(Container.Resolve<EllipsoideEditor>(), ViewKeys.EllipsoideEditor);
      SpecificPropertiesRegion.Add(Container.Resolve<PrismeEditor>(), ViewKeys.PrismeRectangulaireEditor);

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
