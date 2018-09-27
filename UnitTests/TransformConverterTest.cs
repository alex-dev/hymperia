using System;
using System.Linq;
using System.Windows.Media.Media3D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuccincT.Functional;
using Hymperia.Facade;
using Hymperia.Facade.Services;

namespace UnitTests
{
  [TestClass]
  public class TransformConverterTest
  {
    private readonly Random Random;

    public TransformConverterTest()
    {
      Random = new Random();
    }

    [TestMethod]
    public void ShouldConvertQuaternionAndPointsToMatrixTransform3D()
    {
      var service = new TransformConverter();
      var quaternion = new Quaternion(Random.Next(), Random.Next(), Random.Next(), Random.Next());
      var point = new Point3D(Random.Next(), Random.Next(), Random.Next());

      var subject = new Point3D(Random.Next(), Random.Next(), Random.Next());
      var subjects_expected = new Point3D[]
      {
        new Point3D(Random.Next(), Random.Next(), Random.Next()),
        new Point3D(Random.Next(), Random.Next(), Random.Next())
      };
      var subjects_tested = new Point3D[2];
      subjects_expected.CopyTo(subjects_tested, 0);

      var transforms = new Transform3DGroup
      {
        Children = new Transform3DCollection
        {
          new TranslateTransform3D(point.X, point.Y, point.Z),
          new RotateTransform3D(new QuaternionRotation3D(quaternion), point)
        }
      };
      var test = service.Convert(
        new object[] { point.Convert(), quaternion.Convert() },
        typeof(MatrixTransform3D)) as MatrixTransform3D;

      Assert.IsNotNull(test);
      Assert.AreEqual(transforms.Transform(subject), test.Transform(subject));
      transforms.Transform(subjects_expected);
      test.Transform(subjects_tested);
      Assert.IsTrue(Enumerable.SequenceEqual(subjects_expected, subjects_tested));
    }

    [TestMethod]
    public void ShouldConvertMatrixTransform3DToQuaternionAndPoints()
    {
      var service = new TransformConverter();
      var quaternion = new Quaternion(Random.Next(), Random.Next(), Random.Next(), Random.Next());
      var point = new Point3D(Random.Next(), Random.Next(), Random.Next());

      var transforms = new Transform3DGroup
      {
        Children = new Transform3DCollection
        {
          new TranslateTransform3D(point.X, point.Y, point.Z),
          new RotateTransform3D(new QuaternionRotation3D(quaternion), point)
        }
      };

      var (point_test, (quaternion_test, rest)) = service.ConvertBack(new MatrixTransform3D(transforms.Value), service.Types);

      Assert.AreEqual(quaternion, ((Hymperia.Model.Modeles.JsonObject.Quaternion)quaternion_test).Convert());
      Assert.AreEqual(point, ((Hymperia.Model.Modeles.JsonObject.Point)point_test).Convert());
    }
  }
}
