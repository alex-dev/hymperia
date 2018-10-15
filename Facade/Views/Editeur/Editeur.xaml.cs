using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Prism.Regions;
using Hymperia.Model.Modeles;
using System;
using Hymperia.Facade.ViewModels.Editeur;

namespace Hymperia.Facade.Views.Editeur
{
  public partial class Editeur : UserControl, INavigationAware
  {
    #region Dependency Properties

    public static readonly DependencyProperty ProjetProperty;

    #endregion

    public Projet Projet
    {
      get => (Projet)GetValue(ProjetProperty);
      set => SetValue(ProjetProperty, value);
    }

    #region Constructors

    static Editeur()
    {
      ProjetProperty = DependencyProperty.Register("Projet", typeof(Projet), typeof(Editeur), new PropertyMetadata(ProjetChanged));
    }

    public Editeur(IRegionManager manager)
    {
      RegisterViews(manager);
      InitializeComponent();

      BindingOperations.SetBinding(this, ProjetProperty, new Binding("Projet") { Source = DataContext, Mode = BindingMode.OneWayToSource, NotifyOnSourceUpdated = true });
    }

    #endregion

    #region Views Registration

    private void RegisterViews(IRegionManager manager)
    {
      manager.RegisterViewWithRegion("ViewportRegion", typeof(Viewport));
      manager.RegisterViewWithRegion("FormesSelectionRegion", typeof(FormesSelection));
      manager.RegisterViewWithRegion("MateriauxSelectionRegion", typeof(MateriauxSelection));
      manager.RegisterViewWithRegion("PrixAnalyseRegion", typeof(PrixAnalyseFormes));
      manager.RegisterViewWithRegion("FormesPropertiesRegion", typeof(FormesProperties));
      manager.RegisterViewWithRegion("SelectionModeRegion", typeof(SelectionMode));
    }

    #endregion

    #region Projet Changes

    private static void ProjetChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args) =>
      ((Editeur)sender).ProjetChanged(args);

    private void ProjetChanged(DependencyPropertyChangedEventArgs args)
    {
      if (DataContext is EditeurViewModel context && context.Revert.CanExecute(null))
      {
        context.Revert.Execute(null);
      }
    }

    #endregion

    #region INavigationAware 

    public bool IsNavigationTarget(NavigationContext context) => true;
    public void OnNavigatedTo(NavigationContext context) => Projet = (Projet)context.Parameters["Projet"];
    public void OnNavigatedFrom(NavigationContext context) => Projet = null;

    #endregion
  }
}
