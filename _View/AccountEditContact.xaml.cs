using System.Windows;
using MainClient._ViewModel;

namespace MainClient._View
{
    public partial class AccountEditContact : Window
    {
        public AccountEditContact()
        {
            InitializeComponent();

            var viewModel = new AccountEditContactVM
            {
                CloseAndLoadAccountAction = (accountNumber) => this.Close()
            };

            DataContext = viewModel;
        }
    }
}
