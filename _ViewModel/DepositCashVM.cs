using MainClient.Utilities;
using MainClient.Services;
using System;
using System.Windows.Input;
using System.Windows;

namespace MainClient._ViewModel
{
    class DepositCashVM : ViewModelBase
    {
        public ICommand AddCashDepositCommand { get; }

        public DepositCashVM()
        {
            AddCashDepositCommand = new RelayCommand(ExecuteAddCashDeposit);
        }

        public decimal? CashAmount { get; set; }

        private string _cashAmountText;
        public string CashAmountText
        {
            get => _cashAmountText;
            set
            {
                _cashAmountText = value;
                CashAmount = TryParseDecimal(value);
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

        private void ExecuteAddCashDeposit(object parameter)
        {
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            string repId = RepIdService.Instance.RepId;

            if (CashAmount != null)
            {
                MoveMoneyService cashService = new MoveMoneyService();
                cashService.InsertAcctTransactionCashDepositByAcctNum(
                    accountNumber,
                    repId,
                    CashAmount
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
