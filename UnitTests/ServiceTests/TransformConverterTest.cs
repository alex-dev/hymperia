using System;
using System.Linq;
using System.Windows.Media.Media3D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuccincT.Functional;
using Hymperia.Facade;
using Hymperia.Facade.Services;

namespace UnitTests.ServiceTests
{
  [TestClass]
  public class TransformConverterTest
  {
    private readonly Random Random;
    private TransformConverter Converter { get; set; }

    public TransformConverterTest()
    {
      Random = new Random();
    }

    [TestInitialize]
    public void Initialize()
    {
      Converter = TransformConverter.Instance;
    }

    [TestMethod]
    public void ShouldConvertQuaternionAndPointToMatrixTransform3D()
    {
      var point = new Point3D(Random.Next(1000), Random.Next(1000), Random.Next(1000));
      var quaternion = new Quaternion(Random.Next(1000), Random.Next(1000), Random.Next(1000), Random.Next(1000));
      quaternion.Normalize();

      var expected = new Transform3DGroup
      {
        Children = new Transform3DCollection
        {
          new TranslateTransform3D(point.X, point.Y, point.Z),
          new RotateTransform3D(new QuaternionRotation3D(quaternion), point)
        }
      }.Value;
      var test = Converter.Convert(
        new object[] { point.Convert(), quaternion.Convert() },
        typeof(MatrixTransform3D)) as MatrixTransform3D;

      Assert.IsNotNull(test);
      GeometryAssert.AreEqual(expected, test.Value);
    }

    [TestMethod]
    public void ShouldConvertMatrixTransform3DToQuaternionAndPoint()
    {
      var point = new Point3D(Random.Next(1000), Random.Next(1000), Random.Next(1000));
      var quaternion = new Quaternion(Random.Next(1000), Random.Next(1000), Random.Next(1000), Random.Next(1000));
      quaternion.Normalize();

      var transforms = new Transform3DGroup
      {
        Children = new Transform3DCollection
        {
          new TranslateTransform3D(point.X, point.Y, point.Z),
          new RotateTransform3D(new QuaternionRotation3D(quaternion), point)
        }
      };

      var (point_test, (quaternion_test, rest)) = Converter.ConvertBack(new MatrixTransform3D(transforms.Value), Converter.Types);

      GeometryAssert.AreEqual(quaternion, ((Hymperia.Model.Modeles.JsonObject.Quaternion)quaternion_test).Convert());
      GeometryAssert.AreEqual(point, ((Hymperia.Model.Modeles.JsonObject.Point)point_test).Convert());
    }
  }
}
