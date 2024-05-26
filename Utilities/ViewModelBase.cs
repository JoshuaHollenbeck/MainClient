using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using MainClient.Services;

namespace MainClient.Utilities
{
    // This class serves as a foundation for ViewModels, providing functionality to notify changes in properties.
    public class ViewModelBase : INotifyPropertyChanged
    {
        // Event triggered when a property value changes.
        public event PropertyChangedEventHandler PropertyChanged;

        // Private field to store the current view.
        private object _currentView;

        // Public property representing the current view.
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                // Set the value of the current view and notify any subscribers about the property change.
                _currentView = value;
                OnPropertyChanged();
            }
        }

        // Method to raise the PropertyChanged event.
        public void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            // Invoke the event, passing in this instance and the property name that has changed.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        // Method to load a ViewModel based on the provided account number and a factory method.
        protected void LoadViewModel<T>(string accountNumber, Func<string, T> viewModelFactory)
            where T : class
        {
            // Check if the account number is empty.
            if (string.IsNullOrEmpty(accountNumber))
            {
                // If the account number is empty, show a warning message.
                MessageBox.Show(
                    "No account selected",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                // If the account number is not empty, load the corresponding ViewModel using the factory method.
                CurrentView = viewModelFactory(accountNumber);
            }
        }

        // For ViewModels that require IDialogService
        protected void LoadViewModel<T>(string accountNumber, Func<string, IDialogService, T> viewModelFactory)
            where T : class
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show("No account selected", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                IDialogService dialogService = new DialogService(); // Create or get IDialogService instance.
                CurrentView = viewModelFactory(accountNumber, dialogService);
            }
        }

        protected virtual void LoadViewModel<T>(string accountNumber, Func<string, IDialogService, ICommand[], T> viewModelFactory) where T : class
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show("No account selected", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                IDialogService dialogService = new DialogService(); // Assume you have a way to get a dialog service instance
                // This part should not instantiate commands directly. It's just a placeholder.
                ICommand[] commands = new ICommand[0]; // No commands are instantiated here, just a placeholder.
                CurrentView = viewModelFactory(accountNumber, dialogService, commands);
            }
        }
    }
}
