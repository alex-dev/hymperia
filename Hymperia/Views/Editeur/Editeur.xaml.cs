using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Hymperia.Facade.Views.Editeur.ProjetAnalyse;
using Hymperia.Model.Modeles;
using Prism.Regions;

namespace Hymperia.Facade.Views.Editeur
{
  public partial class Editeur : UserControl, INavigationAware
  {
    #region Dependency Properties

    public static readonly DependencyProperty ProjetProperty =
      DependencyProperty.Register("Projet", typeof(Projet), typeof(Editeur));

    #endregion

    public Projet Projet
    {
      get => (Projet)GetValue(ProjetProperty);
      set => SetValue(ProjetProperty, value);
    }

    #region Constructors

    public Editeur(IRegionManager manager)
    {
      RegisterViews(manager);
      InitializeComponent();
      BindingOperations.SetBinding(this, ProjetProperty, new Binding("Projet") { Source = DataContext, Mode = BindingMode.OneWayToSource });
    }

    #endregion

    #region Views Registration

    private void RegisterViews(IRegionManager manager)
    {
      manager.RegisterViewWithRegion("FormesSelectionRegion", typeof(FormesSelection));
      manager.RegisterViewWithRegion("MateriauxSelectionRegion", typeof(MateriauxSelection));
      manager.RegisterViewWithRegion("ViewportRegion", typeof(Viewport));
      manager.RegisterViewWithRegion("PrixAnalyseRegion", typeof(MateriauxAnalyse));
      //manager.RegisterViewWithRegion("FormesPropertiesRegion", typeof(FormesProperties));
    }

    #endregion

    #region INavigationAware 

    public bool IsNavigationTarget(NavigationContext context) => context.Parameters["Projet"] is Projet;
    public void OnNavigatedTo(NavigationContext context) => Projet = (Projet)context.Parameters["Projet"];
    public void OnNavigatedFrom(NavigationContext context) => Projet = null;

    #endregion
  }
}
