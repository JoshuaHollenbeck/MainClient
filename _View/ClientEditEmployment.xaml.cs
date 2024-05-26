using System.Windows;
using MainClient._ViewModel;

namespace MainClient._View
{
    public partial class ClientEditEmployment : Window
    {
        public ClientEditEmployment()
        {
            InitializeComponent();

            var viewModel = new ClientEditEmploymentVM
            {
                CloseAndLoadAccountAction = (accountNumber) => this.Close()
            };

            DataContext = viewModel;
        }
    }
}
