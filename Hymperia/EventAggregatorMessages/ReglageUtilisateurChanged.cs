/*
 * Auteur : Antoine Mailhot
 * Date de création : 23 novembre 2018
 */

using Hymperia.Model.Modeles;
using Prism.Events;

namespace Hymperia.Facade.EventAggregatorMessages
{
  public class ReglageUtilisateurChanged : PubSubEvent<Utilisateur> { }
}
