using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using Hymperia.Facade.Services.PointsToHeightConverters;
using JetBrains.Annotations;

namespace Hymperia.Facade.DependencyObjects.Manipulators
{
  public class ResizeManipulator : CombinedManipulator
  {
    #region Dependency Properties

    public static readonly DependencyProperty HeightProperty;
    public static readonly DependencyProperty LengthProperty;
    public static readonly DependencyProperty WidthProperty;

    #endregion

    #region Properties

    public double Height
    {
      get => (double)GetValue(HeightProperty);
      set => SetValue(HeightProperty, value);
    }

    public double Length
    {
      get => (double)GetValue(LengthProperty);
      set => SetValue(LengthProperty, value);
    }

    public double Width
    {
      get => (double)GetValue(WidthProperty);
      set => SetValue(WidthProperty, value);
    }

    #endregion

    #region Constructors

    static ResizeManipulator()
    {
      LinearConverter = new LinearConverter() { M = 1 };
      PointsToHeightConverter = new PointsToHeightConverter();
      WidthProperty = DependencyProperty.Register("Width", typeof(double), typeof(ResizeManipulator));
      HeightProperty = DependencyProperty.Register("Height", typeof(double), typeof(ResizeManipulator));
      LengthProperty = DependencyProperty.Register("Length", typeof(double), typeof(ResizeManipulator));
    }

    public ResizeManipulator() : base() { }

    [NotNull]
    [ItemNotNull]
    protected override IEnumerable<Tuple<Manipulator, Action<Manipulator>>> GenerateManipulators()
    {
      yield return Tuple.Create<Manipulator, Action<Manipulator>>(
        new TranslateManipulator { Direction = new Vector3D(1, 0, 0), Color = Colors.Red },
        BindToLengthManipulator);
      yield return Tuple.Create<Manipulator, Action<Manipulator>>(
        new TranslateManipulator { Direction = new Vector3D(0, 1, 0), Color = Colors.Green },
        BindToWidthManipulator);
      yield return Tuple.Create<Manipulator, Action<Manipulator>>(
        new TranslateManipulator { Direction = new Vector3D(0, 0, 1), Color = Colors.Blue },
        BindToHeightManipulator);
      yield return Tuple.Create<Manipulator, Action<Manipulator>>(
        new TranslateManipulator { Direction = new Vector3D(-1, 0, 0), Color = Colors.Red },
        BindToLengthManipulator);
      yield return Tuple.Create<Manipulator, Action<Manipulator>>(
        new TranslateManipulator { Direction = new Vector3D(0, -1, 0), Color = Colors.Green },
        BindToWidthManipulator);
      yield return Tuple.Create<Manipulator, Action<Manipulator>>(
        new TranslateManipulator { Direction = new Vector3D(0, 0, -1), Color = Colors.Blue },
        BindToHeightManipulator);
    }

    #region Manipulator Size Bindings

    private void BindToHeightManipulator(Manipulator manipulator)
    {
      BindingOperations.SetBinding(manipulator, TranslateManipulator.LengthProperty, new Binding("Height")
      {
        Source = this,
        Converter = LinearConverter,
        ConverterParameter = 1.25,
        Mode = BindingMode.OneWay
      });
      BindingOperations.SetBinding(manipulator, TranslateManipulator.DiameterProperty, new Binding("Height")
      {
        Source = this,
        Converter = LinearConverter,
        ConverterParameter = 0.12,
        Mode = BindingMode.OneWay
      });
    }
    private void BindToLengthManipulator(Manipulator manipulator)
    {
      BindingOperations.SetBinding(manipulator, TranslateManipulator.LengthProperty, new Binding("Length")
      {
        Source = this,
        Converter = LinearConverter,
        ConverterParameter = 1.25,
        Mode = BindingMode.OneWay
      });
      BindingOperations.SetBinding(manipulator, TranslateManipulator.DiameterProperty, new Binding("Length")
      {
        Source = this,
        Converter = LinearConverter,
        ConverterParameter = 0.12,
        Mode = BindingMode.OneWay
      });
    }

    private void BindToWidthManipulator(Manipulator manipulator)
    {
      BindingOperations.SetBinding(manipulator, TranslateManipulator.LengthProperty, new Binding("Width")
      {
        Source = this,
        Converter = LinearConverter,
        ConverterParameter = 1.25,
        Mode = BindingMode.OneWay
      });
      BindingOperations.SetBinding(manipulator, TranslateManipulator.DiameterProperty, new Binding("Width")
      {
        Source = this,
        Converter = LinearConverter,
        ConverterParameter = 0.12,
        Mode = BindingMode.OneWay
      });
    }

    #endregion

    #endregion

    #region Binding to Source

    public override void Bind([NotNull] ModelVisual3D source)
    {
      (Binding height, Binding length, Binding width) = CreateBindings(source);

      BindingOperations.SetBinding(this, HeightProperty, height);
      BindingOperations.SetBinding(this, LengthProperty, length);
      BindingOperations.SetBinding(this, WidthProperty, width);
    }

    public override void Unbind()
    {
      BindingOperations.ClearBinding(this, HeightProperty);
      BindingOperations.ClearBinding(this, LengthProperty);
      BindingOperations.ClearBinding(this, WidthProperty);
    }

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

    private Tuple<Binding, Binding, Binding> CreateBindings([NotNull] BoxVisual3D source) =>
      Tuple.Create(
        new Binding("Height") { Source = source, Converter = LinearConverter, Mode = BindingMode.TwoWay },
        new Binding("Length") { Source = source, Converter = LinearConverter, Mode = BindingMode.TwoWay },
        new Binding("Width") { Source = source, Converter = LinearConverter, Mode = BindingMode.TwoWay });

    private Tuple<Binding, Binding, Binding> CreateBindings([NotNull] EllipsoidVisual3D source) =>
      Tuple.Create(
        new Binding("RadiusZ") { Source = source, Converter = LinearConverter, Mode = BindingMode.TwoWay },
        new Binding("RadiusX") { Source = source, Converter = LinearConverter, Mode = BindingMode.TwoWay },
        new Binding("RadiusY") { Source = source, Converter = LinearConverter, Mode = BindingMode.TwoWay });

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

    #region Static Services

    private static readonly LinearConverter LinearConverter;
    private static readonly PointsToHeightConverter PointsToHeightConverter;

    #endregion
  }
}
