using MainClient.Utilities;
using MainClient.Services;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System;

namespace MainClient._ViewModel
{
    class WithdrawalWireVM : ViewModelBase
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
                WireRoutingNumber = value?.BankRoutingNumber;
            }
        }

        private ObservableCollection<CurrencyService.CurrencyInfo> _currencyTypes;
        public ObservableCollection<CurrencyService.CurrencyInfo> CurrencyTypes
        {
            get => _currencyTypes;
            set
            {
                _currencyTypes = value;
                OnPropertyChanged(nameof(CurrencyTypes));
            }
        }

        private CurrencyService.CurrencyInfo _selectedCurrencyType;
        public CurrencyService.CurrencyInfo SelectedCurrencyType
        {
            get => _selectedCurrencyType;
            set
            {
                _selectedCurrencyType = value;
                OnPropertyChanged(nameof(SelectedCurrencyType));
                WireCurrency = value?.CurrencyAbbr;
            }
        }

        public ICommand AddWireWithdrawalCommand { get; }

        public WithdrawalWireVM()
        {
            LoadCurrencyTypes();
            LoadBankRoutingInfos();
            AddWireWithdrawalCommand = new RelayCommand(ExecuteWireWithdrawal);
        }

        public string WireToAccountNumber { get; set; }
        public string WireRoutingNumber { get; set; }
        public string WireCurrency { get; set; }
        public decimal? WireAmount { get; set; }

        private string _wireAmountText;
        public string WireAmountText
        {
            get => _wireAmountText;
            set
            {
                _wireAmountText = value;
                WireAmount = TryParseDecimal(value);
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

        private void ExecuteWireWithdrawal(object parameter)
        {
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            string repId = RepIdService.Instance.RepId;

            if (WireAmount != null)
            {
                MoveMoneyService moveMoneyService = new MoveMoneyService();
            moveMoneyService.InsertAcctTransactionWireWithdrawalByAcctNum(
                accountNumber,
                repId,
                WireToAccountNumber,
                WireAmount,
                WireRoutingNumber,
                WireCurrency
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

        public void LoadCurrencyTypes()
        {
            var currencyInfos = CurrencyService.LookUpCurrencyAbbr();

            CurrencyTypes = new ObservableCollection<CurrencyService.CurrencyInfo>(currencyInfos);
        }
    }
}
