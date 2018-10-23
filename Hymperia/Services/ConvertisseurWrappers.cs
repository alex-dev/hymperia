using System;
using System.Collections.ObjectModel;
using System.Windows.Data;
using HelixToolkit.Wpf;
using Hymperia.Facade.ModelWrappers;
using JetBrains.Annotations;
using Hymperia.Facade.Converters;

namespace Hymperia.Facade.Services
{
  /// <summary>Convertit des <see cref="FormeWrapper"/> en <see cref="MeshElement3D"/> et les binds aux <see cref="FormeWrapper"/>.</summary>
  public class ConvertisseurWrappers
  {
    public ConvertisseurWrappers([NotNull] TransformConverter transform)
    {
      TransformConverter = transform;
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
    private CylinderVisual3D Convertir([NotNull] CylindreWrapper forme) => Lier(new CylinderVisual3D(), forme);
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
        case CylinderVisual3D cylinder:
          return Lier(cylinder, (CylindreWrapper)source);
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
      BindingOperations.SetBinding(forme, TruncatedConeVisual3D.HeightProperty, new Binding(nameof(source.Hauteur)) { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, TruncatedConeVisual3D.BaseRadiusProperty, new Binding(nameof(source.RayonBase)) { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, TruncatedConeVisual3D.TopRadiusProperty, new Binding(nameof(source.RayonTop)) { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, TruncatedConeVisual3D.ThetaDivProperty, new Binding(nameof(source.ThetaDiv)) { Source = source, Mode = BindingMode.TwoWay });

      return (TruncatedConeVisual3D)_Lier(forme, source);
    }

    [NotNull]
    private CylinderVisual3D Lier([NotNull] CylinderVisual3D forme, [NotNull] CylindreWrapper source)
    {
      BindingOperations.SetBinding(forme, CylinderVisual3D.OriginProperty, new Binding(nameof(source.Origine)) { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, CylinderVisual3D.HeightProperty, new Binding(nameof(source.Hauteur)) { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, CylinderVisual3D.DiameterProperty, new Binding(nameof(source.Diametre)) { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, CylinderVisual3D.InnerDiameterProperty, new Binding(nameof(source.InnerDiametre)) { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, CylinderVisual3D.ThetaDivProperty, new Binding(nameof(source.ThetaDiv)) { Source = source, Mode = BindingMode.TwoWay });

      return (CylinderVisual3D)_Lier(forme, source);
    }

    [NotNull]
    private EllipsoidVisual3D Lier([NotNull] EllipsoidVisual3D forme, [NotNull] EllipsoideWrapper source)
    {
      BindingOperations.SetBinding(forme, EllipsoidVisual3D.RadiusXProperty, new Binding(nameof(source.RayonX)) { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, EllipsoidVisual3D.RadiusYProperty, new Binding(nameof(source.RayonY)) { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, EllipsoidVisual3D.RadiusZProperty, new Binding(nameof(source.RayonZ)) { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, EllipsoidVisual3D.PhiDivProperty, new Binding(nameof(source.PhiDiv)) { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, EllipsoidVisual3D.ThetaDivProperty, new Binding(nameof(source.ThetaDiv)) { Source = source, Mode = BindingMode.TwoWay });

      return (EllipsoidVisual3D)_Lier(forme, source);
    }

    [NotNull]
    private BoxVisual3D Lier([NotNull] BoxVisual3D forme, [NotNull] PrismeRectangulaireWrapper source)
    {
      BindingOperations.SetBinding(forme, BoxVisual3D.HeightProperty, new Binding(nameof(source.Hauteur)) { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, BoxVisual3D.LengthProperty, new Binding(nameof(source.Longueur)) { Source = source, Mode = BindingMode.TwoWay });
      BindingOperations.SetBinding(forme, BoxVisual3D.WidthProperty, new Binding(nameof(source.Largeur)) { Source = source, Mode = BindingMode.TwoWay });

      return (BoxVisual3D)_Lier(forme, source);
    }

    [NotNull]
    private MeshElement3D _Lier([NotNull] MeshElement3D forme, [NotNull] FormeWrapper source)
    {
      var fill = new Binding($"{ nameof(source.Materiau) }.{ nameof(source.Materiau.Fill) }") { Source = source, Mode = BindingMode.OneWay };
      var transform = new MultiBinding() { Converter = TransformConverter, Mode = BindingMode.TwoWay };
      transform.Bindings.AddRange(new Binding[]
      {
        new Binding(nameof(source.Origine)) { Source = source, Mode = BindingMode.TwoWay },
        new Binding(nameof(source.Rotation)) { Source = source, Mode = BindingMode.TwoWay }
      });

      BindingOperations.SetBinding(forme, MeshElement3D.TransformProperty, transform);
      BindingOperations.SetBinding(forme, MeshElement3D.FillProperty, fill);

      return forme;
    }

    #endregion

    #region Services

    [NotNull]
    private readonly TransformConverter TransformConverter;

    #endregion
  }
}
