﻿using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media.Media3D;
using DirectXOperations;
using Hymperia.Facade.Properties;
using Hymperia.Facade.Extensions;
using JetBrains.Annotations;
using Object = Hymperia.Model.Modeles.JsonObject;

namespace Hymperia.Facade.Converters
{
  /// <summary>Convertit des <see cref="Object.Point"/> et <see cref="Object.Quaternion"/> en <see cref="Transform3D"/>.</summary>
  public sealed class TransformConverter : IMultiValueConverter
  {
    /// <summary>Le type de la propriété source.</summary>
    [NotNull]
    [ItemNotNull]
    public readonly Type[] Types = new Type[]
    {
      typeof(Object.Point),
      typeof(Object.Quaternion)
    };

    /// inheritdoc/>
    /// <remarks>Vers la vue.</remarks>
    public object Convert(object[] values, Type target, object parameter = null, CultureInfo culture = default)
    {
      if (!typeof(MatrixTransform3D).IsSubclassOf(target) && typeof(MatrixTransform3D) != target)
        throw new ArgumentException(Resources.CanOnlyConvertTo(nameof(MatrixTransform3D)), nameof(target));

      var vector = ((Object.Point)values[0]).ConvertToVector();
      var quaternion = ((Object.Quaternion)values[1]).Convert();

      return new MatrixTransform3D(quaternion.ComposeTransform(ref vector));
    }

    /// inheritdoc/>
    /// <remarks>Vers le modèle.</remarks>
    public object[] ConvertBack(object value, Type[] targets, object parameter = null, CultureInfo culture = default)
    {
      if (!Enumerable.SequenceEqual(targets, Types))
        throw new ArgumentException(Resources.CanOnlyConvertTo(Types), nameof(targets));

      if (!(value is Transform3D transform))
        throw new ArgumentException(Resources.CanOnlyConvertFrom(nameof(Transform3D)), nameof(value));

      var matrix = transform.Value;
      var (vector, quaternion) = matrix.Decompose();

      return new object[] { vector.ConvertToPoint(), quaternion.Convert() };
    }
  }
}
