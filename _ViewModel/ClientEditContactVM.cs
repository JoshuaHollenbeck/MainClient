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
    class ClientEditContactVM : ViewModelBase
    {
        private ObservableCollection<ClientEditContactModel> _clientContactResults =
            new ObservableCollection<ClientEditContactModel>();
        public ObservableCollection<ClientEditContactModel> ClientContactResults
        {
            get => _clientContactResults;
            set
            {
                if (_clientContactResults != value)
                {
                    _clientContactResults = value;
                    OnPropertyChanged(nameof(ClientContactResults));
                }
            }
        }

        public ICommand UpdateClientContactCommand { get; }

        public ClientEditContactVM()
        {            
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            FetchContactDetails(accountNumber);
            
            UpdateClientContactCommand = new RelayCommand(ExecuteUpdateClientContact);
        }

        public Action<string> CloseAndLoadAccountAction { get; set; }

        private string _newCustEmail;
        private string _newCustPhoneHome;
        private string _newCustPhoneBusiness;
        private string _newCustAddress;
        private string _newCustAddress2;
        private string _newCustPostalCode;

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

        public string CustEmail { get; private set; }
        public string CustPhoneHome { get; private set; }
        public string CustPhoneBusiness { get; private set; }
        public string CustAddress { get; private set; }
        public string CustAddress2 { get; private set; }
        public string CustCity { get; private set; }
        public string CustState { get; private set; }
        public string CustPostalCode { get; private set; }
        
        public string NewCustEmail
        {
            get => _newCustEmail;
            set { _newCustEmail = value; OnPropertyChanged(); }
        }
        public string NewCustPhoneHome
        {
            get => _newCustPhoneHome;
            set { _newCustPhoneHome = value; OnPropertyChanged(); }
        }
        public string NewCustPhoneBusiness
        {
            get => _newCustPhoneBusiness;
            set { _newCustPhoneBusiness = value; OnPropertyChanged(); }
        }
        public string NewCustAddress
        {
            get => _newCustAddress;
            set { _newCustAddress = value; OnPropertyChanged(); }
        }
        public string NewCustAddress2
        {
            get => _newCustAddress2;
            set { _newCustAddress2 = value; OnPropertyChanged(); }
        }
        public string NewCustPostalCode
        {
            get => _newCustPostalCode;
            set { _newCustPostalCode = value; OnPropertyChanged(); }
        }
        private void ExecuteUpdateClientContact(object parameter)
        {   
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            if (accountNumber != null)
            {
                ClientEditContactModel editContact = new ClientEditContactModel();
                editContact.EditClientContact(
                    accountNumber,
                    NewCustEmail,
                    NewCustPhoneHome,
                    NewCustPhoneBusiness,
                    NewCustAddress,
                    NewCustAddress2,
                    NewCustPostalCode,
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
            var clientContactInfoList = ClientEditContactModel.GetClientContactByAcctNum(accountNumber).FirstOrDefault();
            if (clientContactInfoList != null)
            {   
                CustEmail = clientContactInfoList.CustEmail;
                CustPhoneHome = clientContactInfoList.CustPhoneHome;
                CustPhoneBusiness = clientContactInfoList.CustPhoneBusiness;
                CustAddress = clientContactInfoList.CustAddress;
                CustAddress2 = clientContactInfoList.CustAddress2;
                CustPostalCode = clientContactInfoList.CustPostalCode;

                // Initialize the new properties with existing data
                NewCustEmail = CustEmail;
                NewCustPhoneHome = CustPhoneHome;
                NewCustPhoneBusiness = CustPhoneBusiness;
                NewCustAddress = CustAddress;
                NewCustAddress2 = CustAddress2;
                NewCustPostalCode = CustPostalCode;
                OnPropertyChanged("");
            }

            OnPropertyChanged(String.Empty);
        }
    }
}