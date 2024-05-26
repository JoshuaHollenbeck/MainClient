using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MainClient._View
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            
            Loaded += LoginWindowLoaded;
        }
        
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { 
                ((dynamic)this.DataContext).Password = ((PasswordBox)sender).SecurePassword;
            }
        }

        private void LoginWindowLoaded(object sender, RoutedEventArgs e)
        {
            var usernameTextBox = FindName("UserNameTextBox") as TextBox;
            usernameTextBox?.Focus();
        }
    }
}