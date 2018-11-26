/*
 * Auteur : Antoine Mailhot
 * Date de création : 26 novembre 2018
 */

using System.Collections.Generic;
using Prism.Events;

namespace Hymperia.Facade.EventAggregatorMessages
{
  class ReglageErreursChanged : PubSubEvent<List<string>> { }
}
