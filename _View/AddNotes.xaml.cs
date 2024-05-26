using System.Windows;
using MainClient._ViewModel;

namespace MainClient._View
{
    public partial class AddNotes : Window
    {
        public AddNotes()
        {
            InitializeComponent();
            
            var viewModel = new AddNotesVM
            {
                CloseAndLoadAccountAction = (accountNumber) => this.Close()
            };

            DataContext = viewModel;        
        }
    }
}
