using MainClient.Utilities;
using MainClient.Services;
using System.Windows.Input;
using System;
using System.Windows;
using System.Linq;
using MainClient._Model;
using System.Collections.ObjectModel;


namespace MainClient._ViewModel
{
    class WithdrawalJournalVM : ViewModelBase
    {
        public ICommand JournalWithdrawalCommand { get; }

        public WithdrawalJournalVM()
        {
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            FetchJournalDetails(accountNumber);
            JournalWithdrawalCommand = new RelayCommand(ExecuteJournalWithdrawal);
        }

        public decimal? JournalAmount { get; set; }

        private string _journalAmountText;
        public string JournalAmountText
        {
            get => _journalAmountText;
            set
            {
                _journalAmountText = value;
                JournalAmount = TryParseDecimal(value);
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

        private void ExecuteJournalWithdrawal(object parameter)
        {   
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            string repId = RepIdService.Instance.RepId;
            
            if (JournalAmount != null)
            {
                MoveMoneyService journalService = new MoveMoneyService();
                journalService.InsertAcctTransactionJournalByAcctNum(
                    accountNumber,
                    SelectedAcct?.AcctNum,
                    JournalAmount,
                    repId
                );
                                
                CloseAndLoadAccountAction?.Invoke(accountNumber);
            }
            else
            {
                MessageBox.Show("No item selected");
            }
        }
                
        private void FetchJournalDetails(string acctNum)
        {
            var acctJournalInfoList = ClientService.GetClientAcctJournalListByAcctNum(acctNum);
            if (acctJournalInfoList != null)
            {
                _acctJournalResults.Clear();
                foreach (var journalInfo in acctJournalInfoList)
                {
                    var journalModel = new ClientService.JournalAcctInfo
                    {
                        AcctNum = journalInfo.AcctNum
                    };
                    _acctJournalResults.Add(journalModel);
                }
            }

            if (_acctJournalResults.Any())
            {
                SelectedAcct = _acctJournalResults.First();
            }
        }
        
        private ObservableCollection<ClientService.JournalAcctInfo> _acctJournalResults =
            new ObservableCollection<ClientService.JournalAcctInfo>();
        public ObservableCollection<ClientService.JournalAcctInfo> AcctJournalResults
        {
            get => _acctJournalResults;
            set
            {
                if (_acctJournalResults != value)
                {
                    _acctJournalResults = value;
                    OnPropertyChanged(nameof(AcctJournalResults));
                }
            }
        }

        private ClientService.JournalAcctInfo _selectedAcct;
        public ClientService.JournalAcctInfo  SelectedAcct
        {
            get => _selectedAcct;
            set
            {
                if (_selectedAcct != value)
                {
                    _selectedAcct = value;
                    OnPropertyChanged(nameof(SelectedAcct));
                }
            }
        }
    }
}
