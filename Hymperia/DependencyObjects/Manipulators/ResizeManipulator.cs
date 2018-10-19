using System;
using System.Collections.Generic;
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
      DependencyProperty.Register("Width", typeof(double), typeof(ResizeManipulator), new PropertyMetadata(2d));
    /// <seealso cref="Length"/>
    public static readonly DependencyProperty LengthProperty =
      DependencyProperty.Register("Height", typeof(double), typeof(ResizeManipulator), new PropertyMetadata(2d));
    /// <seealso cref="Width"/>
    public static readonly DependencyProperty WidthProperty =
      DependencyProperty.Register("Length", typeof(double), typeof(ResizeManipulator), new PropertyMetadata(2d));

    #endregion

    #region Properties

    /// <summary>La taille en Y de la source.</summary>
    public double Height
    {
      get => (double)GetValue(HeightProperty);
      set => SetValue(HeightProperty, value);
    }

    /// <summary>La taille en Z de la source.</summary>
    public double Length
    {
      get => (double)GetValue(LengthProperty);
      set => SetValue(LengthProperty, value);
    }

    /// <summary>La taille en X de la source.</summary>
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
    protected override IEnumerable<Tuple<Manipulator, Action<Manipulator>>> GenerateManipulators()
    {
      yield return Tuple.Create<Manipulator, Action<Manipulator>>(
        new TranslateManipulator { Direction = new Vector3D(1, 0, 0), Color = Colors.Red },
        BindToHeightManipulator);
      yield return Tuple.Create<Manipulator, Action<Manipulator>>(
        new TranslateManipulator { Direction = new Vector3D(0, 1, 0), Color = Colors.Green },
        BindToLengthManipulator);
      yield return Tuple.Create<Manipulator, Action<Manipulator>>(
        new TranslateManipulator { Direction = new Vector3D(0, 0, 1), Color = Colors.Blue },
        BindToWidthManipulator);
      yield return Tuple.Create<Manipulator, Action<Manipulator>>(
        new TranslateManipulator { Direction = new Vector3D(-1, 0, 0), Color = Colors.Red },
        BindToHeightManipulator);
      yield return Tuple.Create<Manipulator, Action<Manipulator>>(
        new TranslateManipulator { Direction = new Vector3D(0, -1, 0), Color = Colors.Green },
        BindToLengthManipulator);
      yield return Tuple.Create<Manipulator, Action<Manipulator>>(
        new TranslateManipulator { Direction = new Vector3D(0, 0, -1), Color = Colors.Blue },
        BindToWidthManipulator);
    }

    #region Manipulator Size Bindings

    private void BindToHeightManipulator([NotNull] Manipulator manipulator) =>
      BindTo(nameof(Height), (TranslateManipulator)manipulator);
    private void BindToLengthManipulator([NotNull] Manipulator manipulator) =>
      BindTo(nameof(Length), (TranslateManipulator)manipulator);
    private void BindToWidthManipulator([NotNull] Manipulator manipulator) =>
      BindTo(nameof(Width), (TranslateManipulator)manipulator);
    private void BindTo([NotNull] string dimension, [NotNull] TranslateManipulator manipulator)
    {
      //SetBinding(manipulator, Manipulator.ValueProperty, new Binding(dimension) { Source = this, Mode = BindingMode.TwoWay });
      BindToTranslateManipulator(manipulator);
    }

    #endregion

    #endregion

    #region Binding to Source

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
    private Tuple<Binding, Binding, Binding> CreateBindings([NotNull] ModelVisual3D source)
    {
      switch (source)
      {
        case BoxVisual3D box:
          return CreateBindings(box);
        case EllipsoidVisual3D ellipsoid:
          return CreateBindings(ellipsoid);
        case PipeVisual3D pipe:
          throw new NotImplementedException();
          return CreateBindings(pipe);
        case TruncatedConeVisual3D cone:
          throw new NotImplementedException();
          return CreateBindings(cone);
        default:
          string err = $"This manipulator only support { nameof(BoxVisual3D) }, { nameof(EllipsoidVisual3D) }, { nameof(PipeVisual3D) } and { nameof(TruncatedConeVisual3D) }.";
          throw new InvalidOperationException(err);
      }
    }

    [NotNull]
    [ItemNotNull]
    private Tuple<Binding, Binding, Binding> CreateBindings([NotNull] BoxVisual3D source) =>
      Tuple.Create(
        new Binding(nameof(source.Height)) { Source = source, Mode = BindingMode.TwoWay },
        new Binding(nameof(source.Length)) { Source = source, Mode = BindingMode.TwoWay },
        new Binding(nameof(source.Width)) { Source = source, Mode = BindingMode.TwoWay });

    [NotNull]
    [ItemNotNull]
    private Tuple<Binding, Binding, Binding> CreateBindings([NotNull] EllipsoidVisual3D source) =>
      Tuple.Create(
        new Binding(nameof(source.RadiusZ)) { Source = source, Mode = BindingMode.TwoWay },
        new Binding(nameof(source.RadiusX)) { Source = source, Mode = BindingMode.TwoWay },
        new Binding(nameof(source.RadiusY)) { Source = source, Mode = BindingMode.TwoWay });

    /*private Tuple<Binding, Binding, Binding> CreateBindings([NotNull] PipeVisual3D source) =>
      Tuple.Create(
        new Binding("RadiusZ") { Source = source, Converter = LinearConverter, Mode = BindingMode.TwoWay },
        new Binding("RadiusX") { Source = source, Converter = LinearConverter, Mode = BindingMode.TwoWay },
        new Binding("WidthY") { Source = source, Converter = LinearConverter, Mode = BindingMode.TwoWay });

    private Tuple<Binding, Binding, Binding> CreateBindings([NotNull] TruncatedConeVisual3D source) =>
      Tuple.Create(
        new Binding("Height") { Source = source, Converter = LinearConverter, Mode = BindingMode.TwoWay },
        new Binding("RadiusX") { Source = source, Converter = LinearConverter, Mode = BindingMode.TwoWay },
        new Binding("WidthY") { Source = source, Converter = LinearConverter, Mode = BindingMode.TwoWay });*/

    #endregion
  }
}
