using System.Windows;
using MainClient._ViewModel;

namespace MainClient._View
{
    public partial class AccountAddPowerOfAttorney : Window
    {
        public AccountAddPowerOfAttorney()
        {
            InitializeComponent();

            var viewModel = new AccountAddPowerOfAttorneyVM
            {
                CloseAndLoadAccountAction = (accountNumber) => this.Close()
            };

            DataContext = viewModel;
        }
    }
}
