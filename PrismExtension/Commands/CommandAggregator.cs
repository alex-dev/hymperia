﻿using System;
using System.Collections.Generic;
using System.Windows.Input;
using JetBrains.Annotations;

namespace Prism.Commands
{
  public class CommandAggregator : ICommandAggregator
  {
    /// <inheritdoc />
    public TCommand GetCommand<TCommand>() where TCommand : ICommand
    {
      lock (commands)
      {
        return (TCommand)commands[typeof(TCommand)];
      }
    }

    /// <inheritdoc />
    public void RegisterCommand<TCommand>([NotNull] TCommand command) where TCommand : ICommand =>
      commands[typeof(TCommand)] = command;

    private readonly IDictionary<Type, ICommand> commands = new Dictionary<Type, ICommand>();
  }
}