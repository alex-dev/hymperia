using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
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
    [NotNull]
    [LinqTunnel]
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
    [NotNull]
    [LinqTunnel]
    public static IEnumerable<T> DeferredForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
    {
      int index = 0;
      foreach (var item in enumerable)
      {
        action(item, index++);
        yield return item;
      }
    }

    /// <summary>Specific reverse implementation for <see cref="LinkedList{T}"/>.</summary>
    [NotNull]
    [LinqTunnel]
    public static IEnumerable<T> Reverse<T>(this LinkedList<T> collection)
    {
      var item = collection.Last;

      while (item != null)
      {
        yield return item.Value;
        item = item.Previous;
      }
    }
  }
}
