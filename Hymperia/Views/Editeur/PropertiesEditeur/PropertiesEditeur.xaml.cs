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
      Manager = manager;
      PositionEditeur = container.Resolve<PositionEditeur>();
      ConeEditeur = container.Resolve<ConeEditeur>();
      CylindreEditeur = container.Resolve<CylindreEditeur>();
      EllipsoideEditeur = container.Resolve<EllipsoideEditeur>();
      PrismeEditeur = container.Resolve<PrismeEditeur>();

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

      PositionPropertiesRegion.Add(PositionEditeur, ViewKeys.PositionEditeur);
      SpecificPropertiesRegion.Add(ConeEditeur, ViewKeys.ConeEditor);
      SpecificPropertiesRegion.Add(CylindreEditeur, ViewKeys.CylindreEditor);
      SpecificPropertiesRegion.Add(EllipsoideEditeur, ViewKeys.EllipsoideEditor);
      SpecificPropertiesRegion.Add(PrismeEditeur, ViewKeys.PrismeRectangulaireEditor);

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

    private readonly IRegionManager Manager;
    private readonly PositionEditeur PositionEditeur;
    private readonly ConeEditeur ConeEditeur;
    private readonly CylindreEditeur CylindreEditeur;
    private readonly EllipsoideEditeur EllipsoideEditeur;
    private readonly PrismeEditeur PrismeEditeur;
    private IRegion PositionPropertiesRegion;
    private IRegion SpecificPropertiesRegion;

      #endregion
   }
}
