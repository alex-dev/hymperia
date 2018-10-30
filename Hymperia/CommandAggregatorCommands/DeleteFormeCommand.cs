using System;
using Prism.Commands;

namespace Hymperia.Facade.CommandAggregatorCommands
{
  public class DeleteFormeCommand : DelegateCommand
  {
    /// <inheritdoc />
    public DeleteFormeCommand(Action execute) : base(execute) { }

    /// <inheritdoc />
    public DeleteFormeCommand(Action execute, Func<bool> canExecute) : base(execute, canExecute) { }
  }
}
