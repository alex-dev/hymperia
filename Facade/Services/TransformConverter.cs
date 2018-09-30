using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media.Media3D;
using DirectXOperations;
using Object = Hymperia.Model.Modeles.JsonObject;

namespace Hymperia.Facade.Services
{
  public class TransformConverter : IMultiValueConverter
  {
    /// <summary>Le type de la propriété source.</summary>
    public readonly Type[] Types;

    public TransformConverter()
    {
      Types = new Type[]
      {
        typeof(Object.Point),
        typeof(Object.Quaternion)
      };
    }

    /// <inheritdoc />
    /// <remarks>Vers la vue.</remarks>
    public object Convert(object[] values, Type target, object parameter = null, CultureInfo culture = default)
    {
      if (target != typeof(MatrixTransform3D))
      {
        throw new InvalidOperationException("Target is not a MatrixTransform3D.");
      }

      var vector = ((Object.Point)values[0]).ConvertToVector();
      var quaternion = ((Object.Quaternion)values[1]).Convert();

      return new MatrixTransform3D(quaternion.ComposeTransform(ref vector));
    }

    /// <inheritdoc />
    /// <remarks>Vers le modèle.</remarks>
    public object[] ConvertBack(object value, Type[] targets, object parameter = null, CultureInfo culture = default)
    {
      if (!(value is MatrixTransform3D transform))
      {
        throw new InvalidOperationException("Value to convert is not a MatrixTransform3D.");
      }
      if (!Enumerable.SequenceEqual(targets, Types))
      {
        throw new InvalidOperationException("Could not cast values into proper return type.");
      }

      var matrix = transform.Matrix;
      var (vector, quaternion) = matrix.Decompose();

      return new object[] { vector.ConvertToPoint(), quaternion.Convert() };
    }
  }
}
