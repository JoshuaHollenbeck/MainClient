using System.Windows;
using MainClient._ViewModel;

namespace MainClient._View
{
    public partial class Search : Window
    {
        public Search()
        {
            InitializeComponent();

            var viewModel = new SearchVM
            {
                CloseAndLoadAccountAction = (accountNumber) => this.Close()
            };
        }
    }
}
