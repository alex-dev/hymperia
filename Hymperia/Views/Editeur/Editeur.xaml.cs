using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Hymperia.Facade.Constants;
using Hymperia.Facade.Extensions;
using Hymperia.Facade.Views.Editeur.ProjetAnalyse;
using Hymperia.Facade.Views.Editeur.PropertiesEditor;
using Hymperia.Model.Modeles;
using Prism;
using Prism.Ioc;
using Prism.Regions;

namespace Hymperia.Facade.Views.Editeur
{
  public partial class Editeur : UserControl, INavigationAware, IActiveAware
  {
    #region Dependency Properties

    public static readonly DependencyProperty ProjetProperty =
      DependencyProperty.Register(nameof(Projet), typeof(Projet), typeof(Editeur));

    #endregion

    public Projet Projet
    {
      get => (Projet)GetValue(ProjetProperty);
      set => SetValue(ProjetProperty, value);
    }

    #region Constructors

    public Editeur(IRegionManager manager, IContainerExtension container)
    {
      Container = container;
      Manager = manager;

      Loaded += RegisterViews;
      InitializeComponent();

      BindingOperations.SetBinding(this, ProjetProperty, new Binding(nameof(Projet)) { Source = DataContext, Mode = BindingMode.OneWayToSource });
    }

    #endregion
    
    #region Views Registration

    private void RegisterViews(object sender, RoutedEventArgs e)
    {
      Loaded -= RegisterViews;

      ViewportRegion = Manager.Regions[RegionKeys.ViewportRegion];
      FormesSelectionRegion = Manager.Regions[RegionKeys.FormesSelectionRegion];
      MateriauxSelectionRegion = Manager.Regions[RegionKeys.MateriauxSelectionRegion];
      ProjetAnalyseRegion = Manager.Regions[RegionKeys.ProjetAnalyseRegion];
      FormesPropertiesRegion = Manager.Regions[RegionKeys.FormesPropertiesRegion];

      ViewportRegion.Add(Container.Resolve<Viewport>(), ViewKeys.Viewport);
      FormesSelectionRegion.Add(Container.Resolve<FormesSelection>(), ViewKeys.FormesSelection);
      MateriauxSelectionRegion.Add(Container.Resolve<MateriauxSelection>(), ViewKeys.MateriauxSelection);
      ProjetAnalyseRegion.Add(Container.Resolve<MateriauxAnalyse>(), ViewKeys.MateriauxAnalyse);
      FormesPropertiesRegion.Add(Container.Resolve<PropertiesEditeur>(), ViewKeys.PropertiesEditeur);
    }

    #endregion

    #region INavigationAware 

    public bool IsNavigationTarget(NavigationContext context) => context.Parameters[NavigationParameterKeys.Projet] is Projet;
    public void OnNavigatedTo(NavigationContext context) => Projet = (Projet)context.Parameters[NavigationParameterKeys.Projet];
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
      ViewportRegion?.Activate(ViewportRegion?.GetView(ViewKeys.Viewport));
      FormesSelectionRegion?.Activate(FormesSelectionRegion?.GetView(ViewKeys.FormesSelection));
      MateriauxSelectionRegion?.Activate(MateriauxSelectionRegion?.GetView(ViewKeys.MateriauxSelection));
      ProjetAnalyseRegion?.Activate(ProjetAnalyseRegion?.GetView(ViewKeys.MateriauxAnalyse));
      FormesPropertiesRegion?.Activate(FormesPropertiesRegion?.GetView(ViewKeys.PropertiesEditeur));
    }

    private void OnDeactivation()
    {
      ViewportRegion?.Deactivate();
      FormesSelectionRegion?.Deactivate();
      MateriauxSelectionRegion?.Deactivate();
      ProjetAnalyseRegion?.Deactivate();
      FormesPropertiesRegion?.Deactivate();
    }

    private bool isActive;

    #endregion

    #region Services

    private readonly IContainerExtension Container;
    private readonly IRegionManager Manager;
    private IRegion ViewportRegion;
    private IRegion FormesSelectionRegion;
    private IRegion MateriauxSelectionRegion;
    private IRegion ProjetAnalyseRegion;
    private IRegion FormesPropertiesRegion;

    #endregion
  }
}
