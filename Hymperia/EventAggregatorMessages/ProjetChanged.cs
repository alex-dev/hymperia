﻿using Hymperia.Model.Modeles;
using Prism.Events;

namespace Hymperia.Facade.EventAggregatorMessages
{
  public sealed class ProjetChanged : PubSubEvent<Projet> { }
}
