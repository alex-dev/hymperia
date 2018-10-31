using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Data;
using JetBrains.Annotations;

namespace Hymperia.Facade.BaseClasses
{
  /// <summary>Implement bulk changes for <see cref="ObservableCollection{T}"/>.</summary>
  /// <inheritdoc/>
  /// <remarks>Reflection is used to potential avoid issue described in <a href="https://blogs.msdn.microsoft.com/samng/2007/11/26/virtual-events-in-c/">this post</a>.</remarks>
  public class BulkObservableCollection<T> : ObservableCollection<T>
  {
    /// <summary>Backing field for <see cref="ObservableCollection{T}.CollectionChanged"/> acquired through reflection. Avoid using if not needed.</summary>
    protected NotifyCollectionChangedEventHandler CollectionChangedEventHandler => CollectionChangedInfos.Delegate;

    #region Constructors

    /// inheritdoc/>
    public BulkObservableCollection() { }
    /// inheritdoc/>
    public BulkObservableCollection([NotNull] List<T> list) : base(list) { }
    /// inheritdoc/>
    public BulkObservableCollection([NotNull] IEnumerable<T> collection) : base(collection) { }

    #endregion

    /// <summary>Add <paramref name="items"/> to collection.</summary>
    public void AddRange([NotNull] IEnumerable<T> items)
    {
      CheckReentrancy();
      int index = Items.Count;
      var _items = items.ToArray();

      foreach (var item in _items)
        Items.Insert(Items.Count, item);

      OnCollectionChanged_Add(_items, index);
    }

    /// <summary>Remove <paramref name="items"/> from collection.</summary>
    public void RemoveRange([NotNull] IEnumerable<T> items)
    {
      CheckReentrancy();
      var _items = items.Intersect(Items).ToArray();

      foreach (var item in _items)
        Items.Remove(item);

      OnCollectionChanged_Remove(_items);
    }

    /// <summary>Replace <paramref name="olditems"/> with <paramref name="newitems"/>.</summary>
    /// <remarks>This replace all values in <paramref name="olditems"/> with the corresponding in <paramref name="newitems"/>.
    /// Extra values are then removed or appended.</remarks>
    public void ReplaceRange([NotNull] IEnumerable<T> olditems, [NotNull] IEnumerable<T> newitems)
    {
      CheckReentrancy();
      var _olditems = olditems.Intersect(Items).ToArray();
      var _newitems = newitems.ToArray();
      var oldenumerator = _olditems.Cast<T>().GetEnumerator();
      var newenumerator = _newitems.Cast<T>().GetEnumerator();
      bool oldbool, newbool;

      // Evil boolean witchcraft! Gotta ensures both enumerators are always moved before leaving the loop and && shortcircuit...
      while ((oldbool = oldenumerator.MoveNext()) & (newbool = newenumerator.MoveNext()))
        Items[Items.IndexOf(oldenumerator.Current)] = newenumerator.Current;

      if (oldbool)
        do
        {
          Items.Remove(oldenumerator.Current);
        } while (oldenumerator.MoveNext());

      if (newbool)
        do
        {
          Items.Add(newenumerator.Current);
        } while (newenumerator.MoveNext());

      OnCollectionChanged_Replaced(_olditems, _newitems);
    }

    #region On Collection Changed Invokers

    #region Subhandlers

    private void OnCollectionChanged_Add(IList items, int index)
    {
      OnPropertyChanged(new PropertyChangedEventArgs("Count"));
      OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
      OnMultipleCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items, index));
    }

    private void OnCollectionChanged_Remove(IList items)
    {
      OnPropertyChanged(new PropertyChangedEventArgs("Count"));
      OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
      OnMultipleCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, items));
    }

    private void OnCollectionChanged_Replaced(IList olditems, IList newitems)
    {
      OnPropertyChanged(new PropertyChangedEventArgs("Count"));
      OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
      OnMultipleCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newitems, olditems));
    }

    #endregion

    /// <summary>Trigger <see cref="ObservableCollection{T}.CollectionChanged"/> with <paramref name="e"/>.</summary>
    /// <remarks>
    ///   Rather than overriding <see cref="OnMultipleCollectionChanged(NotifyCollectionChangedEventArgs)"/>, override
    ///   <see cref="Handle(NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs)"/> to add exception to
    ///   <see cref="NotifyCollectionChangedEventHandler"/> or <see cref="HandleCollectionView(NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs)"/>
    ///   to add fonctionnalities to <see cref="CollectionView"/>.
    /// </remarks>
    protected void OnMultipleCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      if (CollectionChangedEventHandler is NotifyCollectionChangedEventHandler handlers)
      {
        using (BlockReentrancy())
        {
          foreach (NotifyCollectionChangedEventHandler handler in handlers.TraverseRecursively())
            Handle(handler, e);
        }
      }
    }

    /// <summary>Handle each <see cref="NotifyCollectionChangedEventHandler"/>.</summary>
    protected virtual void Handle(NotifyCollectionChangedEventHandler handler, NotifyCollectionChangedEventArgs e)
    {
      switch (handler.Target)
      {
        case CollectionView view:
          HandleCollectionView(handler, e); break;
        default:
          handler(this, e); break;
      }
    }

    /// <summary>Handle <see cref="NotifyCollectionChangedEventHandler"/> for a <see cref="CollectionView"/>.</summary>
    protected virtual void HandleCollectionView(NotifyCollectionChangedEventHandler handler, NotifyCollectionChangedEventArgs e)
    {
      void HandleMultiple(NotifyCollectionChangedAction action, IList items)
      {
        var eventArgs = from T arg in items
                        select new NotifyCollectionChangedEventArgs(action, arg);

        foreach (var arg in eventArgs)
          handler(this, arg);
      }

      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          HandleMultiple(NotifyCollectionChangedAction.Add, e.NewItems); break;
        case NotifyCollectionChangedAction.Remove:
          HandleMultiple(NotifyCollectionChangedAction.Remove, e.OldItems); break;
        case NotifyCollectionChangedAction.Replace:
          HandleMultiple(NotifyCollectionChangedAction.Remove, e.OldItems);
          HandleMultiple(NotifyCollectionChangedAction.Add, e.NewItems);
          break;
        case NotifyCollectionChangedAction.Move:
        case NotifyCollectionChangedAction.Reset:
          handler(this, e); break;
      }
    }

    #endregion

    #region Reflection - Danger!! Do not attempt to change or you shall lose your mind

    private EventDelegate CollectionChangedInfos => infos ?? (infos = EventDelegate.LoadCollectionChanged(this));
    private EventDelegate infos;

    private class EventDelegate
    {
      public static readonly BindingFlags Flags =
        BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

      #region Constructors

      public static EventDelegate LoadCollectionChanged(ObservableCollection<T> owner) =>
        new EventDelegate(typeof(ObservableCollection<T>).GetField(nameof(CollectionChanged), Flags), owner);

      private EventDelegate(FieldInfo field, ObservableCollection<T> owner)
      {
        Field = field;
        Owner = owner;
      }

      #endregion

      public NotifyCollectionChangedEventHandler Delegate => (NotifyCollectionChangedEventHandler)Field.GetValue(Owner);

      #region Private Fields

      [NotNull]
      private readonly FieldInfo Field;
      [NotNull]
      private readonly ObservableCollection<T> Owner;

      #endregion
    }
  }

  #endregion
}
