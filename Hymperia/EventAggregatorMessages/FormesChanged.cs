using System.Collections.Specialized;
using Prism.Events;

namespace Hymperia.Facade.EventAggregatorMessages
{
  public sealed class FormesChanged : PubSubEvent<NotifyCollectionChangedEventArgs> { }
}
