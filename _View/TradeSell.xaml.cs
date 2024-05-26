using MainClient._ViewModel;
using System.Windows;

namespace MainClient._View
{
    public partial class TradeSell : Window
    {
        public TradeSell()
        {
            InitializeComponent();

            var viewModel = new TradeSellVM
            {
                CloseAndLoadAccountAction = (accountNumber) => this.Close()
            };

            DataContext = viewModel;
        }
    }
}
