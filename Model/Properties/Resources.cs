using JetBrains.Annotations;

namespace Hymperia.Model.Properties
{
  public static partial class Resources
  {
    public static string ConeToString([CanBeNull] object id, [CanBeNull] object origin, [CanBeNull] object rotation) =>
      FormeToString(Cone, id, origin, rotation);
    public static string CylinderToString([CanBeNull] object id, [CanBeNull] object origin, [CanBeNull] object rotation) =>
      FormeToString(Cylinder, id, origin, rotation);
    public static string EllipsoidToString([CanBeNull] object id, [CanBeNull] object origin, [CanBeNull] object rotation) =>
      FormeToString(Ellipsoid, id, origin, rotation);
    public static string PrismToString([CanBeNull] object id, [CanBeNull] object origin, [CanBeNull] object rotation) =>
      FormeToString(Prism, id, origin, rotation);
  }
}
