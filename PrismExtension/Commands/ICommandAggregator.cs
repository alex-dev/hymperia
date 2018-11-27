using System.Collections.Generic;
using System.Windows.Input;
using JetBrains.Annotations;

namespace Prism.Commands
{
  public interface ICommandAggregator
  {
    /// <summary>Acquire a registered <see cref="ICommand"/>.</summary>
    TCommand GetCommand<TCommand>() where TCommand : CompositeCommand, new();
  }
}
