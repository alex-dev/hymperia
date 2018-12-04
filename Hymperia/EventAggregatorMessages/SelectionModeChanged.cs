using Prism.Events;

namespace Hymperia.Facade.EventAggregatorMessages
{
  public sealed class SelectionModeChanged : PubSubEvent<SelectionMode?> { }
}
