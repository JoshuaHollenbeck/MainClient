using System.Windows;
using MainClient._ViewModel;

namespace MainClient._View
{
    public partial class WithdrawalJournal : Window
    {
        public WithdrawalJournal()
        {
            InitializeComponent();

            var viewModel = new WithdrawalJournalVM()
            {
                CloseAndLoadAccountAction = (accountNumber) => this.Close()
            };

            DataContext = viewModel;
        }
    }
}
