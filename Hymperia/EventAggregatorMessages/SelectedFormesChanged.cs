using System.Collections.Specialized;
using Prism.Events;

namespace Hymperia.Facade.EventAggregatorMessages
{
  public class SelectedFormesChanged : PubSubEvent<NotifyCollectionChangedEventArgs> { }
}
