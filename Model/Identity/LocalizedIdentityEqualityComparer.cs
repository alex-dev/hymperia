using System.Collections.Generic;

namespace Hymperia.Model.Identity
{
  internal class LocalizedIdentityEqualityComparer<T> : EqualityComparer<ILocalizedIdentity<T>> where T : IIdentity
  {
    public override bool Equals(ILocalizedIdentity<T> left, ILocalizedIdentity<T> right) => StaticEquals(left, right);
    public override int GetHashCode(ILocalizedIdentity<T> obj) => StaticGetHashCode(obj);
    public static bool StaticEquals(ILocalizedIdentity<T> left, ILocalizedIdentity<T> right) =>
      left?.StringKey == right?.StringKey && left?.CultureKey == right?.CultureKey;
    public static int StaticGetHashCode(ILocalizedIdentity<T> obj) => GetHashCode(obj?.StringKey, obj?.CultureKey);
    public static int GetHashCode(string stringKey, string cultureKey)
    {
      var hashCode = -1651700841;
      hashCode = hashCode * -1521134295 + stringKey?.GetHashCode() ?? 0;
      hashCode = hashCode * -1521134295 + cultureKey?.GetHashCode() ?? 0;

      return hashCode;
    }
  }
}
