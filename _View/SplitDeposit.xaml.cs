using System.Windows.Controls;
using MainClient._ViewModel;

namespace MainClient._View
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SplitDeposit : UserControl
    {
        public SplitDeposit()
        {
            InitializeComponent();
            DataContext = new SplitDepositVM();
        }
    }
}
