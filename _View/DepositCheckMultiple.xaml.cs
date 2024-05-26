using System.Windows;
using MainClient._ViewModel;

namespace MainClient._View
{
    public partial class DepositCheckMultiple : Window
    {
        public DepositCheckMultiple()
        {
            InitializeComponent();

            var viewModel = new DepositCheckMultipleVM
            {
                CloseAndLoadAccountAction = (accountNumber) => this.Close()
            };

            DataContext = viewModel;
        }
    }
}
