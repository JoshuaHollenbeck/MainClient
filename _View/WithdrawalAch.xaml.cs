using System.Windows;
using MainClient._ViewModel;

namespace MainClient._View
{
    public partial class WithdrawalAch : Window
    {
        public WithdrawalAch()
        {
            InitializeComponent();
            
            var viewModel = new WithdrawalAchVM
            {
                CloseAndLoadAccountAction = (accountNumber) => this.Close()
            };

            DataContext = viewModel;
        }
    }
}
