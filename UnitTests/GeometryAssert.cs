using System.Windows.Media.Media3D;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
  internal static class GeometryAssert
  {
    private const double Delta = 0.0005;

    public static void AreEqual(Point3D expected, Point3D actual, double delta = Delta)
    {
      try
      {
        Assert.AreEqual(expected.X, actual.X, delta);
        Assert.AreEqual(expected.Y, actual.Y, delta);
        Assert.AreEqual(expected.Z, actual.Z, delta);
      }
      catch (AssertFailedException)
      {
        Assert.Fail($"Expected { expected } but received { actual }.");
      }
    }

    public static void AreEqual(Point3D expected, Point3D actual, double delta, string message)
    {
      try
      {
        Assert.AreEqual(expected.X, actual.X, delta);
        Assert.AreEqual(expected.Y, actual.Y, delta);
        Assert.AreEqual(expected.Z, actual.Z, delta);
      }
      catch (AssertFailedException)
      {
        Assert.Fail(message);
      }
    }

    public static void AreEqual(Point3D expected, Point3D actual, double delta, string message, params object[] parameters)
    {
      try
      {
        Assert.AreEqual(expected.X, actual.X, delta);
        Assert.AreEqual(expected.Y, actual.Y, delta);
        Assert.AreEqual(expected.Z, actual.Z, delta);
      }
      catch (AssertFailedException)
      {
        Assert.Fail(message, parameters);
      }
    }

    public static void AreNotEqual(Point3D expected, Point3D actual, double delta = Delta)
    {
      try
      {
        Assert.AreNotEqual(expected.X, actual.X, delta);
        Assert.AreNotEqual(expected.Y, actual.Y, delta);
        Assert.AreNotEqual(expected.Z, actual.Z, delta);
      }
      catch (AssertFailedException)
      {
        Assert.Fail($"Did not expect { expected } and received { actual }.");
      }
    }

    public static void AreNotEqual(Point3D expected, Point3D actual, double delta, string message)
    {
      try
      {
        Assert.AreNotEqual(expected.X, actual.X, delta);
        Assert.AreNotEqual(expected.Y, actual.Y, delta);
        Assert.AreNotEqual(expected.Z, actual.Z, delta);
      }
      catch (AssertFailedException)
      {
        Assert.Fail(message);
      }
    }

    public static void AreNotEqual(Point3D expected, Point3D actual, double delta, string message, params object[] parameters)
    {
      try
      {
        Assert.AreNotEqual(expected.X, actual.X, delta);
        Assert.AreNotEqual(expected.Y, actual.Y, delta);
        Assert.AreNotEqual(expected.Z, actual.Z, delta);
      }
      catch (AssertFailedException)
      {
        Assert.Fail(message, parameters);
      }
    }

    public static void AreEqual(Vector3D expected, Vector3D actual, double delta = Delta)
    {
      try
      {
        Assert.AreEqual(expected.X, actual.X, delta);
        Assert.AreEqual(expected.Y, actual.Y, delta);
        Assert.AreEqual(expected.Z, actual.Z, delta);
      }
      catch (AssertFailedException)
      {
        Assert.Fail($"Expected { expected } but received { actual }.");
      }
    }

    public static void AreEqual(Vector3D expected, Vector3D actual, double delta, string message)
    {
      try
      {
        Assert.AreEqual(expected.X, actual.X, delta);
        Assert.AreEqual(expected.Y, actual.Y, delta);
        Assert.AreEqual(expected.Z, actual.Z, delta);
      }
      catch (AssertFailedException)
      {
        Assert.Fail(message);
      }
    }

    public static void AreEqual(Vector3D expected, Vector3D actual, double delta, string message, params object[] parameters)
    {
      try
      {
        Assert.AreEqual(expected.X, actual.X, delta);
        Assert.AreEqual(expected.Y, actual.Y, delta);
        Assert.AreEqual(expected.Z, actual.Z, delta);
      }
      catch (AssertFailedException)
      {
        Assert.Fail(message, parameters);
      }
    }

    public static void AreNotEqual(Vector3D expected, Vector3D actual, double delta = Delta)
    {
      try
      {
        Assert.AreNotEqual(expected.X, actual.X, delta);
        Assert.AreNotEqual(expected.Y, actual.Y, delta);
        Assert.AreNotEqual(expected.Z, actual.Z, delta);
      }
      catch (AssertFailedException)
      {
        Assert.Fail($"Did not expect { expected } and received { actual }.");
      }
    }

    public static void AreNotEqual(Vector3D expected, Vector3D actual, double delta, string message)
    {
      try
      {
        Assert.AreNotEqual(expected.X, actual.X, delta);
        Assert.AreNotEqual(expected.Y, actual.Y, delta);
        Assert.AreNotEqual(expected.Z, actual.Z, delta);
      }
      catch (AssertFailedException)
      {
        Assert.Fail(message);
      }
    }

    public static void AreNotEqual(Vector3D expected, Vector3D actual, double delta, string message, params object[] parameters)
    {
      try
      {
        Assert.AreNotEqual(expected.X, actual.X, delta);
        Assert.AreNotEqual(expected.Y, actual.Y, delta);
        Assert.AreNotEqual(expected.Z, actual.Z, delta);
      }
      catch (AssertFailedException)
      {
        Assert.Fail(message, parameters);
      }
    }

    public static void AreEqual(Quaternion expected, Quaternion actual, double delta = Delta)
    {
      try
      {
        Assert.AreEqual(expected.X, actual.X, delta);
        Assert.AreEqual(expected.Y, actual.Y, delta);
        Assert.AreEqual(expected.Z, actual.Z, delta);
        Assert.AreEqual(expected.W, actual.W, delta);
      }
      catch (AssertFailedException)
      {
        Assert.Fail($"Expected { expected } but received { actual }.");
      }
    }

    public static void AreEqual(Quaternion expected, Quaternion actual, double delta, string message)
    {
      try
      {
        Assert.AreEqual(expected.X, actual.X, delta);
        Assert.AreEqual(expected.Y, actual.Y, delta);
        Assert.AreEqual(expected.Z, actual.Z, delta);
        Assert.AreEqual(expected.W, actual.W, delta);
      }
      catch (AssertFailedException)
      {
        Assert.Fail(message);
      }
    }

    public static void AreEqual(Quaternion expected, Quaternion actual, double delta, string message, params object[] parameters)
    {
      try
      {
        Assert.AreEqual(expected.X, actual.X, delta);
        Assert.AreEqual(expected.Y, actual.Y, delta);
        Assert.AreEqual(expected.Z, actual.Z, delta);
        Assert.AreEqual(expected.W, actual.W, delta);
      }
      catch (AssertFailedException)
      {
        Assert.Fail(message, parameters);
      }
    }

    public static void AreNotEqual(Quaternion expected, Quaternion actual, double delta = Delta)
    {
      try
      {
        Assert.AreNotEqual(expected.X, actual.X, delta);
        Assert.AreNotEqual(expected.Y, actual.Y, delta);
        Assert.AreNotEqual(expected.Z, actual.Z, delta);
        Assert.AreNotEqual(expected.W, actual.W, delta);
      }
      catch (AssertFailedException)
      {
        Assert.Fail($"Did not expect { expected } and received { actual }.");
      }
    }

    public static void AreNotEqual(Quaternion expected, Quaternion actual, double delta, string message)
    {
      try
      {
        Assert.AreNotEqual(expected.X, actual.X, delta);
        Assert.AreNotEqual(expected.Y, actual.Y, delta);
        Assert.AreNotEqual(expected.Z, actual.Z, delta);
        Assert.AreNotEqual(expected.W, actual.W, delta);
      }
      catch (AssertFailedException)
      {
        Assert.Fail(message);
      }
    }

    public static void AreNotEqual(Quaternion expected, Quaternion actual, double delta, string message, params object[] parameters)
    {
      try
      {
        Assert.AreNotEqual(expected.X, actual.X, delta);
        Assert.AreNotEqual(expected.Y, actual.Y, delta);
        Assert.AreNotEqual(expected.Z, actual.Z, delta);
        Assert.AreNotEqual(expected.W, actual.W, delta);
      }
      catch (AssertFailedException)
      {
        Assert.Fail(message, parameters);
      }
    }

    public static void AreEqual(Matrix3D expected, Matrix3D actual, double delta = Delta)
    {
      try
      {
        Assert.AreEqual(expected.M11, actual.M11, delta);
        Assert.AreEqual(expected.M12, actual.M12, delta);
        Assert.AreEqual(expected.M13, actual.M13, delta);
        Assert.AreEqual(expected.M14, actual.M14, delta);
        Assert.AreEqual(expected.M21, actual.M21, delta);
        Assert.AreEqual(expected.M22, actual.M22, delta);
        Assert.AreEqual(expected.M23, actual.M23, delta);
        Assert.AreEqual(expected.M24, actual.M24, delta);
        Assert.AreEqual(expected.M31, actual.M31, delta);
        Assert.AreEqual(expected.M32, actual.M32, delta);
        Assert.AreEqual(expected.M33, actual.M33, delta);
        Assert.AreEqual(expected.M34, actual.M34, delta);
        Assert.AreEqual(expected.OffsetX, actual.OffsetX, delta);
        Assert.AreEqual(expected.OffsetY, actual.OffsetY, delta);
        Assert.AreEqual(expected.OffsetZ, actual.OffsetZ, delta);
        Assert.AreEqual(expected.M44, actual.M44, delta);
      }
      catch (AssertFailedException)
      {
        Assert.Fail($"Expected { expected } but received { actual }.");
      }
    }

    public static void AreEqual(Matrix3D expected, Matrix3D actual, double delta, string message)
    {
      try
      {
        Assert.AreEqual(expected.M11, actual.M11, delta);
        Assert.AreEqual(expected.M12, actual.M12, delta);
        Assert.AreEqual(expected.M13, actual.M13, delta);
        Assert.AreEqual(expected.M14, actual.M14, delta);
        Assert.AreEqual(expected.M21, actual.M21, delta);
        Assert.AreEqual(expected.M22, actual.M22, delta);
        Assert.AreEqual(expected.M23, actual.M23, delta);
        Assert.AreEqual(expected.M24, actual.M24, delta);
        Assert.AreEqual(expected.M31, actual.M31, delta);
        Assert.AreEqual(expected.M32, actual.M32, delta);
        Assert.AreEqual(expected.M33, actual.M33, delta);
        Assert.AreEqual(expected.M34, actual.M34, delta);
        Assert.AreEqual(expected.OffsetX, actual.OffsetX, delta);
        Assert.AreEqual(expected.OffsetY, actual.OffsetY, delta);
        Assert.AreEqual(expected.OffsetZ, actual.OffsetZ, delta);
        Assert.AreEqual(expected.M44, actual.M44, delta);
      }
      catch (AssertFailedException)
      {
        Assert.Fail(message);
      }
    }

    public static void AreEqual(Matrix3D expected, Matrix3D actual, double delta, string message, params object[] parameters)
    {
      try
      {
        Assert.AreEqual(expected.M11, actual.M11, delta);
        Assert.AreEqual(expected.M12, actual.M12, delta);
        Assert.AreEqual(expected.M13, actual.M13, delta);
        Assert.AreEqual(expected.M14, actual.M14, delta);
        Assert.AreEqual(expected.M21, actual.M21, delta);
        Assert.AreEqual(expected.M22, actual.M22, delta);
        Assert.AreEqual(expected.M23, actual.M23, delta);
        Assert.AreEqual(expected.M24, actual.M24, delta);
        Assert.AreEqual(expected.M31, actual.M31, delta);
        Assert.AreEqual(expected.M32, actual.M32, delta);
        Assert.AreEqual(expected.M33, actual.M33, delta);
        Assert.AreEqual(expected.M34, actual.M34, delta);
        Assert.AreEqual(expected.OffsetX, actual.OffsetX, delta);
        Assert.AreEqual(expected.OffsetY, actual.OffsetY, delta);
        Assert.AreEqual(expected.OffsetZ, actual.OffsetZ, delta);
        Assert.AreEqual(expected.M44, actual.M44, delta);
      }
      catch (AssertFailedException)
      {
        Assert.Fail(message, parameters);
      }
    }

    public static void AreNotEqual(Matrix3D expected, Matrix3D actual, double delta = Delta)
    {
      try
      {
        Assert.AreNotEqual(expected.M11, actual.M11, delta);
        Assert.AreNotEqual(expected.M12, actual.M12, delta);
        Assert.AreNotEqual(expected.M13, actual.M13, delta);
        Assert.AreNotEqual(expected.M14, actual.M14, delta);
        Assert.AreNotEqual(expected.M21, actual.M21, delta);
        Assert.AreNotEqual(expected.M22, actual.M22, delta);
        Assert.AreNotEqual(expected.M23, actual.M23, delta);
        Assert.AreNotEqual(expected.M24, actual.M24, delta);
        Assert.AreNotEqual(expected.M31, actual.M31, delta);
        Assert.AreNotEqual(expected.M32, actual.M32, delta);
        Assert.AreNotEqual(expected.M33, actual.M33, delta);
        Assert.AreNotEqual(expected.M34, actual.M34, delta);
        Assert.AreNotEqual(expected.OffsetX, actual.OffsetX, delta);
        Assert.AreNotEqual(expected.OffsetY, actual.OffsetY, delta);
        Assert.AreNotEqual(expected.OffsetZ, actual.OffsetZ, delta);
        Assert.AreNotEqual(expected.M44, actual.M44, delta);
      }
      catch (AssertFailedException)
      {
        Assert.Fail($"Did not expect { expected } and received { actual }.");
      }
    }

    public static void AreNotEqual(Matrix3D expected, Matrix3D actual, double delta, string message)
    {
      try
      {
        Assert.AreNotEqual(expected.M11, actual.M11, delta);
        Assert.AreNotEqual(expected.M12, actual.M12, delta);
        Assert.AreNotEqual(expected.M13, actual.M13, delta);
        Assert.AreNotEqual(expected.M14, actual.M14, delta);
        Assert.AreNotEqual(expected.M21, actual.M21, delta);
        Assert.AreNotEqual(expected.M22, actual.M22, delta);
        Assert.AreNotEqual(expected.M23, actual.M23, delta);
        Assert.AreNotEqual(expected.M24, actual.M24, delta);
        Assert.AreNotEqual(expected.M31, actual.M31, delta);
        Assert.AreNotEqual(expected.M32, actual.M32, delta);
        Assert.AreNotEqual(expected.M33, actual.M33, delta);
        Assert.AreNotEqual(expected.M34, actual.M34, delta);
        Assert.AreNotEqual(expected.OffsetX, actual.OffsetX, delta);
        Assert.AreNotEqual(expected.OffsetY, actual.OffsetY, delta);
        Assert.AreNotEqual(expected.OffsetZ, actual.OffsetZ, delta);
        Assert.AreNotEqual(expected.M44, actual.M44, delta);
      }
      catch (AssertFailedException)
      {
        Assert.Fail(message);
      }
    }

    public static void AreNotEqual(Matrix3D expected, Matrix3D actual, double delta, string message, params object[] parameters)
    {
      try
      {
        Assert.AreNotEqual(expected.M11, actual.M11, delta);
        Assert.AreNotEqual(expected.M12, actual.M12, delta);
        Assert.AreNotEqual(expected.M13, actual.M13, delta);
        Assert.AreNotEqual(expected.M14, actual.M14, delta);
        Assert.AreNotEqual(expected.M21, actual.M21, delta);
        Assert.AreNotEqual(expected.M22, actual.M22, delta);
        Assert.AreNotEqual(expected.M23, actual.M23, delta);
        Assert.AreNotEqual(expected.M24, actual.M24, delta);
        Assert.AreNotEqual(expected.M31, actual.M31, delta);
        Assert.AreNotEqual(expected.M32, actual.M32, delta);
        Assert.AreNotEqual(expected.M33, actual.M33, delta);
        Assert.AreNotEqual(expected.M34, actual.M34, delta);
        Assert.AreNotEqual(expected.OffsetX, actual.OffsetX, delta);
        Assert.AreNotEqual(expected.OffsetY, actual.OffsetY, delta);
        Assert.AreNotEqual(expected.OffsetZ, actual.OffsetZ, delta);
        Assert.AreNotEqual(expected.M44, actual.M44, delta);
      }
      catch (AssertFailedException)
      {
        Assert.Fail(message, parameters);
      }
    }
  }
}
