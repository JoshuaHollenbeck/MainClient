using MainClient.Utilities;
using MainClient.Services;
using System;
using System.Linq;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;

namespace MainClient._ViewModel
{
    public class SplitCheckDetail
    {
        public string SplitAccountNumber { get; set; }
        public decimal? SplitCheckAmount { get; set; }

        private string _splitCheckAmountText;
        public string SplitCheckAmountText
        {
            get => _splitCheckAmountText;
            set
            {
                _splitCheckAmountText = value;
                SplitCheckAmount = TryParseDecimal(value);
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

    class DepositCheckSplitVM : ViewModelBase
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
                SplitCheckRoutingNumber = value?.BankRoutingNumber;
            }
        }

        private ObservableCollection<SplitCheckDetail> _splitcheckDetails =
            new ObservableCollection<SplitCheckDetail>();
        public ObservableCollection<SplitCheckDetail> SplitCheckDetails
        {
            get => _splitcheckDetails;
            set
            {
                _splitcheckDetails = value;
                OnPropertyChanged();
            }
        }

        private SplitCheckDetail _newSplitCheck = new SplitCheckDetail();
        public SplitCheckDetail NewSplitCheck
        {
            get => _newSplitCheck;
            set
            {
                _newSplitCheck = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddSplitCommand { get; }
        public ICommand AddDepositCheckSplitCommand { get; }

        public DepositCheckSplitVM()
        {
            LoadBankRoutingInfos();
            AddSplitCommand = new RelayCommand(AddSplitCheck);
            AddDepositCheckSplitCommand = new RelayCommand(AddDepositCheckSplit);
        }

        private void AddSplitCheck(object parameter)
        {
            SplitCheckDetails.Add(NewSplitCheck);
            NewSplitCheck = new SplitCheckDetail();
            OnPropertyChanged(nameof(SplitCheckDetails));
            OnPropertyChanged(nameof(NewSplitCheck));
        }

        public string SplitPayerName { get; set; }
        public string SplitPayToOrderOf { get; set; }
        public string SplitCheckAccountNumber { get; set; }
        public string SplitCheckNumber { get; set; }
        public string SplitCheckRoutingNumber { get; set; }
        public DateTime? SplitCheckDate { get; set; }
        public string SplitCheckMemo { get; set; }

        public decimal? TotalSplitCheckAmount { get; set; }

        private string _totalSplitCheckAmountText;
        public string TotalSplitCheckAmountText
        {
            get => _totalSplitCheckAmountText;
            set
            {
                _totalSplitCheckAmountText = value;
                TotalSplitCheckAmount = TryParseDecimal(value);
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

        private void AddDepositCheckSplit(object parameter)
        {
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            string repId = RepIdService.Instance.RepId;

            if (TotalSplitCheckAmount != null)
            {
                MoveMoneyService splitCheckService = new MoveMoneyService();
                splitCheckService.InsertAcctTransactionCheckDepositSingleCheckSplitByAcctNum(
                    SplitCheckDetails,
                    repId,
                    SplitPayerName,
                    SplitPayToOrderOf,
                    SplitCheckAccountNumber,
                    SplitCheckNumber,
                    SplitCheckRoutingNumber,
                    SplitCheckDate,
                    SplitCheckMemo,
                    TotalSplitCheckAmount
                );
                SplitCheckDetails.Clear();

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
