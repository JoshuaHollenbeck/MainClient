using System.Windows;
using MainClient._ViewModel;

namespace MainClient._View
{
    public partial class WithdrawalCheckSingle : Window
    {
        public WithdrawalCheckSingle()
        {
            InitializeComponent();

            var viewModel = new WithdrawalCheckSingleVM
            {
                CloseAndLoadAccountAction = (accountNumber) => this.Close()
            };

            DataContext = viewModel;
        }
    }
}
