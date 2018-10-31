using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MoreLinq;

namespace Hymperia.Facade
{
  public static class LINQExtension
  {
    /// <summary>Finds the only element matching <paramref name="predicate"/> or return the first element of sequence.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> or <paramref name="predicate"/> are <see cref="null"/>.</exception>
    /// <exception cref="InvalidOperationException"><paramref name="enumerable"/> is empty or no matches were found.</exception>
    /// <remarks>The enumerator approach is faster, but impossible to do on a <see cref="IQueryable{T}"/>.</remarks>
    [NotNull]
    public static T SingleOrFirst<T>(this IEnumerable<T> enumerable, Predicate<T> predicate)
    {
      if (enumerable is null)
        throw new ArgumentNullException(nameof(enumerable));
      if (predicate is null)
        throw new ArgumentNullException(nameof(predicate));

      var enumerator = enumerable.GetEnumerator();

      if (!enumerator.MoveNext())
        throw new InvalidOperationException("No elements");
      var result = enumerator.Current;

      while (enumerator.MoveNext())
        if (predicate(enumerator.Current))
        {
          if (predicate(result))
            throw new InvalidOperationException("Multiple matches");

          result = enumerator.Current;
        }

      return result;
    }

    /// <summary>Apply <paramref name="action"/> to the collection when evaluated.</summary>
    /// <returns>An enumerable containaig the objects passed.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> or <paramref name="action"/> are <see cref="null"/>.</exception>
    /// <remarks>
    ///   Deferred action equivalent to <see cref="Enumerable.Select{T, T}(IEnumerable{T}, Func{T, T})"/> with added guarantee of
    ///   receiving the same object back like <see cref="MoreEnumerable.ForEach{T}(IEnumerable{T}, Action{T})"/> but deferred and chainable.
    /// </remarks>
    [NotNull]
    [LinqTunnel]
    public static IEnumerable<T> DeferredForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
      if (enumerable is null)
        throw new ArgumentNullException(nameof(enumerable));
      if (action is null)
        throw new ArgumentNullException(nameof(action));

      foreach (var item in enumerable)
      {
        action(item);
        yield return item;
      }
    }

    /// <summary>Apply <paramref name="action"/> to the collection when evaluated.</summary>
    /// <returns>An enumerable containaig the objects passed.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> or <paramref name="action"/> are <see cref="null"/>.</exception>
    /// <remarks>
    ///   Deferred action equivalent to <see cref="Enumerable.Select{T, T}(IEnumerable{T}, Func{T, T, int})"/> with added guarantee of
    ///   receiving the same object back like <see cref="MoreEnumerable.ForEach{T}(IEnumerable{T}, Action{T, int})"/> but deferred and chainable.
    /// </remarks>
    [NotNull]
    [LinqTunnel]
    public static IEnumerable<T> DeferredForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
    {
      if (enumerable is null)
        throw new ArgumentNullException(nameof(enumerable));
      if (action is null)
        throw new ArgumentNullException(nameof(action));

      int index = 0;
      foreach (var item in enumerable)
      {
        action(item, index++);
        yield return item;
      }
    }

    /// <summary>Specific reverse implementation for <see cref="LinkedList{T}"/>.</summary>
    /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see cref="null"/>.</exception>
    [NotNull]
    [LinqTunnel]
    public static IEnumerable<T> Reverse<T>(this LinkedList<T> collection)
    {
      if (collection is null)
        throw new ArgumentNullException(nameof(collection));

      var item = collection.Last;

      while (item != null)
      {
        yield return item.Value;
        item = item.Previous;
      }
    }
  }
}
