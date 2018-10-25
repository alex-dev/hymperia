using System.Linq;
using JetBrains.Annotations;

namespace Hymperia.Facade.Properties
{
  internal static partial class Resources
  {
    public static string CanOnlyConvertFrom([NotNull][ItemCanBeNull] params object[] origins) =>
      CanOnlyConvertFrom(Join(origins));
    public static string CanOnlyConvertTo([NotNull][ItemCanBeNull] params object[] targets) =>
      CanOnlyConvertTo(Join(targets));
    public static string ImpossibleCast([CanBeNull] object value, [NotNull][ItemCanBeNull] params object[] targets) =>
      ImpossibleCast(value, Join(targets));
    public static string ManipulatorSupport([CanBeNull] object value, [NotNull][ItemCanBeNull] params object[] visuals) =>
      ManipulatorSupport(Join(visuals));
    public static string ViewportManipulatorSupport([NotNull][ItemCanBeNull] params object[] types) =>
      ViewportManipulatorSupport(Join(types));

    private static string Join([NotNull][ItemCanBeNull] params object[] objects)
    {
      if (objects.Length > 1)
      {
        int last = objects.Length - 1;
        return $"{ string.Join(EnumerationSeparator, objects.Take(last)) } { And } { objects[last] }";
      }
      else
      {
        return objects[0].ToString();
      }
    }
  }
}
