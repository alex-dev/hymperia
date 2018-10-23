using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using Hymperia.Facade.Converters;
using Hymperia.Facade.Converters.AggregateConverters;
using JetBrains.Annotations;

namespace Hymperia.Facade.DependencyObjects.Manipulators
{
  /// <summary>Classe de base de tous les groupes de <see cref="Manipulator"/> utilisés.</summary>
  public abstract class CombinedManipulator : ModelVisual3D
  {
    #region Dependency Properties

    /// <seealso cref=Radius/>
    public static readonly DependencyProperty RadiusProperty =
      DependencyProperty.Register(nameof(Radius), typeof(double), typeof(CombinedManipulator), new PropertyMetadata(2.0));

    #endregion

    #region Properties

    /// <summary>Le rayon du manipulateur.</summary>
    public double Radius
    {
      get => (double)GetValue(RadiusProperty);
      set => SetValue(RadiusProperty, value);
    }

    /// <summary>Le <see cref="Transform3D"/> appliqué à la cible du manipulateur.</summary>
    [NotNull]
    public Transform3D TargetTransform
    {
      get => (Transform3D)GetValue(TransformProperty);
      set => SetValue(TransformProperty, value);
    }

    #endregion

    #region Constructors

    [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors",
      Justification = @"The call is known and needed to perform proper initialization.")]
    public CombinedManipulator()
    {
      var binding = new Binding(nameof(Transform)) { Source = this };

      foreach (var manipulator in GenerateManipulators())
      {
        SetBinding(manipulator, Manipulator.TargetTransformProperty, binding);
        Children.Add(manipulator);
      }
    }

    /// <summary>Génère les <see cref="Manipulator"/> et des <see cref="Action{Manipulator}"/> pour les lier au <see cref="CombinedManipulator"/>.</summary>
    /// <returns>
    ///   Un <see cref="IEnumerable{Tuple{Manipulator, Action{Manipulator}}}"/> de <see cref="Manipulator"/> et
    ///   d'<see cref="Action{Manipulator}"/> pour le lier.
    /// </returns>
    [NotNull]
    [ItemNotNull]
    protected abstract IEnumerable<Manipulator> GenerateManipulators();

    #endregion

    #region Binding to Manipulators Size

    protected void BindToManipulator([NotNull] RotateManipulator manipulator)
    {
      SetBinding(manipulator, RotateManipulator.DiameterProperty, new Binding(nameof(Radius))
      {
        Source = this,
        Converter = LinearConverter,
        ConverterParameter = 2.15,
        Mode = BindingMode.OneWay
      });
      SetBinding(manipulator, RotateManipulator.InnerDiameterProperty, new Binding(nameof(Radius))
      {
        Source = this,
        Converter = LinearConverter,
        ConverterParameter = 2.1,
        Mode = BindingMode.OneWay
      });
      SetBinding(manipulator, RotateManipulator.LengthProperty, new Binding(nameof(Radius))
      {
        Source = this,
        Converter = LinearConverter,
        ConverterParameter = 0.15,
        Mode = BindingMode.OneWay
      });
    }

    protected void BindToManipulator([NotNull] TranslateManipulator manipulator)
    {
      SetBinding(manipulator, TranslateManipulator.LengthProperty, new Binding(nameof(Radius))
      {
        Source = this,
        Converter = LinearConverter,
        ConverterParameter = 1.5,
        Mode = BindingMode.OneWay
      });
      SetBinding(manipulator, TranslateManipulator.DiameterProperty, new Binding(nameof(Radius))
      {
        Source = this,
        Converter = LinearConverter,
        ConverterParameter = 0.15,
        Mode = BindingMode.OneWay
      });
    }

    protected void BindToManipulator([NotNull] ScaleManipulator manipulator)
    {
      SetBinding(manipulator, ScaleManipulator.LengthProperty, new Binding(nameof(Radius))
      {
        Source = this,
        Converter = LinearConverter,
        ConverterParameter = 1.5,
        Mode = BindingMode.OneWay
      });
      SetBinding(manipulator, ScaleManipulator.DiameterProperty, new Binding(nameof(Radius))
      {
        Source = this,
        Converter = LinearConverter,
        ConverterParameter = 0.15,
        Mode = BindingMode.OneWay
      });
    }

    #endregion

    #region Binding from Source

    /// <summary>
    ///   Lie la <paramref name="source"/> au <see cref="CombinedManipulator"/> pour appliquer à <paramref name="source"/> les
    ///   transformations du <see cref="CombinedManipulator"/>.
    /// </summary>
    public virtual void Bind([NotNull] ModelVisual3D source)
    {
      var bindings = new MultiBinding() { Mode = BindingMode.OneWay, Converter = RadiusConverter, NotifyOnTargetUpdated = true };
      bindings.Bindings.AddRange(CreateBindings(source));

      SetBinding(RadiusProperty, bindings);
    }

    /// <summary>Délie le <see cref="ModelVisual3D"/> déjà lié au <see cref="CombinedManipulator"/>.</summary>
    public virtual void Unbind() =>
      ClearBinding(RadiusProperty);

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
        ConverterParameter = 1.75,
        Mode = BindingMode.OneWay
      };
      yield return new Binding(nameof(source.RadiusY))
      {
        Source = source,
        Converter = LinearConverter,
        ConverterParameter = 1.75,
        Mode = BindingMode.OneWay
      };
      yield return new Binding(nameof(source.RadiusZ))
      {
        Source = source,
        Converter = LinearConverter,
        ConverterParameter = 1.75,
        Mode = BindingMode.OneWay
      };
    }

    [NotNull]
    [ItemNotNull]
    private IEnumerable<Binding> CreateBindings([NotNull] CylinderVisual3D source)
    {
      yield return new Binding(nameof(source.Diameter)) { Source = source, Mode = BindingMode.OneWay };
      yield return new Binding(nameof(source.Height)) { Source = source, Mode = BindingMode.OneWay };
    }

    [NotNull]
    [ItemNotNull]
    private IEnumerable<Binding> CreateBindings([NotNull] TruncatedConeVisual3D source)
    {
      yield return new Binding(nameof(source.Height))
      {
        Source = source,
        Converter = LinearConverter,
        ConverterParameter = 1.1,
        Mode = BindingMode.OneWay
      };
      yield return new Binding(nameof(source.BaseRadius))
      {
        Source = source,
        Converter = LinearConverter,
        ConverterParameter = 1.75,
        Mode = BindingMode.OneWay
      };
      yield return new Binding(nameof(source.TopRadius))
      {
        Source = source,
        Converter = LinearConverter,
        ConverterParameter = 1.75,
        Mode = BindingMode.OneWay
      };
    }

    #endregion

    #region Binding Operations

    [NotNull]
    protected BindingBase GetBinding([NotNull] DependencyProperty property) => GetBinding(this, property);
    protected BindingBase GetBinding([NotNull] DependencyObject @object, [NotNull] DependencyProperty property) =>
      BindingOperations.GetBinding(@object, property);

    [NotNull]
    protected BindingExpressionBase SetBinding([NotNull] DependencyProperty property, [NotNull] BindingBase binding) =>
      SetBinding(this, property, binding);
    [NotNull]
    protected BindingExpressionBase SetBinding([NotNull] DependencyObject @object, [NotNull] DependencyProperty property, [NotNull] BindingBase binding) =>
      BindingOperations.SetBinding(@object, property, binding);

    protected void ClearBinding([NotNull] DependencyProperty property) => ClearBinding(this, property);
    protected void ClearBinding([NotNull] DependencyObject @object, [NotNull] DependencyProperty property) =>
      BindingOperations.ClearBinding(@object, property);

    #endregion

    #region Static Services

    [NotNull]
    protected static readonly ManipulatorRadiusConverter RadiusConverter =
      (ManipulatorRadiusConverter)Application.Current.Resources["ManipulatorRadius"];
    [NotNull]
    protected static readonly MaxAggregateConverter MaxConverter =
      (MaxAggregateConverter)Application.Current.Resources["MaxAggregate"];
    [NotNull]
    protected static readonly LinearConverter LinearConverter =
      (LinearConverter)Application.Current.Resources["Linear"];

    #endregion
  }
}
