using Hymperia.Model.Modeles;
using Prism.Events;

namespace Hymperia.Facade.EventAggregatorMessages
{
  public sealed class AccesChanged : PubSubEvent<Acces.Droit> { }
}
