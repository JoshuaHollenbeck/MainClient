using System.Windows;
using MainClient._ViewModel;

namespace MainClient._View
{
    public partial class DepositCheckSingle : Window
    {
        public DepositCheckSingle()
        {
            InitializeComponent();

            var viewModel = new DepositCheckSingleVM
            {
                CloseAndLoadAccountAction = (accountNumber) => this.Close()
            };

            DataContext = viewModel;
        }
    }
}
