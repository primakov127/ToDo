﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ToDo.ViewModel.Commands
{
    public class HistoryCommand : ICommand
    {
        protected readonly Predicate<object> _canExecute;
        protected readonly Action<object> _execute;

        public event EventHandler CanExecuteChanged;

        public HistoryCommand(Action<object> execute) : this(execute, _ => true) { }

        public HistoryCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        public bool CanExecute(object parameter) => _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);

        public void RaiseCanExecuteChanged()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
