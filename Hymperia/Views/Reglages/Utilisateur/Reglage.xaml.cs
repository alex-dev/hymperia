/*
 * Auteur : Antoine Mailhot
 * Date de création : 21 novembre 2018
*/

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Hymperia.Facade.Constants;
using Hymperia.Facade.Extensions;
using Prism;
using Prism.Ioc;
using Prism.Regions;

namespace Hymperia.Facade.Views.Reglages.Utilisateur
{
  /// <summary>
  /// Logique d'interaction pour Reglage.xaml
  /// </summary>
  public partial class Reglage : UserControl, INavigationAware, IActiveAware
  {
    #region Dependency Properties

    public static readonly DependencyProperty ProjetProperty =
      DependencyProperty.Register(nameof(Utilisateur), typeof(Model.Modeles.Utilisateur), typeof(Reglage));

    #endregion

    public Model.Modeles.Utilisateur Utilisateur
    {
      get => (Model.Modeles.Utilisateur)GetValue(ProjetProperty);
      set => SetValue(ProjetProperty, value);
    }

    #region Constructors

    public Reglage(IRegionManager manager, IContainerExtension container)
    {
      Container = container;
      Manager = manager;

      Loaded += RegisterViews;
      InitializeComponent();

      BindingOperations.SetBinding(this, ProjetProperty, new Binding(nameof(Utilisateur)) { Source = DataContext, Mode = BindingMode.OneWayToSource });
    }

    #endregion

    #region Views Registration

    private void RegisterViews(object sender, RoutedEventArgs e)
    {
      Loaded -= RegisterViews;

      ChangementMotDePasseRegion = Manager.Regions[RegionKeys.ChangementMotDePasseRegion];
      ConnexionAutomatiqueRegion = Manager.Regions[RegionKeys.ConnexionAutomatiqueRegion];

      ChangementMotDePasseRegion.Add(Container.Resolve<Model.Modeles.Utilisateur>(), ViewKeys.ChangementMotDePasse);
      ConnexionAutomatiqueRegion.Add(Container.Resolve<>(), ViewKeys.ConnexionAutomatique);
    }

    #endregion

    #region INavigationAware 

    public bool IsNavigationTarget(NavigationContext context) => context.Parameters[NavigationParameterKeys.Utilisateur] is Model.Modeles.Utilisateur;
    public void OnNavigatedTo(NavigationContext context) => Utilisateur = (Model.Modeles.Utilisateur)context.Parameters[NavigationParameterKeys.Utilisateur];
    public void OnNavigatedFrom(NavigationContext context) => Utilisateur = null;

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
      ConnexionAutomatiqueRegion?.Activate(ConnexionAutomatiqueRegion?.GetView(ViewKeys.ConnexionAutomatique));
    }

    private void OnDeactivation()
    {
      ChangementMotDePasseRegion?.Deactivate();
      ConnexionAutomatiqueRegion?.Deactivate();
    }

    private bool isActive;

    #endregion

    #region Services

    private readonly IContainerExtension Container;
    private readonly IRegionManager Manager;
    private IRegion ChangementMotDePasseRegion;
    private IRegion ConnexionAutomatiqueRegion;

    #endregion
  }
}
