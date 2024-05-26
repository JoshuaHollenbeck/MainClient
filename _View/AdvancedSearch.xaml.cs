using System.Windows;
using MainClient._ViewModel;

namespace MainClient._View
{
    public partial class AdvancedSearch : Window
    {
        public AdvancedSearch()
        {
            InitializeComponent();

            var viewModel = new AdvancedSearchVM
            {
                CloseAndLoadAccountAction = (accountNumber) => this.Close()
            };

            DataContext = viewModel;
        }
    }
}
