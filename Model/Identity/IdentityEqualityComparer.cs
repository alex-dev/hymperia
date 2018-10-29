using System.Collections.Generic;

namespace Hymperia.Model.Identity
{
  internal  class IdentityEqualityComparer<T> : EqualityComparer<T> where T : IIdentity
  {
    public override bool Equals(T left, T right) => StaticEquals(left, right);
    public override int GetHashCode(T obj) => StaticGetHashCode(obj);
    public static bool StaticEquals(T left, T right) => left?.Id == right?.Id;
    public static int StaticGetHashCode(T obj) => GetHashCode(obj?.Id);
    /// <remarks><see cref="null"/> object or <see cref="T.Id"/> of 0 are both invalid objects so the hash collision doesn't matter in actual use as long as Id is quickly set.</remarks>
    public static int GetHashCode(int? id) => 2108858624 + id?.GetHashCode() ?? 0;
  }
}
