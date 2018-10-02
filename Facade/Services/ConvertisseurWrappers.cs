using System;
using System.Collections.ObjectModel;
using System.Windows.Data;
using HelixToolkit.Wpf;
using JetBrains.Annotations;
using Hymperia.Model.Modeles;
using Hymperia.Facade.ModelWrappers;

namespace Hymperia.Facade.Services
{
  public class ConvertisseurWrappers
  {
    #region Services

    [NotNull]
    private readonly TransformConverter Converter;

    #endregion

    public ConvertisseurWrappers([NotNull] TransformConverter converter)
    {
      Converter = converter;
    }

    public MeshElement3D ConvertirLier(FormeWrapper<Forme> wrapper)
    {
      var forme = Convertir(wrapper);
      return Lier(forme, wrapper);
    }

    #region Convertir

    /// <summary>Convertit <paramref name="forme"/> en <see cref="MeshElement3D"/>.</summary>
    /// <exception cref="ArgumentException"><paramref name="forme"/> n'était pas une <see cref="Forme"/> connue.</exception>
    [NotNull]
    public MeshElement3D Convertir([NotNull] FormeWrapper forme)
    {
      switch (forme)
      {
        case Cone cone:
          return new ConeWrapper(forme);
        case Cylindre cylindre:
          return Convertir(cylindre);
        case Ellipsoide ellipsoide:
          return Convertir(ellipsoide);
        case PrismeRectangulaire prisme:
          return Convertir(prisme);
        default:
          throw new ArgumentException($"Unknown child of { nameof(FormeWrapper) }.", nameof(forme));
      }*/
      }
    }

    private TruncatedConeVisual3D Convertir(Cone forme)
    {
      var final = new TruncatedConeVisual3D
      {
        Origin = forme.Origine.Convert(),
        Height = forme.Hauteur,
        BaseRadius = forme.RayonBase,
        TopRadius = forme.RayonTop,
        ThetaDiv = forme.ThetaDiv
      };

      return Materialize(final, forme);
    }

    private PipeVisual3D Convertir(Cylindre forme)
    {
      var final = new PipeVisual3D
      {
        Point1 = forme.Origine.Convert(),
        Point2 = forme.Point.Convert(),
        Diameter = forme.Diametre,
        InnerDiameter = forme.InnerDiametre,
        ThetaDiv = forme.ThetaDiv
      };

      return Materialize(final, forme);
    }

    private EllipsoidVisual3D Convertir(Ellipsoide forme)
    {
      var final = new EllipsoidVisual3D
      {
        Center = forme.Origine.Convert(),
        RadiusX = forme.RayonX,
        RadiusY = forme.RayonY,
        RadiusZ = forme.RayonZ,
        PhiDiv = forme.PhiDiv,
        ThetaDiv = forme.ThetaDiv
      };

      return Materialize(final, forme);
    }

    private BoxVisual3D Convertir(PrismeRectangulaire forme)
    {
      var final = new BoxVisual3D
      {
        Center = forme.Origine.Convert(),
        Height = forme.Hauteur,
        Length = forme.Longueur,
        Width = forme.Largeur,
      };

      return Materialize(final, forme);
    }

    private TFinal Materialize<TFinal, TInitial>(TFinal final, TInitial initial )
      where TFinal : MeshElement3D
      where TInitial : Forme
    {
      return final;
    }

    #endregion

    #region Lier

    /// <summary>DataBind <paramref name="forme"/> selon <paramref name="path"/>.</summary>
    /// <exception cref="InvalidCastException"><paramref name="source"/> était une <see cref="Forme"/> incompatible avec <paramref name="forme"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="forme"/> était un <see cref="MeshElement3D"/> inconnu.</exception>
    [NotNull]
    public MeshElement3D Lier([NotNull] MeshElement3D forme, [NotNull] FormeWrapper source)
    {
      throw new NotImplementedException();
      /*switch (forme)
      {
        case TruncatedConeVisual3D cone:
          return Lier(cone, (ConeWrapper)source);
        // case PipeVisual3D cylindre:
        //   return Lier(cylindre, (CylindreWrapper)source);
        case EllipsoidVisual3D ellipsoide:
          return Lier(ellipsoide, (EllipsoideWrapper)source);
        case BoxVisual3D prisme:
          return Lier(prisme, (PrismeRectangulaire)source);
        default:
          throw new ArgumentException($"Unknown child of { nameof(MeshElement3D) }.", nameof(forme));
      }*/
    }

    private TruncatedConeVisual3D Lier(TruncatedConeVisual3D forme, Cone source)
    {
      var bindings = new MultiBinding() { Converter = Converter, Mode = BindingMode.TwoWay };
      bindings.Bindings.AddRange(new Binding[]
      {
        new Binding("Origine") { Source = source },
        new Binding("Quaternion") { Source = source }
      });

      BindingOperations.SetBinding(forme, TruncatedConeVisual3D.TransformProperty, bindings);
      BindingOperations.SetBinding(forme, TruncatedConeVisual3D.HeightProperty, new Binding("Hauteur") { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, TruncatedConeVisual3D.BaseRadiusProperty, new Binding("RayonBase") { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, TruncatedConeVisual3D.TopRadiusProperty, new Binding("RayonTop") { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, TruncatedConeVisual3D.ThetaDivProperty, new Binding("ThetaDiv") { Source = source, Mode = BindingMode.TwoWay });

      return forme;
    }

    private PipeVisual3D Lier(PipeVisual3D forme, Cylindre source)
    {
      //BindingOperations.SetBinding(forme, PipeVisual3D.Point1Property, new Binding("Origine") { Converter = PointValueConverter });
      //BindingOperations.SetBinding(forme, PipeVisual3D.Point2Property, new Binding("Point") { Converter = PointValueConverter });
      BindingOperations.SetBinding(forme, PipeVisual3D.DiameterProperty, new Binding("Diametre") { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, PipeVisual3D.InnerDiameterProperty, new Binding("InnerDiametre") { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, PipeVisual3D.ThetaDivProperty, new Binding("ThetaDiv") { Source = source, Mode = BindingMode.TwoWay });

      return forme;
    }

    private EllipsoidVisual3D Lier(EllipsoidVisual3D forme, Ellipsoide source)
    {
      var bindings = new MultiBinding() { Converter = Converter, Mode = BindingMode.TwoWay };
      bindings.Bindings.AddRange(new Binding[]
      {
        new Binding("Origine") { Source = source },
        new Binding("Quaternion") { Source = source }
      });

      BindingOperations.SetBinding(forme, EllipsoidVisual3D.TransformProperty, bindings);
      BindingOperations.SetBinding(forme, EllipsoidVisual3D.RadiusXProperty, new Binding("RayonX") { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, EllipsoidVisual3D.RadiusYProperty, new Binding("RayonY") { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, EllipsoidVisual3D.RadiusZProperty, new Binding("RayonZ") { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, EllipsoidVisual3D.PhiDivProperty, new Binding("PhiDiv") { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, EllipsoidVisual3D.ThetaDivProperty, new Binding("ThetaDiv") { Source = source, Mode = BindingMode.TwoWay });

      return forme;
    }

    private BoxVisual3D Lier(BoxVisual3D forme, PrismeRectangulaire source)
    {
      var bindings = new MultiBinding() { Converter = Converter, Mode = BindingMode.TwoWay };
      bindings.Bindings.AddRange(new Binding[]
      {
        new Binding("Origine") { Source = source },
        new Binding("Quaternion") { Source = source }
      });

      BindingOperations.SetBinding(forme, BoxVisual3D.TransformProperty, bindings);
      BindingOperations.SetBinding(forme, BoxVisual3D.HeightProperty, new Binding("Hauteur") { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, BoxVisual3D.LengthProperty, new Binding("Longueur") { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, BoxVisual3D.WidthProperty, new Binding("Largeur") { Source = source, Mode = BindingMode.TwoWay });

      return forme;
    }

    #endregion
  }
}
