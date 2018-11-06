using System.Collections.Specialized;
using System.ComponentModel;

namespace Hymperia.Facade.Extensions
{
  public static class EventDelegateExtension
  {
    #region INotifyPropertyChanged

    /// <summary>Add <paramref name="handler"/> to <see cref="INotifyPropertyChanged.PropertyChanged"/> with null check.</summary>
    public static void Add(this INotifyPropertyChanged sender, PropertyChangedEventHandler handler)
    {
      if (sender is null)
        return;

      sender.PropertyChanged += handler;
    }

    /// <summary>Remove <paramref name="handler"/> to <see cref="INotifyPropertyChanged.PropertyChanged"/> with null check.</summary>
    public static void Remove(this INotifyPropertyChanged sender, PropertyChangedEventHandler handler)
    {
      if (sender is null)
        return;

      sender.PropertyChanged -= handler;
    }

    /// <summary>Add <paramref name="handler"/> to <see cref="INotifyPropertyChanging.PropertyChanging"/> with null check.</summary>
    public static void Add(this INotifyPropertyChanging sender, PropertyChangingEventHandler handler)
    {
      if (sender is null)
        return;

      sender.PropertyChanging += handler;
    }

    /// <summary>Remove <paramref name="handler"/> to <see cref="INotifyPropertyChanging.PropertyChanging"/> with null check.</summary>
    public static void Remove(this INotifyPropertyChanging sender, PropertyChangingEventHandler handler)
    {
      if (sender is null)
        return;

      sender.PropertyChanging -= handler;
    }

    #endregion

    #region ICollectionChanged

    /// <summary>Add <paramref name="handler"/> to <see cref="INotifyCollectionChanged.CollectionChanged"/> with null check.</summary>
    public static void Add(this INotifyCollectionChanged sender, NotifyCollectionChangedEventHandler handler)
    {
      if (sender is null)
        return;

      sender.CollectionChanged += handler;
    }

    /// <summary>Remove <paramref name="handler"/> to <see cref="INotifyCollectionChanged.CollectionChanged"/> with null check.</summary>
    public static void Remove(this INotifyCollectionChanged sender, NotifyCollectionChangedEventHandler handler)
    {
      if (sender is null)
        return;

      sender.CollectionChanged -= handler;
    }

    #endregion
  }
}
