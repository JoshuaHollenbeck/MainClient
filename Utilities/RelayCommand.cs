using System;
using System.Windows.Input;

namespace MainClient.Utilities
{
    // This class represents a RelayCommand, which is used to bind commands to UI elements.
    class RelayCommand : ICommand
    {
        // Action to execute when the command is invoked.
        private readonly Action<object> _execute;

        // Function to determine if the command can be executed.
        private readonly Func<object, bool> _canExecute;

        // Event triggered when the ability to execute the command changes.
        public event EventHandler CanExecuteChanged
        {
            // Add a handler to the CommandManager's RequerySuggested event.
            add { CommandManager.RequerySuggested += value; }
            // Remove the handler from the CommandManager's RequerySuggested event.
            remove { CommandManager.RequerySuggested -= value; }
        }

        // Constructor for RelayCommand taking a single action to execute.
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // Constructor for RelayCommand taking separate actions for single and double clicks.
        public RelayCommand(
            Action<object> executeSingleClick,
            Action<object> executeDoubleClick,
            Func<object, bool> canExecute = null
        )
        {
            _execute = parameter =>
            {
                // Check if the parameter is a MouseButtonEventArgs and if it's a double click.
                var mouseEventArgs = parameter as MouseButtonEventArgs;
                if (mouseEventArgs != null && mouseEventArgs.ClickCount == 2)
                {
                    // If it's a double click, invoke the executeDoubleClick action.
                    executeDoubleClick?.Invoke(parameter);
                }
                else
                {
                    // If it's not a double click, invoke the executeSingleClick action.
                    executeSingleClick?.Invoke(parameter);
                }
            };
            _canExecute = canExecute;
        }

        // Method to determine if the command can be executed.
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        // Method to execute the command.
        public void Execute(object parameter) => _execute(parameter);
    }
}
