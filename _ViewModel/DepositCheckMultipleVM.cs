using MainClient.Utilities;
using MainClient.Services;
using System;
using System.Linq;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;

namespace MainClient._ViewModel
{
    public class MultiCheckDetail
    {
        public string MultiPayerName { get; set; }
        public string MultiPayToOrderOf { get; set; }
        public string MultiCheckMemo { get; set; }
        public string MultiCheckRoutingNumber { get; set; }
        public string MultiCheckAccountNumber { get; set; }
        public string MultiCheckNumber { get; set; }
        public DateTime? MultiCheckDate { get; set; }
        public decimal? MultiCheckAmount { get; set; }

        private string _multiCheckAmountText;
        public string MultiCheckAmountText
        {
            get => _multiCheckAmountText;
            set
            {
                _multiCheckAmountText = value;
                MultiCheckAmount = TryParseDecimal(value);
            }
        }

        private decimal? TryParseDecimal(string value)
        {
            if (decimal.TryParse(value, out decimal result))
            {
                return result;
            }
            return null;
        }
    }

    class DepositCheckMultipleVM : ViewModelBase
    {
        private ObservableCollection<BankService.BankRoutingInfo> _bankRoutingInfos;
        public ObservableCollection<BankService.BankRoutingInfo> BankRoutingInfos
        {
            get => _bankRoutingInfos;
            set
            {
                _bankRoutingInfos = value;
                OnPropertyChanged(nameof(BankRoutingInfos));
            }
        }

        private BankService.BankRoutingInfo _selectedRoutingInfo;
        public BankService.BankRoutingInfo SelectedRoutingInfo
        {
            get => _selectedRoutingInfo;
            set
            {
                _selectedRoutingInfo = value;
                OnPropertyChanged(nameof(SelectedRoutingInfo));
                NewMultiCheck.MultiCheckRoutingNumber = value?.BankRoutingNumber;
            }
        }

        private ObservableCollection<MultiCheckDetail> _multiCheckDetails =
            new ObservableCollection<MultiCheckDetail>();
        public ObservableCollection<MultiCheckDetail> MultiCheckDetails
        {
            get => _multiCheckDetails;
            set
            {
                _multiCheckDetails = value;
                OnPropertyChanged();
            }
        }

        private MultiCheckDetail _newMultiCheck = new MultiCheckDetail();
        public MultiCheckDetail NewMultiCheck
        {
            get => _newMultiCheck;
            set
            {
                _newMultiCheck = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddMultiCheckCommand { get; }
        public ICommand AddDepositCheckMultipleCommand { get; }

        public DepositCheckMultipleVM()
        {
            LoadBankRoutingInfos();
            AddMultiCheckCommand = new RelayCommand(AddMultiCheck);
            AddDepositCheckMultipleCommand = new RelayCommand(AddDepositCheckMultiple);
        }

        private void AddMultiCheck(object parameter)
        {
            MultiCheckDetails.Add(NewMultiCheck);
            NewMultiCheck = new MultiCheckDetail();
        }

        public Action<string> CloseAndLoadAccountAction { get; set; }

        private void AddDepositCheckMultiple(object parameter)
        {
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            string repId = RepIdService.Instance.RepId;

            if (MultiCheckDetails != null)
            {
                MoveMoneyService multiCheckService = new MoveMoneyService();
                multiCheckService.InsertAcctTransactionCheckDepositMultipleCheckByAcctNum(
                    MultiCheckDetails,
                    accountNumber,
                    repId
                );
                MultiCheckDetails.Clear();

                CloseAndLoadAccountAction?.Invoke(accountNumber);
            }
            else
            {
                MessageBox.Show("No item selected");
            }
        }

        public void LoadBankRoutingInfos()
        {
            var routingInfos = BankService.LookUpBankRouting();

            var maxLength = routingInfos.Max(info => info.BankRoutingNumber.Length);
            var sortedRoutingInfos = routingInfos
                .OrderBy(info => info.BankRoutingNumber.PadLeft(maxLength, '0'))
                .ToList();

            BankRoutingInfos = new ObservableCollection<BankService.BankRoutingInfo>(
                sortedRoutingInfos
            );
        }
    }
}
