﻿using System.Windows;
using System.Windows.Controls;
using HelixToolkit.Wpf;

namespace HelixViewport3DTest
{
  public partial class MainWindow : Window
  {
    private UserControl Control { get; set; }

    public MainWindow()
    {
      InitializeComponent();
    }

    private void PlaceUserControl(UserControl control)
    {
      grid.Children.Remove(Control);
      Grid.SetColumn(control, 1);
      grid.Children.Add(control);
      Control = control;
    }

    private void AddShapeOnCursor_Click(object sender, RoutedEventArgs e)
    {
      PlaceUserControl(new PlaceRandomShapes());
    }

    private void MaterialExploration_Click(object sender, RoutedEventArgs e)
    {
      PlaceUserControl(new FormBrushesExploration());
    }

    private void SpherePhiThetaExploration_Click(object sender, RoutedEventArgs e)
    {
        PlaceUserControl(new SpherePhiThetaExploration());
    }

    private void ManipulatorExploration_Click(object sender, RoutedEventArgs e)
    {
      PlaceUserControl(new ManipulatorExploration());
    }

    private void CombinedManipulatorExploration_Click(object sender, RoutedEventArgs e)
    {
      PlaceUserControl(new CombinedManipulatorExploration());
    }

    private void SelectionExploration_Click(object sender, RoutedEventArgs e)
    {
      PlaceUserControl(new SelectionExploration());
    }

    private void DeplacementForme_Click(object sender, RoutedEventArgs e)
    {
      PlaceUserControl(new DeplacementForme());
    }

    private void RotationForme_Click(object sender, RoutedEventArgs e)
    {
      PlaceUserControl(new RotationForme());
    }

    private void DataBinding_Click(object sender, RoutedEventArgs e)
    {
      PlaceUserControl(new DataBindingFormes());
    }

    private void ModificationTailleForme_Click(object sender, RoutedEventArgs e)
    {
      PlaceUserControl(new ModificationTailleForme());
    }
  }
}
