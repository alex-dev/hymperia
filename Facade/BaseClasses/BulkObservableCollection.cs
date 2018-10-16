using System;
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
  public class BulkObservableCollection<T> : ObservableCollection<T>
  {
    /// <summary>Backing field for <see cref="ObservableCollection{T}.CollectionChanged"/> acquired through reflection. Avoid using if not needed.</summary>
    protected NotifyCollectionChangedEventHandler CollectionChangedEventHandler => CollectionChangedInfos.Delegate as NotifyCollectionChangedEventHandler;

    #region Constructors

    public BulkObservableCollection() : base() { }
    public BulkObservableCollection([NotNull] List<T> list) : base(list) { }
    public BulkObservableCollection([NotNull] IEnumerable<T> collection) : base(collection) { }

    #endregion

    public void AddRange([NotNull] IEnumerable<T> items)
    {
      CheckReentrancy();
      var index = Items.Count;
      var _items = items.ToArray();

      foreach (var item in _items)
      {
        Items.Insert(Items.Count, item);
      }

      OnCollectionChanged_Add(_items, index);
    }

    public void RemoveRange([NotNull] IEnumerable<T> items)
    {
      CheckReentrancy();
      var _items = items.Intersect(Items).ToArray();

      foreach (var item in _items)
      {
        Items.Remove(item);
      }

      OnCollectionChanged_Remove(_items);
    }

    #region On Collection Changed Invokers

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

    #region Multiple CollectionChanged Invoker

    protected virtual void OnMultipleCollectionChanged(NotifyCollectionChangedEventArgs args)
    {
      if (CollectionChangedEventHandler is NotifyCollectionChangedEventHandler handlers)
      {
        using (BlockReentrancy())
        {
          foreach (NotifyCollectionChangedEventHandler handler in handlers.GetInvocationList())
          {
            switch (handler.Target)
            {
              case CollectionView view:
                HandleCollectionView(handler, args); break;
              default:
                handler(this, args); break;
            }
          }
        }
      }
    }

    protected void HandleCollectionView(NotifyCollectionChangedEventHandler handler, NotifyCollectionChangedEventArgs args)
    {
      void HandleMultiple(NotifyCollectionChangedAction action, IList items)
      {
        var eventArgs = from T arg in items
                        select new NotifyCollectionChangedEventArgs(action, arg);

        foreach (var arg in eventArgs)
        {
          handler(this, arg);
        }
      }

      switch (args.Action)
      {
        case NotifyCollectionChangedAction.Add:
          HandleMultiple(NotifyCollectionChangedAction.Add, args.NewItems); break;
        case NotifyCollectionChangedAction.Remove:
          HandleMultiple(NotifyCollectionChangedAction.Remove, args.OldItems); break;
        case NotifyCollectionChangedAction.Replace:
        case NotifyCollectionChangedAction.Move:
        case NotifyCollectionChangedAction.Reset:
          handler(this, args); break;
      }
    }

    #endregion

    #endregion

    #region Reflection - Danger!! Do not attempt to change or you shall lose your mind

    private EventDelegate CollectionChangedInfos => infos ?? (infos = EventDelegate.LoadCollectionChanged(this));
    private EventDelegate infos;

    private class EventDelegate
    {
      public static readonly BindingFlags Flags;

      #region Constructors

      public static EventDelegate LoadCollectionChanged(ObservableCollection<T> owner) =>
        new EventDelegate(typeof(ObservableCollection<T>).GetField(nameof(CollectionChanged), Flags), owner);

      static EventDelegate()
      {
        Flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
      }

      public EventDelegate(FieldInfo field, ObservableCollection<T> owner)
      {
        Field = field;
        Owner = owner;
      }

      #endregion

      public Delegate Delegate => (Delegate)Field.GetValue(Owner);

      #region Private Fields

      private readonly FieldInfo Field;
      private readonly ObservableCollection<T> Owner;

      #endregion
    }
  }

  #endregion
}
