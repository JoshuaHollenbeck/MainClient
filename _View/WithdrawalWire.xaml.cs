using System.Windows;
using MainClient._ViewModel;

namespace MainClient._View
{
    public partial class WithdrawalWire : Window
    {
        public WithdrawalWire()
        {
            InitializeComponent();

            var viewModel = new WithdrawalWireVM
            {
                CloseAndLoadAccountAction = (accountNumber) => this.Close()
            };

            DataContext = viewModel;
        }
    }
}
