using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HelixToolkit.Wpf;

namespace HelixViewport3DTest
{
  /// <summary>
  /// Logique d'interaction pour DataBinding.xaml
  /// </summary>
  public partial class DataBindingFormes : UserControl
  {
    public ObservableCollection<ModelVisual3D> Formes { get; set; }
    public SphereVisual3D Sphere { get; set; } 
    public Random Aleatoire { get; set; }

    public DataBindingFormes()
    {
      InitializeComponent();
      Aleatoire = new Random();
      Formes = new ObservableCollection<ModelVisual3D>();
      Formes.CollectionChanged += BindingFormes;
    }

    public void BindingFormes(object sender, NotifyCollectionChangedEventArgs e)
    {
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          foreach (Visual3D item in e.NewItems)
          {
            viewport.Children.Add(item);
          }
          break;
        case NotifyCollectionChangedAction.Remove:
          foreach (Visual3D item in e.OldItems)
          {
            viewport.Children.Remove(item);
          }
          break;
        case NotifyCollectionChangedAction.Replace:
          foreach (Visual3D item in e.OldItems)
          {
            viewport.Children.Remove(item);
          }
          foreach (Visual3D item in e.NewItems)
          {
            viewport.Children.Add(item);
          }
          break;
        case NotifyCollectionChangedAction.Move:

          break;
        case NotifyCollectionChangedAction.Reset:
          viewport.Children.Clear();
          viewport.Children.Add(new SunLight());
          viewport.Children.Add(new GridLinesVisual3D { Width = 100, Length = 100 });
          break;
        default:
          break;
      }
    }

    private int i = 5;

    private void AjouterSphere(object sender, RoutedEventArgs e)
    {
      var sphere = new SphereVisual3D { Radius = i++ };
      Formes.Add(sphere);
    }

    private void Modifierdimension(object sender, RoutedEventArgs e)
    {
      Formes.OfType<SphereVisual3D>().First().Radius = Aleatoire.Next(5, 20);
    }

    private void SupprimerSphere(object sender, RoutedEventArgs e)
    {
      Formes.Remove(Formes.Last());
    }

    private void ReplaceSphere(object sender, RoutedEventArgs e)
    {
      Formes[0] = new SphereVisual3D();
    }

    private void MoveSphere(object sender, RoutedEventArgs e)
    {
      Formes.Move(0, Formes.Count - 2);
    }

    private void ResetSphere(object sender, RoutedEventArgs e)
    {
      Formes.Clear();
    }
  }
}
