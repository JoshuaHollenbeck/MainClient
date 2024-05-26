using System.Windows;
using MainClient._ViewModel;

namespace MainClient._View
{
    public partial class ClientEditTax : Window
    {
        public ClientEditTax()
        {
            InitializeComponent();

            var viewModel = new ClientEditTaxVM
            {
                CloseAndLoadAccountAction = (accountNumber) => this.Close()
            };

            DataContext = viewModel;
        }
    }
}
