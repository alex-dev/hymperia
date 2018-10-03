﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Prism.Regions;
using Hymperia.Model.Modeles;

namespace Hymperia.Facade.Views.Editeur
{
  public partial class Editeur : UserControl, INavigationAware
  {
    public static readonly DependencyProperty ProjetProperty;

    public Projet Projet
    {
      get => (Projet)GetValue(ProjetProperty);
      set => SetValue(ProjetProperty, value);
    }

    static Editeur()
    {
      ProjetProperty = DependencyProperty.Register("Projet", typeof(Projet), typeof(Editeur));
    }

    public Editeur(IRegionManager manager)
    {
      manager.RegisterViewWithRegion("ViewportRegion", typeof(Viewport));
      InitializeComponent();
      BindingOperations.SetBinding(this, ProjetProperty, new Binding("Projet") { Source = DataContext, Mode = BindingMode.OneWayToSource });
    }

    public bool IsNavigationTarget(NavigationContext context) => true;
    public void OnNavigatedTo(NavigationContext context) => Projet = (context.Parameters["Projet"] as Projet);
    public void OnNavigatedFrom(NavigationContext context) => Projet = null;
  }
}
