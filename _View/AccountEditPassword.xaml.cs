using System.Windows;
using MainClient._ViewModel;

namespace MainClient._View
{
    public partial class AccountEditPassword : Window
    {
        public AccountEditPassword()
        {
            InitializeComponent();

            var viewModel = new AccountEditPasswordVM
            {
                CloseAndLoadAccountAction = (accountNumber) => this.Close()
            };

            DataContext = viewModel;
        }
    }
}
