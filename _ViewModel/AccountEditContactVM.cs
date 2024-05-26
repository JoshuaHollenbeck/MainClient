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
    class AccountEditContactVM : ViewModelBase
    {
        private ObservableCollection<AccountEditContactModel> _acctContactResults =
            new ObservableCollection<AccountEditContactModel>();
        public ObservableCollection<AccountEditContactModel> AcctContactResults
        {
            get => _acctContactResults;
            set
            {
                if (_acctContactResults != value)
                {
                    _acctContactResults = value;
                    OnPropertyChanged(nameof(AcctContactResults));
                }
            }
        }

        public ICommand UpdateAccountContactCommand { get; }

        public AccountEditContactVM()
        {            
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            FetchContactDetails(accountNumber);
            
            UpdateAccountContactCommand = new RelayCommand(ExecuteUpdateAccountContact);
        }

        public Action<string> CloseAndLoadAccountAction { get; set; }

        private string _newContactName;
        private string _newContactAddress1;
        private string _newContactAddress2;
        private string _newContactPostalCode;

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

        public string ContactName { get; private set; }
        public string ContactAddress1 { get; private set; }
        public string ContactAddress2 { get; private set; }
        public string ContactPostalCode { get; private set; }
        
        public string NewContactName
        {
            get => _newContactName;
            set { _newContactName = value; OnPropertyChanged(); }
        }
        public string NewContactAddress1
        {
            get => _newContactAddress1;
            set { _newContactAddress1 = value; OnPropertyChanged(); }
        }
        public string NewContactAddress2
        {
            get => _newContactAddress2;
            set { _newContactAddress2 = value; OnPropertyChanged(); }
        }
        public string NewContactPostalCode
        {
            get => _newContactPostalCode;
            set { _newContactPostalCode = value; OnPropertyChanged(); }
        }

        private void ExecuteUpdateAccountContact(object parameter)
        {   
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            if (accountNumber != null)
            {
                AccountEditContactModel editContact = new AccountEditContactModel();
                editContact.EditAcctContact(
                    accountNumber,
                    NewContactName,
                    NewContactAddress1,
                    NewContactAddress2,
                    NewContactPostalCode,
                    SelectedAllAccts
                );
                                
                CloseAndLoadAccountAction?.Invoke(accountNumber);
            }
            else
            {
                MessageBox.Show("No item selected");
            }
        }

        private void FetchContactDetails(string accountNumber)
        {
            var acctContactInfoList = AccountEditContactModel.GetAcctContactByAcctNum(accountNumber).FirstOrDefault();
            if (acctContactInfoList != null)
            {
                ContactName = acctContactInfoList.ContactName;
                ContactAddress1 = acctContactInfoList.ContactAddress1;
                ContactAddress2 = acctContactInfoList.ContactAddress2;
                ContactPostalCode = acctContactInfoList.ContactPostalCode;

                // Initialize the new properties with existing data
                NewContactName = ContactName;
                NewContactAddress1 = ContactAddress1;
                NewContactAddress2 = ContactAddress2;
                NewContactPostalCode = ContactPostalCode;
                OnPropertyChanged("");
            }

            OnPropertyChanged(String.Empty);
        }
    }
}