using System.Windows;
using MainClient._ViewModel;

namespace MainClient._View
{
    public partial class DepositCash : Window
    {
        public DepositCash()
        {
            InitializeComponent();
            
            var viewModel = new DepositCashVM
            {
                CloseAndLoadAccountAction = (accountNumber) => this.Close()
            };

            DataContext = viewModel;
        }
    }
}
