using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Media3D;

namespace Hymperia.Facade.Services
{
  public class TransformConverter : IMultiValueConverter
  {
    /// <inheritdoc />
    /// <remarks>Vers la vue.</remarks>
    public object Convert(object[] values, Type target, object parameter, CultureInfo culture)
    {
      if (target != typeof(MatrixTransform3D))
      {
        throw new InvalidOperationException("Target is not a MatrixTransform3D.");
      }

      var matrix = new Matrix3D();

      foreach (var value in values)
      {
        Convert(value, matrix);
      }

      return new MatrixTransform3D(matrix);
    }

    public void Convert(object value, Matrix3D matrix)
    {
      switch (value)
      {
        case Model.Modeles.JsonObject.Point point:
          // Fonctionne en assumant que le centre de l'objet est { 0, 0, 0 }.
          matrix.Translate(new Vector3D(point.X, point.Y, point.Z));
          break;
        case Model.Modeles.JsonObject.Quaternion quaternion:
          matrix.Rotate(quaternion.Convert());
          break;
      }
    }

    /// <inheritdoc />
    /// <remarks>Vers le modèle.</remarks>
    public object[] ConvertBack(object value, Type[] targets, object parameter, CultureInfo culture)
    {
      if (!(value is MatrixTransform3D transform))
      {
        throw new InvalidOperationException("Value to convert is not a MatrixTransform3D.");
      }

      throw new NotImplementedException();
    }

    public object Convert(MatrixTransform3D transform, Type type)
    {
      if (type == typeof(Model.Modeles.JsonObject.Point))
        return ConvertPoint(transform);
      else if (type == typeof(Model.Modeles.JsonObject.Quaternion))
        return ConvertQuaternion(transform);
      else
        return null;
    }

    private object ConvertQuaternion(MatrixTransform3D transform)
    {
      throw new NotImplementedException();
    }

    private object ConvertPoint(MatrixTransform3D transform) => throw new NotImplementedException();
  }
}
