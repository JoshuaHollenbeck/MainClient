using System.Windows;
using MainClient._ViewModel;

namespace MainClient._View
{

    public partial class AccountEditLimit : Window
    {
        public AccountEditLimit()
        {
            InitializeComponent();

            var viewModel = new AccountEditLimitVM
            {
                CloseAndLoadAccountAction = (accountNumber) => this.Close()
            };

            DataContext = viewModel;
        }
    }
}
