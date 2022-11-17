﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risks_v2
{
    public class Command : System.Windows.Input.ICommand
    {
        private Action<object?> _execute;
        private Func<object?, bool> _canExecute;

        public event EventHandler? CanExecuteChanged;

        public Command(Action<object?> execute, Func<object?, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }
}
