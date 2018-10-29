using System;
using JetBrains.Annotations;

namespace Prism.Events
{
  public static class PubSubEventExtension
  {
    public static SubscriptionToken Subscribe<TPayload>([NotNull] this PubSubEvent<TPayload> handler, Action<TPayload> action, Predicate<TPayload> filter) =>
      handler.Subscribe(action, default, default, filter);
  }
}
