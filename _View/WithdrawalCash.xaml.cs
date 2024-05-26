using System.Windows;
using MainClient._ViewModel;

namespace MainClient._View
{
    public partial class WithdrawalCash : Window
    {
        public WithdrawalCash()
        {
            InitializeComponent();

            var viewModel = new WithdrawalCashVM
            {
                CloseAndLoadAccountAction = (accountNumber) => this.Close()
            };

            DataContext = viewModel;
        }
    }
}
