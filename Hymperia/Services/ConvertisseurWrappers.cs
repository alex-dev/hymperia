using System;
using System.Collections.ObjectModel;
using System.Windows.Data;
using HelixToolkit.Wpf;
using Hymperia.Facade.ModelWrappers;
using Hymperia.Facade.Converters.PointsToHeightConverters;
using JetBrains.Annotations;
using Hymperia.Facade.Converters;

namespace Hymperia.Facade.Services
{
  /// <summary>Convertit des <see cref="FormeWrapper"/> en <see cref="MeshElement3D"/> et les binds aux <see cref="FormeWrapper"/>.</summary>
  public class ConvertisseurWrappers
  {
    #region Services

    [NotNull]
    private readonly TransformConverter TransformConverter;
    [NotNull]
    private readonly HeightToPointsConverter PointToHauteurConverter;

    #endregion

    public ConvertisseurWrappers([NotNull] TransformConverter transform, [NotNull] HeightToPointsConverter pointToHauteur)
    {
      TransformConverter = transform;
      PointToHauteurConverter = pointToHauteur;
    }

    #region Convertir

    /// <summary>Convertit <paramref name="forme"/> en <see cref="MeshElement3D"/>.</summary>
    /// <exception cref="ArgumentException"><paramref name="forme"/> n'était pas une <see cref="Forme"/> connue.</exception>
    [NotNull]
    public MeshElement3D Convertir([NotNull] FormeWrapper forme)
    {
      switch (forme)
      {
        case ConeWrapper cone:
          return Convertir(cone);
        case CylindreWrapper cylindre:
          return Convertir(cylindre);
        case EllipsoideWrapper ellipsoide:
          return Convertir(ellipsoide);
        case PrismeRectangulaireWrapper prisme:
          return Convertir(prisme);
        default:
          throw new ArgumentException($"Unknown child of { nameof(FormeWrapper) }.", nameof(forme));
      }
    }

    [NotNull]
    private TruncatedConeVisual3D Convertir([NotNull] ConeWrapper forme) => Lier(new TruncatedConeVisual3D(), forme);
    [NotNull]
    private PipeVisual3D Convertir([NotNull] CylindreWrapper forme) => Lier(new PipeVisual3D(), forme);
    [NotNull]
    private EllipsoidVisual3D Convertir([NotNull] EllipsoideWrapper forme) => Lier(new EllipsoidVisual3D(), forme);
    [NotNull]
    private BoxVisual3D Convertir([NotNull] PrismeRectangulaireWrapper forme) => Lier(new BoxVisual3D(), forme);

    #endregion

    #region Lier

    /// <summary>DataBind <paramref name="forme"/> selon <paramref name="path"/>.</summary>
    /// <exception cref="InvalidCastException"><paramref name="source"/> était une <see cref="Forme"/> incompatible avec <paramref name="forme"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="forme"/> était un <see cref="MeshElement3D"/> inconnu.</exception>
    [NotNull]
    public MeshElement3D Lier([NotNull] MeshElement3D forme, [NotNull] FormeWrapper source)
    {
      switch (forme)
      {
        case TruncatedConeVisual3D cone:
          return Lier(cone, (ConeWrapper)source);
         case PipeVisual3D cylindre:
           return Lier(cylindre, (CylindreWrapper)source);
        case EllipsoidVisual3D ellipsoide:
          return Lier(ellipsoide, (EllipsoideWrapper)source);
        case BoxVisual3D prisme:
          return Lier(prisme, (PrismeRectangulaireWrapper)source);
        default:
          throw new ArgumentException($"Unknown child of { nameof(MeshElement3D) }.", nameof(forme));
      }
    }

    [NotNull]
    private TruncatedConeVisual3D Lier([NotNull] TruncatedConeVisual3D forme, [NotNull] ConeWrapper source)
    {
      BindingOperations.SetBinding(forme, TruncatedConeVisual3D.HeightProperty, new Binding("Hauteur") { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, TruncatedConeVisual3D.BaseRadiusProperty, new Binding("RayonBase") { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, TruncatedConeVisual3D.TopRadiusProperty, new Binding("RayonTop") { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, TruncatedConeVisual3D.ThetaDivProperty, new Binding("ThetaDiv") { Source = source, Mode = BindingMode.TwoWay });

      return (TruncatedConeVisual3D)_Lier(forme, source);
    }

    [NotNull]
    private PipeVisual3D Lier([NotNull] PipeVisual3D forme, [NotNull] CylindreWrapper source)
    {
      var hauteur_binding_top = new Binding("Hauteur")
      {
        Source = source,
        Converter = PointToHauteurConverter,
        ConverterParameter = PointOrientation.Top,
        Mode = BindingMode.TwoWay
      };
      var hauteur_binding_bottom = new Binding("Hauteur")
      {
        Source = source,
        Converter = PointToHauteurConverter,
        ConverterParameter = PointOrientation.Bottom,
        Mode = BindingMode.TwoWay
      };

      BindingOperations.SetBinding(forme, PipeVisual3D.Point1Property, hauteur_binding_top);
      BindingOperations.SetBinding(forme, PipeVisual3D.Point2Property, hauteur_binding_bottom);
      BindingOperations.SetBinding(forme, PipeVisual3D.DiameterProperty, new Binding("Diametre") { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, PipeVisual3D.InnerDiameterProperty, new Binding("InnerDiametre") { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, PipeVisual3D.ThetaDivProperty, new Binding("ThetaDiv") { Source = source, Mode = BindingMode.TwoWay });

      return (PipeVisual3D)_Lier(forme, source);
    }

    [NotNull]
    private EllipsoidVisual3D Lier([NotNull] EllipsoidVisual3D forme, [NotNull] EllipsoideWrapper source)
    {
      BindingOperations.SetBinding(forme, EllipsoidVisual3D.RadiusXProperty, new Binding("RayonX") { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, EllipsoidVisual3D.RadiusYProperty, new Binding("RayonY") { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, EllipsoidVisual3D.RadiusZProperty, new Binding("RayonZ") { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, EllipsoidVisual3D.PhiDivProperty, new Binding("PhiDiv") { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, EllipsoidVisual3D.ThetaDivProperty, new Binding("ThetaDiv") { Source = source, Mode = BindingMode.TwoWay });

      return (EllipsoidVisual3D)_Lier(forme, source);
    }

    [NotNull]
    private BoxVisual3D Lier([NotNull] BoxVisual3D forme, [NotNull] PrismeRectangulaireWrapper source)
    {
      BindingOperations.SetBinding(forme, BoxVisual3D.HeightProperty, new Binding("Hauteur") { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, BoxVisual3D.LengthProperty, new Binding("Longueur") { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, BoxVisual3D.WidthProperty, new Binding("Largeur") { Source = source, Mode = BindingMode.TwoWay });

      return (BoxVisual3D)_Lier(forme, source);
    }

    [NotNull]
    private MeshElement3D _Lier([NotNull] MeshElement3D forme, [NotNull] FormeWrapper source)
    {
      var bindings = new MultiBinding() { Converter = TransformConverter, Mode = BindingMode.TwoWay };
      bindings.Bindings.AddRange(new Binding[]
      {
        new Binding("Origine") { Source = source, Mode = BindingMode.TwoWay },
        new Binding("Rotation") { Source = source, Mode = BindingMode.TwoWay }
      });

      BindingOperations.SetBinding(forme, MeshElement3D.TransformProperty, bindings);
      BindingOperations.SetBinding(forme, MeshElement3D.FillProperty, new Binding("Materiau.Fill") { Source = source, Mode = BindingMode.OneWay });

      return forme;
    }

    #endregion
  }
}
