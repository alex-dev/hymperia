using System;
using Prism.Events;

namespace Hymperia.Facade.EventAggregatorMessages
{
  public sealed class SelectedFormeChanged : PubSubEvent<Type> { }
}
