namespace Project.WPF.Impl
{
    using System;
    using System.Windows.Input;
    public class CommandHandler : ICommand
    {
        #region Fields
        private Action _action;
        #endregion

        #region Contructors
        public CommandHandler(Action action)
        {
            _action = action;
        }
        #endregion

        #region Events
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        #endregion

        #region Methods
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _action();
        }
        #endregion
    }
}
