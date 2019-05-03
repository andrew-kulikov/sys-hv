using System;
using System.Windows.Input;

namespace WinAdminClientCore.RelayCommand
{
    public class RelayCommand : ICommand
    {
        #region fields

        private Predicate<object> _canExecute;
        private Action<object> _execute;

        #endregion

        #region ctors
        public RelayCommand(Predicate<object> canExecute, Action<object> execute)
        {
            _canExecute = canExecute;
            _execute = execute;
        }
        #endregion

        #region events

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        #endregion

        #region Interfaces

        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        #endregion
    }
}