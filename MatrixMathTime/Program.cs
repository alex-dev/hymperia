using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media.Media3D;
using DirectXOperations;

namespace MatrixMathTime
{
  public static class Program
  {
    private const int Count = 10000;

    private static IEnumerable<Quaternion> Quaternions
    {
      get
      {
        var random = new Random();

        for (int i = 0; i < Count; ++i)
        {
          var quaternion = new Quaternion(random.Next(100), random.Next(100), random.Next(100), random.Next(100));
          quaternion.Normalize();
          yield return quaternion;
        }
      }
    }

    private static IEnumerable<Vector3D> Vectors
    {
      get
      {
        var random = new Random();

        for (int i = 0; i < Count; ++i)
        {
          yield return new Vector3D(random.Next(100), random.Next(100), random.Next(100));
        }
      }
    }

    public static void Main(string[] args)
    {
      var managed_stopwatch = new Stopwatch();
      var directx_stopwatch = new Stopwatch();
      var quaternions = Quaternions.ToArray();
      var vectors = Vectors.ToArray();
      AppDomain.CurrentDomain.Load("DirectXOperations");

      {
        managed_stopwatch.Start();

        for (int i = 0; i < Count; ++i)
        {
          var matrix = Matrix3D.Identity;
          matrix.Rotate(quaternions[i]);
          matrix.Translate(vectors[i]);
        }

        managed_stopwatch.Stop();
      }
      {
        directx_stopwatch.Start();

        for (int i = 0; i < Count; ++i)
        {
          quaternions[i].ComposeTransform(ref vectors[i]);
        }

        directx_stopwatch.Stop();
      }

      Console.WriteLine($"Managed run: { managed_stopwatch.Elapsed }");
      Console.WriteLine($"DirectX run: { directx_stopwatch.Elapsed }");
      Console.ReadKey(true);
    }
  }
}
