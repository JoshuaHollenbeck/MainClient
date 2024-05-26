using System.Windows;
using System.Threading;
using System.Globalization;
using MainClient._ViewModel;
using MainClient.Services;
using MainClient._View;

namespace MainClient
{
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            CultureInfo culture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            // Initialize the DialogService
            IDialogService dialogService = new DialogService();

            // Initialize the MainWindow with the DialogService
            LoginWindowVM loginWindowVM = new LoginWindowVM(dialogService);

            // Create MainWindow and set its DataContext
            LoginWindow loginWindow = new LoginWindow
            {
                DataContext = loginWindowVM
            };

            loginWindow.Show();
        }
    }
}