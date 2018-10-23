using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using JetBrains.Annotations;

namespace Hymperia.Facade.DependencyObjects.Manipulators
{
  /// <summary>Manipulateur de redimensionnement.</summary>
  public class ResizeManipulator : CombinedManipulator
  {
    #region Dependency Properties

    /// <seealso cref="Height"/>
    public static readonly DependencyProperty HeightProperty =
      DependencyProperty.Register(nameof(Height), typeof(double), typeof(ResizeManipulator), new PropertyMetadata(2.0));
    /// <seealso cref="Length"/>
    public static readonly DependencyProperty LengthProperty =
      DependencyProperty.Register(nameof(Length), typeof(double), typeof(ResizeManipulator), new PropertyMetadata(2.0));
    /// <seealso cref="Width"/>
    public static readonly DependencyProperty WidthProperty =
      DependencyProperty.Register(nameof(Width), typeof(double), typeof(ResizeManipulator), new PropertyMetadata(2.0));

    #endregion

    #region Properties

    /// <summary>La taille en Z de la source.</summary>
    public double Height
    {
      get => (double)GetValue(HeightProperty);
      set => SetValue(HeightProperty, value);
    }

    /// <summary>La taille en X de la source.</summary>
    public double Length
    {
      get => (double)GetValue(LengthProperty);
      set => SetValue(LengthProperty, value);
    }

    /// <summary>La taille en Y de la source.</summary>
    public double Width
    {
      get => (double)GetValue(WidthProperty);
      set => SetValue(WidthProperty, value);
    }

    #endregion

    #region Constructors

    /// <inheritdoc/>
    [NotNull]
    [ItemNotNull]
    protected override IEnumerable<Manipulator> GenerateManipulators()
    {
      {
        var manipulator = new ScaleManipulator { Direction = new Vector3D(1, 0, 0), Color = Colors.Red, BindLengthToValue = false };
        BindToLengthManipulator(manipulator);
        yield return manipulator;
      }
      {
        var manipulator = new ScaleManipulator { Direction = new Vector3D(0, 1, 0), Color = Colors.Green, BindLengthToValue = false };
        BindToWidthManipulator(manipulator);
        yield return manipulator;
      }
      {
        var manipulator = new ScaleManipulator { Direction = new Vector3D(0, 0, 1), Color = Colors.Blue, BindLengthToValue = false };
        BindToHeightManipulator(manipulator);
        yield return manipulator;
      }
      {
        var manipulator = new ScaleManipulator { Direction = new Vector3D(-1, 0, 0), Color = Colors.Red, BindLengthToValue = false };
        BindToLengthManipulator(manipulator);
        yield return manipulator;
      }
      {
        var manipulator = new ScaleManipulator { Direction = new Vector3D(0, -1, 0), Color = Colors.Green, BindLengthToValue = false };
        BindToWidthManipulator(manipulator);
        yield return manipulator;
      }
      {
        var manipulator = new ScaleManipulator { Direction = new Vector3D(0, 0, -1), Color = Colors.Blue, BindLengthToValue = false };
        BindToHeightManipulator(manipulator);
        yield return manipulator;
      }
    }

    #region Manipulator Size Bindings

    private void BindToHeightManipulator([NotNull] Manipulator manipulator) =>
      BindTo(nameof(Height), (ScaleManipulator)manipulator);
    private void BindToLengthManipulator([NotNull] Manipulator manipulator) =>
      BindTo(nameof(Length), (ScaleManipulator)manipulator);
    private void BindToWidthManipulator([NotNull] Manipulator manipulator) =>
      BindTo(nameof(Width), (ScaleManipulator)manipulator);
    private void BindTo([NotNull] string dimension, [NotNull] ScaleManipulator manipulator)
    {
      SetBinding(manipulator, Manipulator.ValueProperty, new Binding(dimension) { Source = this, Mode = BindingMode.TwoWay });
      BindToManipulator(manipulator);
    }

    #endregion

    #endregion

    #region Binding from Source

    /// <inheritdoc/>
    public override void Bind([NotNull] ModelVisual3D source)
    {
      var (height, length, width) = CreateBindings(source);

      base.Bind(source);
      SetBinding(HeightProperty, height);
      SetBinding(LengthProperty, length);
      SetBinding(WidthProperty, width);
      SetBinding(TransformProperty, new Binding(nameof(source.Transform)) { Source = source, Mode = BindingMode.OneWay });
    }

    /// <inheritdoc/>
    public override void Unbind()
    {
      base.Unbind();
      ClearBinding(HeightProperty);
      ClearBinding(LengthProperty);
      ClearBinding(WidthProperty);
      ClearBinding(TransformProperty);
    }

    [NotNull]
    [ItemNotNull]
    [SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods",
      Justification = @"Hiding is on private methods.")]
    private Tuple<Binding, Binding, Binding> CreateBindings([NotNull] ModelVisual3D source)
    {
      switch (source)
      {
        case BoxVisual3D box:
          return CreateBindings(box);
        case EllipsoidVisual3D ellipsoid:
          return CreateBindings(ellipsoid);
        case CylinderVisual3D cylinder:
          return CreateBindings(cylinder);
        case TruncatedConeVisual3D cone:
          return CreateBindings(cone);
        default:
          string err = $"This manipulator only support { nameof(BoxVisual3D) }, { nameof(EllipsoidVisual3D) }, { nameof(PipeVisual3D) } and { nameof(TruncatedConeVisual3D) }.";
          throw new InvalidOperationException(err);
      }
    }

    [NotNull]
    [ItemNotNull]
    [SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods",
      Justification = @"Hiding is on private methods.")]
    private Tuple<Binding, Binding, Binding> CreateBindings([NotNull] BoxVisual3D source) =>
      Tuple.Create(
        new Binding(nameof(source.Height)) { Source = source, Converter = LinearConverter, ConverterParameter = 0.5, Mode = BindingMode.TwoWay },
        new Binding(nameof(source.Length)) { Source = source, Converter = LinearConverter, ConverterParameter = 0.5, Mode = BindingMode.TwoWay },
        new Binding(nameof(source.Width)) { Source = source, Converter = LinearConverter, ConverterParameter = 0.5, Mode = BindingMode.TwoWay });

    [NotNull]
    [ItemNotNull]
    [SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods",
      Justification = @"Hiding is on private methods.")]
    private Tuple<Binding, Binding, Binding> CreateBindings([NotNull] EllipsoidVisual3D source) =>
      Tuple.Create(
        new Binding(nameof(source.RadiusZ)) { Source = source, Mode = BindingMode.TwoWay },
        new Binding(nameof(source.RadiusX)) { Source = source, Mode = BindingMode.TwoWay },
        new Binding(nameof(source.RadiusY)) { Source = source, Mode = BindingMode.TwoWay });

    [NotNull]
    [ItemNotNull]
    [SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods",
      Justification = @"Hiding is on private methods.")]
    private Tuple<Binding, Binding, Binding> CreateBindings([NotNull] CylinderVisual3D source)
    {
      var diameter = new Binding(nameof(source.Diameter)) { Source = source, Converter = LinearConverter, ConverterParameter = 0.5, Mode = BindingMode.TwoWay };

      return Tuple.Create(
        new Binding(nameof(source.Height)) { Source = source, Converter = LinearConverter, ConverterParameter = 0.5, Mode = BindingMode.TwoWay },
        diameter,
        diameter);
    }

    [NotNull]
    [ItemNotNull]
    [SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods",
      Justification = @"Hiding is on private methods.")]
    private Tuple<Binding, Binding, Binding> CreateBindings([NotNull] TruncatedConeVisual3D source)
    {
      var radius = new Binding(nameof(source.BaseRadius)) { Source = source, Mode = BindingMode.TwoWay };

      return Tuple.Create(
        new Binding(nameof(source.Height)) { Source = source, Mode = BindingMode.TwoWay },
        radius,
        radius);
    }

    #endregion
  }
}
