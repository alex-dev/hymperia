using System;
using System.Collections.Generic;

namespace Prism.Titles
{
  public interface ITitleAggregator
  {
    T GetTitle<T>() where T : ITitle, new();
  }
}
