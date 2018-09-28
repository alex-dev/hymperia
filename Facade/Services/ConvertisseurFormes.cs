using System;
using System.Windows.Data;
using HelixToolkit.Wpf;
using JetBrains.Annotations;
using Hymperia.Model.Modeles;

namespace Hymperia.Facade.Services
{
  public class ConvertisseurFormes
  {
    #region Services

    [NotNull]
    private PointValueConverter PointValueConverter { get; set; }

    #endregion

    public ConvertisseurFormes([NotNull] PointValueConverter point)
    {
      PointValueConverter = point;
    }

    #region Convertir

    /// <summary>Convertit <paramref name="forme"/> en <see cref="MeshElement3D"/>.</summary>
    /// <exception cref="ArgumentException"><paramref name="forme"/> n'était pas une <see cref="Forme"/> connue.</exception>
    [NotNull]
    public MeshElement3D Convertir([NotNull] Forme forme)
    {
      switch (forme)
      {
        case Cone cone:
          return Convertir(cone);
        case Cylindre cylindre:
          return Convertir(cylindre);
        case Ellipsoide ellipsoide:
          return Convertir(ellipsoide);
        case PrismeRectangulaire prisme:
          return Convertir(prisme);
        default:
          throw new ArgumentException("Unknown child of Form.", "forme");
      }
    }

    private TruncatedConeVisual3D Convertir(Cone forme)
    {
      var final = new TruncatedConeVisual3D
      {
        Origin = forme.Origine.Object.Convert(),
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
        Point1 = forme.Point1.Object.Convert(),
        Point2 = forme.Point2.Object.Convert(),
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
        Center = forme.Centre.Object.Convert(),
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
        Center = forme.Centre.Object.Convert(),
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
    /// <exception cref="ArgumentNullException"><paramref name="path"/> était <see cref="null"/> ou whitespace.</exception>
    /// <exception cref="ArgumentException"><paramref name="forme"/> était un <see cref="MeshElement3D"/> inconnu.</exception>
    [NotNull]
    public MeshElement3D Lier([NotNull] MeshElement3D forme, [NotNull] string path)
    {
      if (string.IsNullOrWhiteSpace(path))
      {
        throw new ArgumentNullException("path");
      }

      switch (forme)
      {
        case TruncatedConeVisual3D cone:
          return Lier(cone, path);
        case PipeVisual3D cylindre:
          return Lier(cylindre, path);
        case EllipsoidVisual3D ellipsoide:
          return Lier(ellipsoide, path);
        case BoxVisual3D prisme:
          return Lier(prisme, path);
        default:
          throw new ArgumentException("Unknown child of MeshElement3D.", "forme");
      }
    }

    private TruncatedConeVisual3D Lier(TruncatedConeVisual3D forme, string path)
    {
      BindingOperations.SetBinding(forme, TruncatedConeVisual3D.OriginProperty, new Binding($"{ path }.Origine") { Converter = PointValueConverter });
      BindingOperations.SetBinding(forme, TruncatedConeVisual3D.HeightProperty, new Binding($"{ path }.Hauteur"));
      BindingOperations.SetBinding(forme, TruncatedConeVisual3D.BaseRadiusProperty, new Binding($"{ path }.RayonBase"));
      BindingOperations.SetBinding(forme, TruncatedConeVisual3D.TopRadiusProperty, new Binding($"{ path }.RayonTop"));
      BindingOperations.SetBinding(forme, TruncatedConeVisual3D.ThetaDivProperty, new Binding($"{ path }.ThetaDiv"));

      return forme;
    }

    private PipeVisual3D Lier(PipeVisual3D forme, string path)
    {
      BindingOperations.SetBinding(forme, PipeVisual3D.Point1Property, new Binding($"{ path }.Point1") { Converter = PointValueConverter });
      BindingOperations.SetBinding(forme, PipeVisual3D.Point2Property, new Binding($"{ path }.Point2") { Converter = PointValueConverter });
      BindingOperations.SetBinding(forme, PipeVisual3D.DiameterProperty, new Binding($"{ path }.Diametre"));
      BindingOperations.SetBinding(forme, PipeVisual3D.InnerDiameterProperty, new Binding($"{ path }.InnerDiametre"));
      BindingOperations.SetBinding(forme, PipeVisual3D.ThetaDivProperty, new Binding($"{ path }.ThetaDiv"));

      return forme;
    }

    private EllipsoidVisual3D Lier(EllipsoidVisual3D forme, string path)
    {
      BindingOperations.SetBinding(forme, EllipsoidVisual3D.CenterProperty, new Binding($"{ path }.Centre") { Converter = PointValueConverter });
      BindingOperations.SetBinding(forme, EllipsoidVisual3D.RadiusXProperty, new Binding($"{ path }.RayonX"));
      BindingOperations.SetBinding(forme, EllipsoidVisual3D.RadiusYProperty, new Binding($"{ path }.RayonY"));
      BindingOperations.SetBinding(forme, EllipsoidVisual3D.RadiusZProperty, new Binding($"{ path }.RayonZ"));
      BindingOperations.SetBinding(forme, EllipsoidVisual3D.PhiDivProperty, new Binding($"{ path }.PhiDiv"));
      BindingOperations.SetBinding(forme, EllipsoidVisual3D.ThetaDivProperty, new Binding($"{ path }.ThetaDiv"));

      return forme;
    }

    private BoxVisual3D Lier(BoxVisual3D forme, string path)
    {
      BindingOperations.SetBinding(forme, BoxVisual3D.CenterProperty, new Binding($"{ path }.Centre") { Converter = PointValueConverter });
      BindingOperations.SetBinding(forme, BoxVisual3D.HeightProperty, new Binding($"{ path }.Hauteur"));
      BindingOperations.SetBinding(forme, BoxVisual3D.LengthProperty, new Binding($"{ path }.Longueur"));
      BindingOperations.SetBinding(forme, BoxVisual3D.WidthProperty, new Binding($"{ path }.Largeur"));

      return forme;
    }

    #endregion
  }
}
