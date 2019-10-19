using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Lain.Xaml.MVVM
{
    public class Command : ICommand
    {
        private readonly Func<bool> _canExecute;
        private readonly Action _execute;

        public Command(Action execute, Func<bool> canexecute = null)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
            _execute = execute;
            _canExecute = canexecute;
        }

        public event EventHandler CanExecuteChanged;

        [DebuggerStepThrough]
        public bool CanExecute(object p)
        {
            return _canExecute?.Invoke() ?? true;
        }
        public void Execute(object p)
        {
            if (!CanExecute(p))
                return;
            try
            {
                _execute();
            }
            catch
            {
                Debugger.Break();
            }
        }
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    public class Command<T> : ICommand
    {
        private readonly Func<T, bool> _canExecute;
        private readonly Action<T> _execute;

        public Command(Action<T> execute, Func<T, bool> canexecute = null)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
            _execute = execute;
            _canExecute = canexecute;
        }

        public event EventHandler CanExecuteChanged;

        [DebuggerStepThrough]
        public bool CanExecute(object p)
        {
            return _canExecute?.Invoke((T)p) ?? true;
        }
        public void Execute(object p)
        {
            if (!CanExecute(p))
                return;
            _execute((T)p);
        }
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}