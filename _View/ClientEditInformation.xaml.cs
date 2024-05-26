using System.Windows;
using MainClient._ViewModel;

namespace MainClient._View
{
    public partial class ClientEditInformation : Window
    {
        public ClientEditInformation()
        {
            InitializeComponent();

            var viewModel = new ClientEditInformationVM
            {
                CloseAndLoadAccountAction = (accountNumber) => this.Close()
            };

            DataContext = viewModel;
        }
    }
}
