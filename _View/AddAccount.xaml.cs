using System.Windows;
using MainClient._ViewModel;

namespace MainClient._View
{
    public partial class AddAccount : Window
    {
       
        public AddAccount()
        {
            InitializeComponent();

            var viewModel = new AddAccountVM
            {
                CloseAndLoadAccountAction = (accountNumber) => this.Close()
            };

            DataContext = viewModel;
        }

    }
}