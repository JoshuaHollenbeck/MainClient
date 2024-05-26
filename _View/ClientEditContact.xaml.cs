using System.Windows;
using MainClient._ViewModel;

namespace MainClient._View
{
    public partial class ClientEditContact : Window
    {
        public ClientEditContact()
        {
            InitializeComponent();

            var viewModel = new ClientEditContactVM
            {
                CloseAndLoadAccountAction = (accountNumber) => this.Close()
            };

            DataContext = viewModel;
        }
    }
}
