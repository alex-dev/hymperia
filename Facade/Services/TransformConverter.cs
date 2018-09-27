using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media.Media3D;

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
        typeof(Model.Modeles.JsonObject.Point),
        typeof(Model.Modeles.JsonObject.Quaternion)
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

      var matrix = new Matrix3D();
      Translate(ref matrix, (Model.Modeles.JsonObject.Point)values[0]);
      Rotate(ref matrix, (Model.Modeles.JsonObject.Quaternion)values[1], (Model.Modeles.JsonObject.Point)values[0]);

      return new MatrixTransform3D(matrix);
    }

    private void Translate(ref Matrix3D matrix, Model.Modeles.JsonObject.Point point)
    {
      matrix.Translate(new Vector3D(point.X, point.Y, point.Z));
    }

    private void Rotate(ref Matrix3D matrix, Model.Modeles.JsonObject.Quaternion quaternion, Model.Modeles.JsonObject.Point center)
    {
      matrix.RotateAt(quaternion.Convert(), new Point3D(center.X, center.Y, center.Z));
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
        throw new InvalidCastException("Could not cast values into proper return type.");
      }

      throw new NotImplementedException();
    }

    /*public Tuple<Point3D, Quaternion> Convert(Matrix3D matrix)
    {
      var quaternion;
      var point;

      return Tuple.Create(point, quaternion);
    }

    private Point3D InverseTranslate(ref Matrix3D matrix)
    {
      X = (m11 - m11p) / m14
      X = (m21 - m21p) / m24
      X = (m31 - m31p) / m34
      X = (m41 - m41p) / m44

      _m11 += _m14 * offset.X; _m12 += _m14 * offset.Y; _m13 += _m14 * offset.Z;
      _m21 += _m24 * offset.X; _m22 += _m24 * offset.Y; _m23 += _m24 * offset.Z;
      _m31 += _m34 * offset.X; _m32 += _m34 * offset.Y; _m33 += _m34 * offset.Z;
      _offsetX += _m44 * offset.X; _offsetY += _m44 * offset.Y; _offsetZ += _m44 * offset.Z;
    }

    private object ConvertPoint(MatrixTransform3D transform) => throw new NotImplementedException();*/
  }
}
