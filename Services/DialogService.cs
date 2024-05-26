using System.Windows;
using System;

namespace MainClient.Services
{
    // Implementation of the IDialogService interface.
    public class DialogService : IDialogService
    {
        // Method to open a dialog window of type T.
        public void OpenDialog<T>()
            where T : Window, new()
        {
            // Creating an instance of the specified window type and showing it as a modal dialog.
            T view = new T();
            view.ShowDialog();
        }

        // Method to show a message dialog with specified message and title.
        public void ShowMessage(string message, string title)
        {
            // Displaying a message box with the specified message and title.
            MessageBox.Show(message, title);
        }
    }
}
