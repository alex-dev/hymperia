/*
 * Auteur : Antoine Mailhot
 * Date de création : 28 novembre 2018
 */

using Hymperia.Model.Modeles;
using Prism.Events;

namespace Hymperia.Facade.EventAggregatorMessages
{
  public sealed class ReglageProjetChanged : PubSubEvent<Projet> { }
}
