using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MainClient._Model;
using MainClient._ViewModel;

namespace MainClient._View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClientAcctsResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                var viewModel = comboBox.DataContext as MainWindowVM;
                if (viewModel != null && comboBox.SelectedItem is MainWindowModel selectedAccount)
                {   
                    viewModel.HandleSelectionChange(selectedAccount);
                }
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
