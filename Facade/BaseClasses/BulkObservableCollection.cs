using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using JetBrains.Annotations;

namespace Hymperia.Facade.BaseClasses
{
  public class BulkObservableCollection<T> : ObservableCollection<T>
  {
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
      OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items, index));
    }

    private void OnCollectionChanged_Remove(IList items)
    {
      OnPropertyChanged(new PropertyChangedEventArgs("Count"));
      OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
      OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, items));
    }

    #endregion
  }
}
