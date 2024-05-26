using System.Windows;
using MainClient._ViewModel;

namespace MainClient._View
{
    public partial class DepositCheckSplit : Window
    {
        public DepositCheckSplit()
        {
            InitializeComponent();

            var viewModel = new DepositCheckSplitVM
            {
                CloseAndLoadAccountAction = (accountNumber) => this.Close()
            };

            DataContext = viewModel;
        }
    }
}
