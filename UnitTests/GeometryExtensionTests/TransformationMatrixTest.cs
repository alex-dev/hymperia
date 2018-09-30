using System;
using System.Windows.Media.Media3D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DirectXOperations;

namespace UnitTests.GeometryExtensionTests
{
  [TestClass]
  public class TransformationMatrixTest
  {
    [TestMethod]
    public void ShouldComposeIdentityMatrix()
    {
      var quaternion = Quaternion.Identity;
      var vector = new Vector3D { };
      var expected = Matrix3D.Identity;

      GeometryAssert.AreEqual(expected, vector.ComposeTransform(ref quaternion));
      GeometryAssert.AreEqual(expected, quaternion.ComposeTransform(ref vector));
    }

    [TestMethod]
    public void ShouldComposeTranslationMatrix()
    {
      var quaternion = Quaternion.Identity;
      var vector = new Vector3D { X = 5, Y = 7, Z = -3 };
      var expected = Matrix3D.Identity;
      expected.Translate(vector);

      GeometryAssert.AreEqual(expected, vector.ComposeTransform(ref quaternion));
      GeometryAssert.AreEqual(expected, quaternion.ComposeTransform(ref vector));
    }

    [TestMethod]
    public void ShouldComposeRotationMatrix()
    {
      var quaternion = new Quaternion { X = 3, Y = 4, Z = 2, W = 0.5 };
      var vector = new Vector3D { };
      var expected = Matrix3D.Identity;
      expected.Rotate(quaternion);

      GeometryAssert.AreEqual(expected, vector.ComposeTransform(ref quaternion));
      GeometryAssert.AreEqual(expected, quaternion.ComposeTransform(ref vector));
    }

    [TestMethod]
    public void ShouldComposeTranslationRotationMatrix()
    {
      var quaternion = new Quaternion { X = 3, Y = 4, Z = 2, W = 0.5 };
      var vector = new Vector3D { X = 5, Y = 7, Z = -3 };
      var expected = Matrix3D.Identity;
      expected.Rotate(quaternion);
      expected.Translate(vector);

      GeometryAssert.AreEqual(expected, vector.ComposeTransform(ref quaternion));
      GeometryAssert.AreEqual(expected, quaternion.ComposeTransform(ref vector));
    }

    [TestMethod]
    public void ShouldDecomposeIdentityMatrix()
    {
      var expected_quaternion = Quaternion.Identity;
      var expected_vector = new Vector3D { };
      var matrix = Matrix3D.Identity;

      var (result_vector, result_quaternion) = matrix.Decompose();

      GeometryAssert.AreEqual(expected_quaternion, result_quaternion);
      GeometryAssert.AreEqual(expected_vector, result_vector);
    }

    [TestMethod]
    public void ShouldDecomposeTranslationMatrix()
    {
      var expected_quaternion = Quaternion.Identity;
      var expected_vector = new Vector3D { X = 5, Y = 7, Z = -3 };
      var matrix = Matrix3D.Identity;
      matrix.Translate(expected_vector);

      var (result_vector, result_quaternion) = matrix.Decompose();

      GeometryAssert.AreEqual(expected_quaternion, result_quaternion);
      GeometryAssert.AreEqual(expected_vector, result_vector);
    }

    [TestMethod]
    public void ShouldDecomposeRotationMatrix()
    {
      var expected_quaternion = new Quaternion { X = 0.5, Y = 1, Z = 0.3, W = 0.5 };
      var expected_vector = new Vector3D { };
      var matrix = Matrix3D.Identity;
      expected_quaternion.Normalize();
      matrix.Rotate(expected_quaternion);

      var (result_vector, result_quaternion) = matrix.Decompose();

      GeometryAssert.AreEqual(expected_quaternion, result_quaternion);
      GeometryAssert.AreEqual(expected_vector, result_vector);
    }

    [TestMethod]
    public void ShouldDecomposeTranslationRotationMatrix()
    {
      var expected_quaternion = new Quaternion { X = 3, Y = 4, Z = 2, W = 0.5 };
      var expected_vector = new Vector3D { X = 5, Y = 7, Z = -3 };
      var matrix = Matrix3D.Identity;
      expected_quaternion.Normalize();
      matrix.Rotate(expected_quaternion);
      matrix.Translate(expected_vector);

      var (result_vector, result_quaternion) = matrix.Decompose();

      GeometryAssert.AreEqual(expected_quaternion, result_quaternion);
      GeometryAssert.AreEqual(expected_vector, result_vector);
    }
  }
}
