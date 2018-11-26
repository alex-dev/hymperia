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
using Hymperia.Model.Modeles;
using Prism;
using Prism.Ioc;
using Prism.Regions;

namespace Hymperia.Facade.Views.Reglages.Application
{
  public partial class Reglage : UserControl, INavigationAware, IActiveAware
  {
    #region Dependency Properties

    public static readonly DependencyProperty UtilisateurProperty =
      DependencyProperty.Register(nameof(Utilisateur), typeof(Utilisateur), typeof(Reglage));

    #endregion

    public Utilisateur Utilisateur
    {
      get => (Utilisateur)GetValue(UtilisateurProperty);
      set => SetValue(UtilisateurProperty, value);
    }

    #region Constructors

    public Reglage(IRegionManager manager, IContainerExtension container)
    {
      Manager = manager;
      ChangementMotDePasse = container.Resolve<ChangementMotDePasse>();
      ConnexionAutomatique = container.Resolve<ConnexionAutomatique>();

      Loaded += RegisterViews;
      InitializeComponent();

      SetBinding(UtilisateurProperty, new Binding(nameof(Utilisateur)) { Source = DataContext, Mode = BindingMode.OneWayToSource });
    }

    #endregion

    #region Views Registration

    private void RegisterViews(object sender, RoutedEventArgs e)
    {
      Loaded -= RegisterViews;

      ChangementMotDePasseRegion = Manager.Regions[RegionKeys.ChangementMotDePasseRegion];
      ConnexionAutomatiqueRegion = Manager.Regions[RegionKeys.ConnexionAutomatiqueRegion];

      ChangementMotDePasseRegion.Add(ChangementMotDePasse, ViewKeys.ChangementMotDePasse);
      ConnexionAutomatiqueRegion.Add(ConnexionAutomatique, ViewKeys.ConnexionAutomatique);
    }

    #endregion

    #region INavigationAware 

    public bool IsNavigationTarget(NavigationContext context) => context.Parameters[NavigationParameterKeys.Utilisateur] is Utilisateur;
    public void OnNavigatedTo(NavigationContext context)
    {
      void Set() =>
        Utilisateur = (Utilisateur)context.Parameters[NavigationParameterKeys.Utilisateur];

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

    private readonly IRegionManager Manager;
    private IRegion ChangementMotDePasseRegion;
    private IRegion ConnexionAutomatiqueRegion;

    #endregion

    #region Views

    private readonly ChangementMotDePasse ChangementMotDePasse;
    private readonly ConnexionAutomatique ConnexionAutomatique;

    #endregion
  }
}
