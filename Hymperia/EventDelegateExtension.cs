using System.Collections.Specialized;
using System.ComponentModel;

namespace Hymperia.Facade
{
  public static class EventDelegateExtension
  {
    #region INotifyPropertyChanged

    /// <summary>Add <paramref name="handler"/> to <see cref="INotifyPropertyChanged.PropertyChanged"/> with null check.</summary>
    public static void Add(this INotifyPropertyChanged @object, PropertyChangedEventHandler handler)
    {
      if (@object is null)
        return;

      @object.PropertyChanged += handler;
    }

    /// <summary>Remove <paramref name="handler"/> to <see cref="INotifyPropertyChanged.PropertyChanged"/> with null check.</summary>
    public static void Remove(this INotifyPropertyChanged @object, PropertyChangedEventHandler handler)
    {
      if (@object is null)
        return;

      @object.PropertyChanged -= handler;
    }

    /// <summary>Add <paramref name="handler"/> to <see cref="INotifyPropertyChanging.PropertyChanging"/> with null check.</summary>
    public static void Add(this INotifyPropertyChanging @object, PropertyChangingEventHandler handler)
    {
      if (@object is null)
        return;

      @object.PropertyChanging += handler;
    }

    /// <summary>Remove <paramref name="handler"/> to <see cref="INotifyPropertyChanging.PropertyChanging"/> with null check.</summary>
    public static void Remove(this INotifyPropertyChanging @object, PropertyChangingEventHandler handler)
    {
      if (@object is null)
        return;

      @object.PropertyChanging -= handler;
    }

    #endregion

    #region ICollectionChanged

    /// <summary>Add <paramref name="handler"/> to <see cref="INotifyCollectionChanged.CollectionChanged"/> with null check.</summary>
    public static void Add(this INotifyCollectionChanged @object, NotifyCollectionChangedEventHandler handler)
    {
      if (@object is null)
        return;

      @object.CollectionChanged += handler;
    }

    /// <summary>Remove <paramref name="handler"/> to <see cref="INotifyCollectionChanged.CollectionChanged"/> with null check.</summary>
    public static void Remove(this INotifyCollectionChanged @object, NotifyCollectionChangedEventHandler handler)
    {
      if (@object is null)
        return;

      @object.CollectionChanged -= handler;
    }

    #endregion
  }
}
