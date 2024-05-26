using System;
using System.Windows;
using MainClient._Model;
using MainClient.Utilities;
using System.Collections.ObjectModel;
using MainClient.Services;
using System.Windows.Input;
using System.Linq;

namespace MainClient._ViewModel
{
    class AccountEditLimitVM : ViewModelBase
    {
        private ObservableCollection<AccountEditLimitModel> _acctLimitResults =
            new ObservableCollection<AccountEditLimitModel>();
        public ObservableCollection<AccountEditLimitModel> AcctLimitResults
        {
            get => _acctLimitResults;
            set
            {
                if (_acctLimitResults != value)
                {
                    _acctLimitResults = value;
                    OnPropertyChanged(nameof(AcctLimitResults));
                }
            }
        }

        public ICommand UpdateAccountLimitCommand { get; }

        public AccountEditLimitVM()
        {            
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            FetchLimitDetails(accountNumber);
            
            UpdateAccountLimitCommand = new RelayCommand(ExecuteUpdateAccountLimit);
        }

        public Action<string> CloseAndLoadAccountAction { get; set; }

        public int? AchLimit { get; private set; }
        public int? AtmLimit { get; private set; }
        public int? WireLimit { get; private set; }

        private int? _newAchLimit;
        public int? NewAchLimit
        {
            get => _newAchLimit;
            set { _newAchLimit = value; OnPropertyChanged(); }
        }

        private int? _newAtmLimit;
        public int? NewAtmLimit
        {
            get => _newAtmLimit;
            set { _newAtmLimit = value; OnPropertyChanged(); }
        }

        private int? _newWireLimit;
        public int? NewWireLimit
        {
            get => _newWireLimit;
            set { _newWireLimit = value; OnPropertyChanged(); }
        }

        private bool _selectedAllAccts;
        public bool SelectedAllAccts
        {
            get => _selectedAllAccts;
            set
            {
                _selectedAllAccts = value;
                OnPropertyChanged(nameof(SelectedAllAccts));
            }
        }

        private void ExecuteUpdateAccountLimit(object parameter)
        {   
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            if (accountNumber != null)
            {
                AccountEditLimitModel editLimit = new AccountEditLimitModel();
                editLimit.EditAcctLimit(
                    accountNumber,
                    NewAchLimit,
                    NewAtmLimit,
                    NewWireLimit,
                    SelectedAllAccts
                );
                                
                CloseAndLoadAccountAction?.Invoke(accountNumber);
            }
            else
            {
                MessageBox.Show("No item selected");
            }
        }

        private void FetchLimitDetails(string accountNumber)
        {
            var acctLimitInfoList = AccountEditLimitModel.GetAcctLimitByAcctNum(accountNumber).FirstOrDefault();
            if (acctLimitInfoList != null)
            {
                AchLimit = acctLimitInfoList.AchLimit;
                AtmLimit = acctLimitInfoList.AtmLimit;
                WireLimit = acctLimitInfoList.WireLimit;
                
                // Initialize the new properties with existing data
                NewAchLimit = AchLimit;
                NewAtmLimit = AtmLimit;
                NewWireLimit = WireLimit;
                OnPropertyChanged("");
            }

            OnPropertyChanged(String.Empty);
        }
    }
}