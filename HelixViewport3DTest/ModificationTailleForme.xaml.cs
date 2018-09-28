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
  /// Logique d'interaction pour ModificationTailleForme.xaml
  /// </summary>
  public partial class ModificationTailleForme : UserControl
  {
    public ObservableCollection<ModelVisual3D> Formes { get; set; }
    public double Taille { get; set; }

    public ModificationTailleForme()
    {
      InitializeComponent();
      viewport.Children.Add(new GridLinesVisual3D());
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
      }
    }

    private int i = 5;

    private ModelVisual3D GenererFormeAleatoire()
    {
      switch (new Random().Next(8,8))
      {
        case 0: return new ArrowVisual3D { Diameter = Taille };
        break;
        case 1: return new BoxVisual3D { Width = Taille, Height = Taille, Length = Taille };
        break;
        case 2: return new CubeVisual3D { SideLength = Taille };
        break;
        case 3: return new EllipsoidVisual3D { RadiusX = Taille, RadiusY = Taille, RadiusZ = Taille };
        break;
        case 4: return new HelixVisual3D { Diameter = Taille, Turns = Taille, Length = Taille, Radius = Taille };
        break;
        case 5: return new PieSliceVisual3D { OuterRadius = Taille, InnerRadius = Taille };
        break;
        case 6: return new PipeVisual3D { Diameter = Taille, InnerDiameter = Taille };
        break;
        case 7: return new RectangleVisual3D { Width = Taille, Length = Taille };
        break;
        case 8: return new SphereVisual3D { Radius = Taille };
        break;
        case 9: return new TruncatedConeVisual3D { Height = Taille };
        break;
        case 10: return new TubeVisual3D { Diameter = Taille };
        break;
        default: return null;
          break;
      }
    }

    private void AjouterItem(object sender, RoutedEventArgs e)
    {Formes.Add(GenererFormeAleatoire());}

    private void Modifierdimension(object sender, RoutedEventArgs e)
    {
      //Formes.OfType<ArrowVisual3D>().First().Diameter = Taille;

      //Formes.OfType<BoxVisual3D>().First().Width = Taille;
      //Formes.OfType<BoxVisual3D>().First().Height = Taille;
      //Formes.OfType<BoxVisual3D>().First().Length = Taille;

      //Formes.OfType<CubeVisual3D>().First().SideLength = Taille;

      //Formes.OfType<EllipsoidVisual3D>().First().RadiusX = Taille;
      //Formes.OfType<EllipsoidVisual3D>().First().RadiusY = Taille;
      //Formes.OfType<EllipsoidVisual3D>().First().RadiusZ = Taille;

      //Formes.OfType<HelixVisual3D>().First().Diameter = Taille;
      //Formes.OfType<HelixVisual3D>().First().Turns = Taille;
      //Formes.OfType<HelixVisual3D>().First().Length = Taille;
      //Formes.OfType<HelixVisual3D>().First().Radius = Taille;

      //Formes.OfType<PieSliceVisual3D>().First().OuterRadius = Taille;
      //Formes.OfType<PieSliceVisual3D>().First().InnerRadius = Taille;

      //Formes.OfType<PipeVisual3D>().First().Diameter = Taille;
      //Formes.OfType<PipeVisual3D>().First().InnerDiameter = Taille;

      //Formes.OfType<RectangleVisual3D>().First().Width = Taille;
      //Formes.OfType<RectangleVisual3D>().First().Length = Taille;

      Formes.OfType<SphereVisual3D>().First().Radius = Taille;

      //Formes.OfType<TruncatedConeVisual3D>().First().Height = Taille;

      //Formes.OfType<TubeVisual3D>().First().Diameter = Taille;
    }

    private void SupprimerItem(object sender, RoutedEventArgs e)
    {
      Formes.Remove(Formes.Last());
    }
  }
}
