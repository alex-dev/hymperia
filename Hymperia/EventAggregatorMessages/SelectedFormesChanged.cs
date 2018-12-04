using System.Collections.Specialized;
using Prism.Events;

namespace Hymperia.Facade.EventAggregatorMessages
{
  public sealed class SelectedFormesChanged : PubSubEvent<NotifyCollectionChangedEventArgs> { }
}
