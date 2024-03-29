﻿using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Hymperia.Facade.Extensions
{
  public static class EventHandlerExtension
  {
    /// <summary>Traverse le <paramref name="handler"/> et trouve tous ses handlers.</summary>
    [NotNull]
    [ItemNotNull]
    public static IEnumerable<Delegate> Flatten([NotNull] this Delegate handler)
    {
      IEnumerable<Delegate> handlers = handler.GetInvocationList();

      if (handlers.First() != handler)
        handlers = handlers.SelectMany(h => h.Flatten());

      return handlers;
    }
  }
}
