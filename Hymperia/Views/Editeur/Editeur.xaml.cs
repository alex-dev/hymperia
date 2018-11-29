using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Hymperia.Facade.Constants;
using Hymperia.Facade.Extensions;
using Hymperia.Facade.Views.Editeur.ProjetAnalyse;
using Hymperia.Model.Modeles;
using Prism;
using Prism.Commands;
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

    public static readonly DependencyProperty DroitProperty =
      DependencyProperty.Register(nameof(Droit), typeof(Acces.Droit), typeof(Editeur), new PropertyMetadata(OnDroitChanged));

    public static readonly DependencyPropertyKey BackActionProperty =
      DependencyProperty.RegisterReadOnly(nameof(BackAction), typeof(ICommand), typeof(Editeur), new PropertyMetadata(new DelegateCommand<Editeur>(BackAction)));

    #endregion

    public Projet Projet
    {
      get => (Projet)GetValue(ProjetProperty);
      set => SetValue(ProjetProperty, value);
    }

    public Acces.Droit Droit
    {
      get => (Acces.Droit)GetValue(DroitProperty);
      set => SetValue(DroitProperty, value);
    }

    #region Constructors

    public Editeur(IRegionManager manager, IContainerExtension container)
    {
      Manager = manager;
      Viewport = container.Resolve<Viewport>();
      FormesSelection = container.Resolve<FormesSelection>();
      MateriauxSelection = container.Resolve<MateriauxSelection>();
      PropertiesEditeur = container.Resolve<P.PropertiesEditeur>();
      MateriauxAnalyse = container.Resolve<MateriauxAnalyse>();

      Loaded += RegisterViews;
      InitializeComponent();

      SetBinding(ProjetProperty, new Binding(nameof(Projet)) { Source = DataContext, Mode = BindingMode.OneWayToSource });
      SetBinding(DroitProperty, new Binding(nameof(Droit)) { Source = DataContext, Mode = BindingMode.OneWayToSource });
    }

    #endregion

    #region Views Registration

    private void RegisterViews(object sender, RoutedEventArgs e)
    {
      Loaded -= RegisterViews;

      ViewportRegion = Manager.Regions[RegionKeys.ViewportRegion];
      HorizontalTabControl = Manager.Regions[RegionKeys.HorizontalTabControlRegion];
      VerticalTabControl = Manager.Regions[RegionKeys.VerticalTabControlRegion];

      ViewportRegion.Add(Viewport, ViewKeys.Viewport);
      RegisterViews();
    }

    private void RegisterViews()
    {
      if (!IsLoaded)
        return;

      HorizontalTabControl.RemoveAll();
      VerticalTabControl.RemoveAll();

      // PropertiesEditeur must be first so it is fully loaded.
      VerticalTabControl.Add(PropertiesEditeur, ViewKeys.PropertiesEditeur);
      VerticalTabControl.Add(MateriauxAnalyse, ViewKeys.MateriauxAnalyse);

      if (Droit >= Acces.Droit.LectureEcriture)
      {
        HorizontalTabControl.Add(FormesSelection, ViewKeys.FormesSelection);
        HorizontalTabControl.Add(MateriauxSelection, ViewKeys.MateriauxSelection);
      }
    }

    #endregion

    private static void OnDroitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
      (d as Editeur)?.RegisterViews();

    #region INavigationAware 

    private static void BackAction(Editeur editeur) =>
      editeur.Manager.Regions[RegionKeys.ContentRegion].NavigationService.Journal.GoBack();

    public bool IsNavigationTarget(NavigationContext context) =>
      context.Parameters[NavigationParameterKeys.Projet] is Projet && context.Parameters[NavigationParameterKeys.Acces] is Acces.Droit;

    public void OnNavigatedTo(NavigationContext context)
    {
      Projet = (Projet)context.Parameters[NavigationParameterKeys.Projet];
      Droit = (Acces.Droit)context.Parameters[NavigationParameterKeys.Acces];
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
      ViewportRegion?.Activate(ViewportRegion?.GetView(ViewKeys.Viewport));
      VerticalTabControl?.Activate(VerticalTabControl?.GetView(ViewKeys.PropertiesEditeur));

      if (HorizontalTabControl?.GetView(ViewKeys.FormesSelection) is object view)
        HorizontalTabControl?.Activate(view);
    }

    private void OnDeactivation()
    {
      ViewportRegion?.Deactivate();
      HorizontalTabControl?.Deactivate();
      VerticalTabControl?.Deactivate();
    }

    private bool isActive;

    #endregion

    #region Services

    private readonly IRegionManager Manager;
    private IRegion ViewportRegion;
    private IRegion HorizontalTabControl;
    private IRegion VerticalTabControl;

    #endregion

    #region Views

    private readonly Viewport Viewport;
    private readonly FormesSelection FormesSelection;
    private readonly MateriauxSelection MateriauxSelection;
    private readonly P.PropertiesEditeur PropertiesEditeur;
    private readonly MateriauxAnalyse MateriauxAnalyse;

    #endregion
  }

}
