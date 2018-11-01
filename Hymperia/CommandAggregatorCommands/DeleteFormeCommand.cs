using System;
using System.Collections.Generic;
using Hymperia.Facade.ModelWrappers;
using Prism.Commands;

namespace Hymperia.Facade.CommandAggregatorCommands
{
  public class DeleteFormesCommand : DelegateCommand<ICollection<FormeWrapper>>
  {
    /// <inheritdoc />
    public DeleteFormesCommand(Action<ICollection<FormeWrapper>> execute) : base(execute) { }

    /// <inheritdoc />
    public DeleteFormesCommand(Action<ICollection<FormeWrapper>> execute, Func<ICollection<FormeWrapper>, bool> canExecute) : base(execute, canExecute) { }
  }
}
