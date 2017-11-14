using System;
using System.Windows.Input;
using Prism.Commands;

namespace Pomodoro.Commands {
    public class SimpleCommand : ICommand {
        private Action<object> _execute;
        private Func<object, bool> _canExecute;

        public SimpleCommand(Action<object> execute, Func<object, bool> canExecute = null) {
            if (execute == null) {
                throw new ArgumentNullException(nameof(execute));
            }
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged ;

        public void RaiseCanExecuteChanged() {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
            
        public bool CanExecute(object parameter) {
            return _canExecute == null || _canExecute(parameter);
        }

        void ICommand.Execute(object parameter) {
            _execute(parameter);
        }
    }
}
