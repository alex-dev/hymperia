using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Hymperia.Facade.Constants;
using Hymperia.Facade.Extensions;
using Hymperia.Facade.Views.Editeur.ProjetAnalyse;
using Hymperia.Model.Modeles;
using Prism;
using Prism.Ioc;
using Prism.Regions;
using P = Hymperia.Facade.Views.Editeur.PropertiesEditeur;

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

      SetBinding(ProjetProperty, new Binding(nameof(Projet)) { Source = DataContext, Mode = BindingMode.OneWayToSource });
    }

    #endregion

    #region Views Registration

    private void RegisterViews(object sender, RoutedEventArgs e)
    {
      Loaded -= RegisterViews;

      ViewportRegion = Manager.Regions[RegionKeys.ViewportRegion];
      HorizontalTabControl = Manager.Regions[RegionKeys.HorizontalTabControlRegion];
      VerticalTabControl = Manager.Regions[RegionKeys.VerticalTabControlRegion];


      ViewportRegion.Add(Container.Resolve<Viewport>(), ViewKeys.Viewport);
      HorizontalTabControl.Add(Container.Resolve<FormesSelection>(), ViewKeys.FormesSelection);
      HorizontalTabControl.Add(Container.Resolve<MateriauxSelection>(), ViewKeys.MateriauxSelection);
      VerticalTabControl.Add(Container.Resolve<MateriauxAnalyse>(), ViewKeys.MateriauxAnalyse);
      VerticalTabControl.Add(Container.Resolve<Editor>(), ViewKeys.PropertiesEditor);

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
      HorizontalTabControl?.Activate(HorizontalTabControl?.GetView(ViewKeys.FormesSelection));
      VerticalTabControl?.Activate(VerticalTabControl?.GetView(ViewKeys.PropertiesEditor));
    }

    private void OnDeactivation()
    {
      ViewportRegion?.Deactivate();
      HorizontalTabControl?.Deactivate();
      VerticalTabControl?.Deactivate();
      //ProjetAnalyseRegion?.Deactivate();
      //FormesPropertiesRegion?.Deactivate();
    }

    private bool isActive;

    #endregion

    #region Services

    private readonly IContainerExtension Container;
    private readonly IRegionManager Manager;
    private IRegion ViewportRegion;
    private IRegion HorizontalTabControl;
    private IRegion VerticalTabControl;

    #endregion

  }
}
