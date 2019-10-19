using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lain.Xaml.MVVM
{
    public class AsyncCommand : ICommand
    {
        private readonly Func<bool> canExecute;
        private readonly Func<Task> execute;

        public AsyncCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        [DebuggerStepThrough]
        public bool CanExecute(object p)
        {
            return canExecute?.Invoke() ?? true;
        }
        public async void Execute(object p)
        {
            if (!CanExecute(p))
                return;
            await execute();
        }
        public async Task ExecuteAsync(object p)
        {
            if (!CanExecute(p))
                return;
            await execute();
        }
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    public class AsyncCommand<T> : ICommand
    {
        private readonly Func<T, bool> _canExecute;
        private readonly Func<T, Task> _execute;


        public AsyncCommand(Func<T, Task> execute, Func<T, bool> canexecute = null)
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
        public async void Execute(object p)
        {
            if (!CanExecute(p))
                return;
            await _execute((T)p);
        }
        public async Task ExecuteAsync(object p)
        {
            if (!CanExecute(p))
                return;
            await _execute((T)p);
        }
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}