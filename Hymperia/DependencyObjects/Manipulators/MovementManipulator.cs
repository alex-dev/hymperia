using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using Hymperia.Facade.Converters.AggregateConverters;
using Hymperia.Facade.Converters.PointsToHeightConverters;
using JetBrains.Annotations;

namespace Hymperia.Facade.DependencyObjects.Manipulators
{
  /// <summary>Manipulateur de déplacement.</summary>
  public class MovementManipulator : CombinedManipulator
  {
    #region Dependency Properties

    /// <seealso cref=nameof(Diameter)/>
    public static readonly DependencyProperty DiameterProperty =
      DependencyProperty.Register(nameof(Diameter), typeof(double), typeof(MovementManipulator), new PropertyMetadata(2d));

    #endregion

    #region Properties

    /// <summary>Le diamètre du manipulateur.</summary>
    public double Diameter
    {
      get => (double)GetValue(DiameterProperty);
      set => SetValue(DiameterProperty, value);
    }

    #endregion

    #region Constructors

    /// inheritdoc/>
    [NotNull]
    [ItemNotNull]
    protected override IEnumerable<Tuple<Manipulator, Action<Manipulator>>> GenerateManipulators()
    {
      IEnumerable<Manipulator> Generate()
      {
        yield return new RotateManipulator { Axis = new Vector3D(1, 0, 0), Color = Colors.Red };
        yield return new RotateManipulator { Axis = new Vector3D(0, 1, 0), Color = Colors.Green };
        yield return new RotateManipulator { Axis = new Vector3D(0, 0, 1), Color = Colors.Blue };
        yield return new TranslateManipulator { Direction = new Vector3D(1, 0, 0), Color = Colors.Red };
        yield return new TranslateManipulator { Direction = new Vector3D(0, 1, 0), Color = Colors.Green };
        yield return new TranslateManipulator { Direction = new Vector3D(0, 0, 1), Color = Colors.Blue };
      }

      return from manipulator in Generate()
             select Tuple.Create<Manipulator, Action<Manipulator>>(manipulator, BindToManipulator);
    }

    #region Manipulator Size Bindings

    private void BindToManipulator([NotNull] Manipulator manipulator)
    {
      switch (manipulator)
      {
        case TranslateManipulator translate:
          BindToTranslateManipulator(translate); break;
        case RotateManipulator rotate:
          BindToRotationManipulator(rotate); break;
      }
    }

    private void BindToTranslateManipulator([NotNull] TranslateManipulator manipulator)
    {
      SetBinding(manipulator, TranslateManipulator.LengthProperty, new Binding(nameof(Diameter))
      {
        Source = this,
        Converter = LinearConverter,
        ConverterParameter = 1.25,
        Mode = BindingMode.OneWay
      });
      SetBinding(manipulator, TranslateManipulator.DiameterProperty, new Binding(nameof(Diameter))
      {
        Source = this,
        Converter = LinearConverter,
        ConverterParameter = 0.12,
        Mode = BindingMode.OneWay
      });
    }

    private void BindToRotationManipulator([NotNull] RotateManipulator manipulator)
    {
      SetBinding(manipulator, RotateManipulator.DiameterProperty, new Binding(nameof(Diameter))
      {
        Source = this,
        Converter = LinearConverter,
        ConverterParameter = 1.65,
        Mode = BindingMode.OneWay
      });
      SetBinding(manipulator, RotateManipulator.InnerDiameterProperty, new Binding(nameof(Diameter))
      {
        Source = this,
        Converter = LinearConverter,
        ConverterParameter = 1.5,
        Mode = BindingMode.OneWay
      });
      SetBinding(manipulator, RotateManipulator.LengthProperty, new Binding(nameof(Diameter))
      {
        Source = this,
        Converter = LinearConverter,
        ConverterParameter = 0.1,
        Mode = BindingMode.OneWay
      });
    }

    #endregion

    #endregion

    #region Binding to Source

    /// inheritdoc/>
    public override void Bind([NotNull] ModelVisual3D source)
    {
      var bindings = new MultiBinding() { Mode = BindingMode.OneWay, Converter = MaxConverter };
      bindings.Bindings.AddRange(CreateBindings(source));

      SetBinding(DiameterProperty, bindings);
      SetBinding(TransformProperty, new Binding(nameof(source.Transform)) { Source = source, Mode = BindingMode.TwoWay });
    }

    /// inheritdoc/>
    public override void Unbind()
    {
      ClearBinding(DiameterProperty);
      ClearBinding(TransformProperty);
    }

    [NotNull]
    [ItemNotNull]
    private IEnumerable<Binding> CreateBindings([NotNull] ModelVisual3D source)
    {
      switch (source)
      {
        case BoxVisual3D box:
          return CreateBindings(box);
        case EllipsoidVisual3D ellipsoid:
          return CreateBindings(ellipsoid);
        case PipeVisual3D pipe:
          return CreateBindings(pipe);
        case TruncatedConeVisual3D cone:
          return CreateBindings(cone);
        default:
          string err = $"This manipulator only support { nameof(BoxVisual3D) }, { nameof(EllipsoidVisual3D) }, { nameof(PipeVisual3D) } and { nameof(TruncatedConeVisual3D) }.";
          throw new InvalidOperationException(err);
      }
    }

    [NotNull]
    [ItemNotNull]
    private IEnumerable<Binding> CreateBindings([NotNull] BoxVisual3D source)
    {
      yield return new Binding(nameof(source.Height)) { Source = source, Mode = BindingMode.OneWay };
      yield return new Binding(nameof(source.Length)) { Source = source, Mode = BindingMode.OneWay };
      yield return new Binding(nameof(source.Width)) { Source = source, Mode = BindingMode.OneWay };
    }

    [NotNull]
    [ItemNotNull]
    private IEnumerable<Binding> CreateBindings([NotNull] EllipsoidVisual3D source)
    {
      yield return new Binding(nameof(source.RadiusX))
      {
        Source = source,
        Converter = LinearConverter,
        ConverterParameter = 2,
        Mode = BindingMode.OneWay
      };
      yield return new Binding(nameof(source.RadiusY))
      {
        Source = source,
        Converter = LinearConverter,
        ConverterParameter = 2,
        Mode = BindingMode.OneWay
      };
      yield return new Binding(nameof(source.RadiusZ))
      {
        Source = source,
        Converter = LinearConverter,
        ConverterParameter = 2,
        Mode = BindingMode.OneWay
      };
    }

    [NotNull]
    [ItemNotNull]
    private IEnumerable<Binding> CreateBindings([NotNull] PipeVisual3D source)
    {
      yield return new Binding(nameof(source.Diameter)) { Source = source, Mode = BindingMode.OneWay };
      yield return new Binding(nameof(source.Point2))
      {
        Source = source,
        Converter = PointsToHeightConverter,
        ConverterParameter = PointOrientation.Top,
        Mode = BindingMode.TwoWay
      };
    }

    [NotNull]
    [ItemNotNull]
    private IEnumerable<Binding> CreateBindings([NotNull] TruncatedConeVisual3D source)
    {
      yield return new Binding(nameof(source.Height))
      {
        Source = source,
        Converter = LinearConverter,
        ConverterParameter = 1.25,
        Mode = BindingMode.OneWay
      };
      yield return new Binding(nameof(source.BaseRadius))
      {
        Source = source,
        Converter = LinearConverter,
        ConverterParameter = 2,
        Mode = BindingMode.OneWay
      };
      yield return new Binding(nameof(source.TopRadius))
      {
        Source = source,
        Converter = LinearConverter,
        ConverterParameter = 2,
        Mode = BindingMode.OneWay
      };
    }

    #endregion

    #region Static Services

    [NotNull]
    protected static readonly MaxAggregateConverter MaxConverter =
      (MaxAggregateConverter)Application.Current.Resources["MaxAggregate"];
    [NotNull]
    protected static readonly LinearConverter LinearConverter =
      (LinearConverter)Application.Current.Resources["Linear"];
    [NotNull]
    protected static readonly PointsToHeightConverter PointsToHeightConverter =
      (PointsToHeightConverter)Application.Current.Resources["PointsToHeight"];

    #endregion
  }
}
