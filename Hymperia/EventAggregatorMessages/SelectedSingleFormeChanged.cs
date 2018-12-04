using Hymperia.Facade.ModelWrappers;
using Prism.Events;

namespace Hymperia.Facade.EventAggregatorMessages
{
  public sealed class SelectedSingleFormeChanged : PubSubEvent<FormeWrapper> { }
}
