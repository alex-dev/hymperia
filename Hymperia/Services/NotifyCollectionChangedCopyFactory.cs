using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Hymperia.Facade.Services
{
  public class NotifyCollectionChangedCopyFactory
  {
    public NotifyCollectionChangedEventArgs Copy<TOriginal, TResult>(NotifyCollectionChangedEventArgs original, Func<TOriginal, TResult> converter)
    {
      switch (original.Action)
      {
        case NotifyCollectionChangedAction.Add:
          return CopyAdd(original, converter);
        case NotifyCollectionChangedAction.Remove:
          return CopyRemove(original, converter);
        case NotifyCollectionChangedAction.Replace:
          return CopyReplace(original, converter);
        case NotifyCollectionChangedAction.Move:
          return CopyMove(original, converter);
        case NotifyCollectionChangedAction.Reset:
          return CopyReset(original);
        default:
          throw new InvalidOperationException();
      }
    }

    private NotifyCollectionChangedEventArgs CopyAdd<TOriginal, TResult>(NotifyCollectionChangedEventArgs original, Func<TOriginal, TResult> converter)
    {
      var newitems = Convert(original.NewItems.Cast<TOriginal>(), converter);

      return new NotifyCollectionChangedEventArgs(original.Action, newitems, original.NewStartingIndex);
    }

    private NotifyCollectionChangedEventArgs CopyRemove<TOriginal, TResult>(NotifyCollectionChangedEventArgs original, Func<TOriginal, TResult> converter)
    {
      var olditems = Convert(original.OldItems.Cast<TOriginal>(), converter);

      return new NotifyCollectionChangedEventArgs(original.Action, olditems, original.OldStartingIndex);
    }

    private NotifyCollectionChangedEventArgs CopyReplace<TOriginal, TResult>(NotifyCollectionChangedEventArgs original, Func<TOriginal, TResult> converter)
    {
      var newitems = Convert(original.NewItems.Cast<TOriginal>(), converter);
      var olditems = Convert(original.OldItems.Cast<TOriginal>(), converter);

      return new NotifyCollectionChangedEventArgs(original.Action, newitems, olditems, original.OldStartingIndex);
    }

    private NotifyCollectionChangedEventArgs CopyMove<TOriginal, TResult>(NotifyCollectionChangedEventArgs original, Func<TOriginal, TResult> converter)
    {
      var newitems = Convert(original.NewItems.Cast<TOriginal>(), converter);

      return new NotifyCollectionChangedEventArgs(original.Action, newitems, original.NewStartingIndex, original.OldStartingIndex);
    }

    private NotifyCollectionChangedEventArgs CopyReset(NotifyCollectionChangedEventArgs original) =>
      new NotifyCollectionChangedEventArgs(original.Action);

    private TResult[] Convert<TOriginal, TResult>(IEnumerable<TOriginal> enumerable, Func<TOriginal, TResult> converter) =>
      (from item in enumerable
       let result = converter(item)
       where result is TResult
       select result).ToArray();
  }
}
