using MainClient._ViewModel;
using System.Windows;

namespace MainClient._View
{
    public partial class TradeBuy : Window
    {
        public TradeBuy()
        {
            InitializeComponent();

            var viewModel = new TradeBuyVM
            {
                CloseAndLoadAccountAction = (accountNumber) => this.Close()
            };

            DataContext = viewModel;
        }
    }
}
