using System;
using System.Collections.Generic;

namespace Prism.Titles
{
  public class TitleAggregator : ITitleAggregator
  {
    public TTitle GetTitle<TTitle>() where TTitle : ITitle, new()
    {
      lock (titles)
      {
        if (!titles.ContainsKey(typeof(TTitle)))
          titles[typeof(TTitle)] = new TTitle();

        return (TTitle)titles[typeof(TTitle)];
      }
    }

    private readonly IDictionary<Type, ITitle> titles = new Dictionary<Type, ITitle>();
  }
}
