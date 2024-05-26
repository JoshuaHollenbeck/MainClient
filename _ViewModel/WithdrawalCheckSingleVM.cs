using MainClient.Utilities;
using MainClient.Services;
using System;
using System.Windows.Input;
using System.Windows;

namespace MainClient._ViewModel
{
    class WithdrawalCheckSingleVM : ViewModelBase
    {
        public ICommand AddCheckWithdrawalCommand { get; }

        public WithdrawalCheckSingleVM()
        {
            AddCheckWithdrawalCommand = new RelayCommand(ExecuteCheckWithdrawal);
        }

        public string SinglePayToOrderOf { get; set; }
        public string SingleCheckNumber { get; set; }
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

        private void ExecuteCheckWithdrawal(object parameter)
        {
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            string repId = RepIdService.Instance.RepId;

            if (SingleCheckAmount != null)
            {
                MoveMoneyService moveMoneyService = new MoveMoneyService();
                moveMoneyService.InsertAcctTransactionCheckWithdrawalByAcctNum(
                    accountNumber,
                    repId,
                    SingleCheckAmount,
                    SinglePayToOrderOf,
                    SingleCheckNumber,
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
    }
}
