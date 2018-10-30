using System;
using Hymperia.Model.Modeles.JsonObject;
using Prism.Commands;

namespace Hymperia.Facade.CommandAggregatorCommands
{
  public class AddFormeCommand : DelegateCommand<Point>
  {
    /// <inheritdoc />
    public AddFormeCommand(Action<Point> execute) : base(execute) { }

    /// <inheritdoc />
    public AddFormeCommand(Action<Point> execute, Func<Point, bool> canExecute) : base(execute, canExecute) { }
  }
}
