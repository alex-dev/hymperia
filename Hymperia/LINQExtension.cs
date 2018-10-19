using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace Hymperia.Facade
{
  public static class LINQExtension
  {
    /// <summary>Apply <paramref name="action"/> to the collection when evaluated.</summary>
    /// <returns>An enumerable containaig the objects passed.</returns>
    /// <remarks>
    ///   Deferred action equivalent to <see cref="Enumerable.Select{T, T}(IEnumerable{T}, Func{T, T})"/> with added guarantee of
    ///   receiving the same object back like <see cref="MoreEnumerable.ForEach{T}(IEnumerable{T}, Action{T})"/> but deferred and chainable.
    /// </remarks>
    public static IEnumerable<T> DeferredForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
      foreach (var item in enumerable)
      {
        action(item);
        yield return item;
      }
    }

    /// <summary>Apply <paramref name="action"/> to the collection when evaluated.</summary>
    /// <returns>An enumerable containaig the objects passed.</returns>
    /// <remarks>
    ///   Deferred action equivalent to <see cref="Enumerable.Select{T, T}(IEnumerable{T}, Func{T, T, int})"/> with added guarantee of
    ///   receiving the same object back like <see cref="MoreEnumerable.ForEach{T}(IEnumerable{T}, Action{T, int})"/> but deferred and chainable.
    /// </remarks>
    public static IEnumerable<T> DeferredForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
    {
      int index = 0;
      foreach (var item in enumerable)
      {
        action(item, index++);
        yield return item;
      }
    }

    /// <summary>
    ///   Validate all items before <paramref name="bound"/> fulfill <paramref name="firstCondition"/> and all
    ///   items after or at <paramref name="bound"/> fulfill <paramref name="lastCondition"/>.
    /// </summary>
    public static bool AllFirstsAndAllLast<T>(this IEnumerable<T> enumerable, Func<T, bool> firstCondition, ulong bound, Func<T, bool> lastCondition)
    {
      var enumerator = enumerable.GetEnumerator();

      for (ulong i = 0; i < bound || enumerator.MoveNext(); ++i) // Relies on shortcut evaluation
        if (!firstCondition(enumerator.Current))                 // to avoid skipping one. 
          return false;

      while (enumerator.MoveNext())
        if (!lastCondition(enumerator.Current))
          return false;

      return true;
    }
  }
}
