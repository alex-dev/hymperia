using System.Collections.Generic;
using System.Windows.Input;
using JetBrains.Annotations;

namespace Prism.Commands
{
  public interface ICommandAggregator
  {
    /// <summary>Acquire a registered <see cref="ICommand"/>.</summary>
    /// <exception cref="KeyNotFoundException">No <see cref="ICommand"/> of type <typeparamref name="TCommand"/>
    /// were found.</exception>
    TCommand GetCommand<TCommand>() where TCommand : ICommand;

    /// <summary>Acquire a registered <see cref="CompositeCommand"/> or create it.</summary>
    TCommand GetCommandOrCreate<TCommand>() where TCommand : CompositeCommand, new();

    /// <summary>Register an unregistered <see cref="ICommand"/></summary>
    void RegisterCommand<TCommand>([NotNull] TCommand command) where TCommand : ICommand;
  }
}
