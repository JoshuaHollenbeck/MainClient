using MainClient.Utilities;
using MainClient.Services;
using System;
using System.Linq;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;

namespace MainClient._ViewModel
{
    class DepositCheckSingleVM : ViewModelBase
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
                SingleCheckRoutingNumber = value?.BankRoutingNumber;
            }
        }

        public ICommand AddDepositCheckSingleCommand { get; }

        public DepositCheckSingleVM()
        {
            LoadBankRoutingInfos();
            AddDepositCheckSingleCommand = new RelayCommand(AddDepositCheckSingle);
        }

        public string SinglePayerName { get; set; }
        public string SinglePayToOrderOf { get; set; }
        public string SingleCheckAccountNumber { get; set; }
        public string SingleCheckNumber { get; set; }
        public string SingleCheckRoutingNumber { get; set; }
        public DateTime? SingleCheckDate { get; set; }
        public string SingleCheckMemo { get; set; }
        public decimal? SingleCheckAmount { get; set; }

        private string _singleCheckAmountText;
        public string SingleCheckAmountText
        {
            get => _singleCheckAmountText;
            set
            {
                _singleCheckAmountText = value;
                SingleCheckAmount = TryParseDecimal(value);
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

        public Action<string> CloseAndLoadAccountAction { get; set; }

        private void AddDepositCheckSingle(object parameter)
        {
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            string repId = RepIdService.Instance.RepId;

            if (SingleCheckAmount != null)
            {
                MoveMoneyService singleCheckService = new MoveMoneyService();
                singleCheckService.InsertAcctTransactionCheckDepositSingleCheckByAcctNum(
                    accountNumber,
                    repId,
                    SingleCheckAmount,
                    SinglePayerName,
                    SinglePayToOrderOf,
                    SingleCheckAccountNumber,
                    SingleCheckNumber,
                    SingleCheckRoutingNumber,
                    SingleCheckDate,
                    SingleCheckMemo
                );

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
