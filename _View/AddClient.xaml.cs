using System.Windows;
using MainClient._ViewModel;

namespace MainClient._View
{
    public partial class AddClient : Window
    {
        public AddClient()
        {
            InitializeComponent();

            var viewModel = new AddClientVM
            {
                CloseAndLoadAccountAction = (customerId) => this.Close()
            };

            DataContext = viewModel;
        }
    }
}
