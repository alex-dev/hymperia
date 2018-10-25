using JetBrains.Annotations;

namespace Hymperia.Model.Properties
{
  public static partial class Resources
  {
    public static string ConeToString([CanBeNull] object id, [CanBeNull] object origin, [CanBeNull] object rotation) =>
      FormeToString(Resources.Cone, id, origin, rotation);
    public static string CylinderToString([CanBeNull] object id, [CanBeNull] object origin, [CanBeNull] object rotation) =>
      FormeToString(Resources.Cylinder, id, origin, rotation);
    public static string EllipsoidToString([CanBeNull] object id, [CanBeNull] object origin, [CanBeNull] object rotation) =>
      FormeToString(Resources.Ellipsoid, id, origin, rotation);
    public static string PrismToString([CanBeNull] object id, [CanBeNull] object origin, [CanBeNull] object rotation) =>
      FormeToString(Resources.Prism, id, origin, rotation);
  }
}
