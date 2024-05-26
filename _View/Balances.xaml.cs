using System.Windows.Controls;
using MainClient._ViewModel;

namespace MainClient._View
{
    /// <summary>
    /// Interaction logic for Orders.xaml
    /// </summary>
    public partial class Balances : UserControl
    {
        public Balances()
        {
            InitializeComponent();
            DataContext = new BalancesVM();
        }
    }
}
