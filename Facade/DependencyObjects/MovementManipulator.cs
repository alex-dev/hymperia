﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using Hymperia.Facade.Services.PointsToHeightConverters;
using JetBrains.Annotations;

namespace Hymperia.Facade.DependencyObjects.Manipulators
{
  public class MovementManipulator : CombinedManipulator
  {
    #region Dependency Properties

    public static readonly DependencyProperty DiameterProperty;

    #endregion

    #region Properties

    public double Diameter
    {
      get => (double)GetValue(DiameterProperty);
      set => SetValue(DiameterProperty, value);
    }

    #endregion

    #region Constructors

    static MovementManipulator()
    {
      Converter = new DiameterConverter();
      LinearConverter = new LinearConverter();
      PointsToHeightConverter = new PointsToHeightConverter();
      DiameterProperty = DependencyProperty.Register("Diameter", typeof(double), typeof(MovementManipulator),
        new UIPropertyMetadata(2.0, (sender, args) => (sender as MovementManipulator).OnDiameterChanged(args)));
    }

    public MovementManipulator() : base() { }

    protected override IEnumerable<Manipulator> GenerateManipulators()
    {
      yield return new RotateManipulator { Axis = new Vector3D(1, 0, 0), Color = Colors.Red };
      yield return new RotateManipulator { Axis = new Vector3D(0, 1, 0), Color = Colors.Green };
      yield return new RotateManipulator { Axis = new Vector3D(0, 0, 1), Color = Colors.Blue };
      yield return new TranslateManipulator { Direction = new Vector3D(1, 0, 0), Color = Colors.Red };
      yield return new TranslateManipulator { Direction = new Vector3D(0, 1, 0), Color = Colors.Green };
      yield return new TranslateManipulator { Direction = new Vector3D(0, 0, 1), Color = Colors.Blue };
    }

    #endregion

    #region Binding to Source

    public override void Bind(ModelVisual3D source)
    {
      MultiBinding bindings = new MultiBinding() { Mode = BindingMode.OneWay, Converter = Converter };
      bindings.Bindings.AddRange(CreateBindings(source));

      BindingOperations.SetBinding(this, DiameterProperty, bindings);
      BindingOperations.SetBinding(this, TransformProperty, new Binding("Transform") { Source = source });
    }

    public override void Unbind()
    {
      BindingOperations.ClearBinding(this, DiameterProperty);
      BindingOperations.ClearBinding(this, TransformProperty);
    }

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

    private IEnumerable<Binding> CreateBindings([NotNull] BoxVisual3D source)
    {
      yield return new Binding("Height") { Source = source, Mode = BindingMode.OneWay };
      yield return new Binding("Length") { Source = source, Mode = BindingMode.OneWay };
      yield return new Binding("Width") { Source = source, Mode = BindingMode.OneWay };
    }

    private IEnumerable<Binding> CreateBindings([NotNull] EllipsoidVisual3D source)
    {
      yield return new Binding("RadiusX")
      {
        Source = source,
        Converter = LinearConverter,
        ConverterParameter = 2,
        Mode = BindingMode.OneWay
      };
      yield return new Binding("RadiusY")
      {
        Source = source,
        Converter = LinearConverter,
        ConverterParameter = 2,
        Mode = BindingMode.OneWay
      };
      yield return new Binding("RadiusZ")
      {
        Source = source,
        Converter = LinearConverter,
        ConverterParameter = 2,
        Mode = BindingMode.OneWay
      };
    }

    private IEnumerable<Binding> CreateBindings([NotNull] PipeVisual3D source)
    {
      yield return new Binding("Diameter") { Source = source, Mode = BindingMode.OneWay };
      yield return new Binding("Point2")
      {
        Source = source,
        Converter = PointsToHeightConverter,
        ConverterParameter = PointOrientation.Top,
        Mode = BindingMode.TwoWay
      };
    }

    private IEnumerable<Binding> CreateBindings([NotNull] TruncatedConeVisual3D source)
    {
      yield return new Binding("Height") { Source = source, Mode = BindingMode.OneWay };
      yield return new Binding("BaseRadius")
      {
        Source = source,
        Converter = LinearConverter,
        ConverterParameter = 2,
        Mode = BindingMode.OneWay
      };
      yield return new Binding("TopRadius")
      {
        Source = source,
        Converter = LinearConverter,
        ConverterParameter = 2,
        Mode = BindingMode.OneWay
      };
    }

    #endregion

    #region Sizing

    protected virtual void OnDiameterChanged(DependencyPropertyChangedEventArgs args)
    {
      foreach (Manipulator manipulator in Children)
      {
        Resize(manipulator, (double)args.NewValue);
      }
    }

    /// <remarks><see cref="ConvertBack(object, Type[], object, CultureInfo)"/> n'est pas implémenté parce que la transformation est un processus destructif.</remarks>
    private class DiameterConverter : IMultiValueConverter
    {
      /// <inheritdoc />
      public object Convert(object[] values, Type target, object parameter = null, CultureInfo culture = default) =>
        ChangeType((from double value in values select Math.Abs(value)).Max(), target);

      /// <inheritdoc />
      public object[] ConvertBack(object value, Type[] targets, object parameter = null, CultureInfo culture = default) => throw new NotImplementedException();

      private object ChangeType(object value, Type target) => System.Convert.ChangeType(value, target);
    }

    #endregion

    #region Static Services

    private static readonly DiameterConverter Converter;
    private static readonly LinearConverter LinearConverter;
    private static readonly PointsToHeightConverter PointsToHeightConverter;

    #endregion
  }
}
