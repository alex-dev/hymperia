using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Hymperia.Facade.BaseClasses
{
  public class BulkObservableCollection<T> : ObservableCollection<T>
  {
    public BulkObservableCollection() : base() { }
    public BulkObservableCollection(List<T> list) : base(list) { }
    public BulkObservableCollection(IEnumerable<T> collection) : base(collection) { }

    public void AddRange(IEnumerable<T> items)
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

    public void RemoveRange(IEnumerable<T> items)
    {
      CheckReentrancy();
      var _items = items.Intersect(Items).ToArray();

      foreach (var item in _items)
      {
        Items.Remove(item);
      }

      OnCollectionChanged_Remove(_items);
    }

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

  }
}
