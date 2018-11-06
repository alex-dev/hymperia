using Prism.Events;

namespace Hymperia.Facade.EventAggregatorMessages
{
  public class SelectionModeChanged : PubSubEvent<SelectionMode?> { }
}
