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
    class AccountEditPasswordVM : ViewModelBase
    {
        private ObservableCollection<AccountEditPasswordModel> _acctPassResults =
            new ObservableCollection<AccountEditPasswordModel>();
        public ObservableCollection<AccountEditPasswordModel> AcctPassResults
        {
            get => _acctPassResults;
            set
            {
                if (_acctPassResults != value)
                {
                    _acctPassResults = value;
                    OnPropertyChanged(nameof(AcctPassResults));
                }
            }
        }

        public ICommand UpdateAccountPasswordCommand { get; }

        public AccountEditPasswordVM()
        {            
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            FetchPasswordDetails(accountNumber);
            
            UpdateAccountPasswordCommand = new RelayCommand(ExecuteUpdateAccountPass);
        }

        public Action<string> CloseAndLoadAccountAction { get; set; }

        private string _newAcctPass;

        public string AcctPass { get; private set; }
        
        public string NewAcctPass
        {
            get => _newAcctPass;
            set { _newAcctPass = value; OnPropertyChanged(); }
        }

        private void ExecuteUpdateAccountPass(object parameter)
        {   
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            if (accountNumber != null)
            {
                AccountEditPasswordModel editContact = new AccountEditPasswordModel();
                editContact.EditAcctPass(
                    accountNumber,
                    NewAcctPass
                );
                                
                CloseAndLoadAccountAction?.Invoke(accountNumber);
            }
            else
            {
                MessageBox.Show("No item selected");
            }
        }

        private void FetchPasswordDetails(string accountNumber)
        {
            var acctPassInfoList = AccountEditPasswordModel.GetAcctPassByAcctNum(accountNumber).FirstOrDefault();
            if (acctPassInfoList != null)
            {
                AcctPass = acctPassInfoList.AcctPass;
                
                // Initialize the new properties with existing data
                NewAcctPass = AcctPass;
                OnPropertyChanged("");
            }

            OnPropertyChanged(String.Empty);
        }
    }
}