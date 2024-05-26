using System.Windows;
using MainClient._ViewModel;

namespace MainClient._View
{
    public partial class AddAccountNewClient : Window
    {
       
        public AddAccountNewClient()
        {
            InitializeComponent();

            var viewModel = new AddAccountNewClientVM
            {
                CloseAndLoadAccountAction = (accountNumber) => this.Close()
            };

            DataContext = viewModel;
        }

    }
}