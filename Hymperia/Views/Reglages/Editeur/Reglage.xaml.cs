/*
 * Auteur : Antoine Mailhot
 * Date de création : 28 novembre 2018
*/

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Hymperia.Facade.Constants;
using Hymperia.Facade.Extensions;
using Hymperia.Model.Modeles;
using Prism;
using Prism.Ioc;
using Prism.Regions;

namespace Hymperia.Facade.Views.Reglages.Editeur
{
  public partial class Reglage : UserControl, INavigationAware, IActiveAware
  {
    #region Dependency Properties

    public static readonly DependencyProperty ProjetProperty =
      DependencyProperty.Register(nameof(Projet), typeof(Projet), typeof(Reglage));

    #endregion

    public Projet Projet
    {
      get => (Projet)GetValue(ProjetProperty);
      set => SetValue(ProjetProperty, value);
    }

    #region Constructors

    public Reglage(IRegionManager manager, IContainerExtension container)
    {
      Manager = manager;

      Loaded += RegisterViews;
      InitializeComponent();

      SetBinding(ProjetProperty, new Binding(nameof(Projet)) { Source = DataContext, Mode = BindingMode.OneWayToSource });
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

    #region INavigationAware 

    public bool IsNavigationTarget(NavigationContext context) => context.Parameters[NavigationParameterKeys.Projet] is Projet;
    public void OnNavigatedTo(NavigationContext context)
    {
      void Set() =>
        Projet = (Projet)context.Parameters[NavigationParameterKeys.Projet];

      void Load(object sender, RoutedEventArgs e)
      {
        Set();
        Loaded -= Load;
      }

      if (IsLoaded)
        Set();
      else
        Loaded += Load;
    }
    public void OnNavigatedFrom(NavigationContext context) => Projet = null;

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
