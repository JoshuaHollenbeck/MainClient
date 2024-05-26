using MainClient.Utilities;
using MainClient.Services;
using System;
using System.Windows.Input;
using MainClient._Model;
using System.Windows;
using System.Linq;
using System.Collections.ObjectModel;

namespace MainClient._ViewModel
{
    class WithdrawalAchVM : ViewModelBase
    {
        public ICommand AddAchWithdrawalCommand { get; }

        public WithdrawalAchVM()
        {
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            FetchAchDetails(accountNumber);
            AddAchWithdrawalCommand = new RelayCommand(ExecuteAddAchWithdrawal);
        }

        public decimal? AchAmount { get; set; }

        private string _achAmountText;
        public string AchAmountText
        {
            get => _achAmountText;
            set
            {
                _achAmountText = value;
                AchAmount = TryParseDecimal(value);
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

        private WithdrawalAchModel _selectedAchPayment;
        public WithdrawalAchModel SelectedAchPayment
        {
            get => _selectedAchPayment;
            set
            {
                if (_selectedBank != value)
                {
                    _selectedAchPayment = value;
                    ACHRoutingNumber = _selectedAchPayment?.PaymentRouting;
                    ACHAccountNumber = _selectedAchPayment?.PaymentAcctNum;
                    OnPropertyChanged(nameof(SelectedAchPayment));
                    OnPropertyChanged(nameof(ACHRoutingNumber));
                    OnPropertyChanged(nameof(ACHAccountNumber));
                }
            }
        }
        
        private string _achAccountNumber;
        public string ACHAccountNumber
        {
            get { return _achAccountNumber; }
            set
            {
                _achAccountNumber = value;
                OnPropertyChanged(nameof(ACHAccountNumber));
            }
        }

        public Action<string> CloseAndLoadAccountAction { get; set; }
        
        private void ExecuteAddAchWithdrawal(object parameter)
        {   
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            string repId = RepIdService.Instance.RepId;
            string achAccountNumber = SelectedAchPayment?.PaymentAcctNum;
            string achRoutingNumber = SelectedAchPayment?.PaymentRouting;

            if (AchAmount != null)
            {
                try
                {
                    MoveMoneyService achService = new MoveMoneyService();
                    achService.InsertAcctTransactionAchWithdrawalByAcctNum(
                        accountNumber,
                        repId,
                        achAccountNumber,
                        AchAmount,
                        achRoutingNumber
                    );

                    CloseAndLoadAccountAction?.Invoke(accountNumber);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Failed to make ACH payment: {ex.Message}",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
            else
            {
                MessageBox.Show("No item selected");
            }
        }

        private WithdrawalAchModel _selectedBank;
        public WithdrawalAchModel SelectedBank
        {
            get => _selectedBank;
            set
            {
                if (_selectedBank != value)
                {
                    _selectedBank = value;
                    OnPropertyChanged(nameof(SelectedBank));
                }
            }
        }
        private string _achRoutingNumber;
        public string ACHRoutingNumber
        {
            get => _achRoutingNumber;
            set
            {
                if (_achRoutingNumber != value)
                {
                    _achRoutingNumber = value;
                    OnPropertyChanged(nameof(ACHRoutingNumber));
                }
            }
        }

        private ObservableCollection<WithdrawalAchModel> _achPaymentListResults =
            new ObservableCollection<WithdrawalAchModel>();
        public ObservableCollection<WithdrawalAchModel> AchPaymentListResults
        {
            get => _achPaymentListResults;
            set
            {
                if (_achPaymentListResults != value)
                {
                    _achPaymentListResults = value;
                    OnPropertyChanged(nameof(AchPaymentListResults));
                }
            }
        }

        private void FetchAchDetails(string acctNum)
        {
            var AchPaymentList = WithdrawalAchModel.GetAcctAchListByAcctNum(acctNum);

            if (AchPaymentList != null)
            {
                _achPaymentListResults.Clear();
                foreach (var ACHPayment in AchPaymentList)
                {
                    _achPaymentListResults.Add(ACHPayment);
                }
            }

            if (_achPaymentListResults.Any())
            {
                SelectedAchPayment = _achPaymentListResults.First();
            }

            OnPropertyChanged(String.Empty);
        }
        
    }
}
