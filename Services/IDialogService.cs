using System.Windows;
using System;

namespace MainClient.Services
{
    // Interface for handling dialog operations.
    public interface IDialogService
    {
        // Method to open a dialog window of type T.
        void OpenDialog<T>()
            where T : Window, new();

        // Method to show a message dialog with specified message and title.
        void ShowMessage(string message, string title);
    }
}
