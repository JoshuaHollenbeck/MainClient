using System.Windows;
using MainClient._ViewModel;

namespace MainClient._View
{
    public partial class AccountAddBeneficiary : Window
    {
        public AccountAddBeneficiary()
        {
            InitializeComponent();

            var viewModel = new AccountAddBeneficiaryVM
            {
                CloseAndLoadAccountAction = (accountNumber) => this.Close()
            };

            DataContext = viewModel;
        }
    }
}
