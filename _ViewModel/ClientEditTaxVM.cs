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
    class ClientEditTaxVM : ViewModelBase
    {
        private ObservableCollection<ClientEditTaxModel> _clientTaxResults =
            new ObservableCollection<ClientEditTaxModel>();
        public ObservableCollection<ClientEditTaxModel> CientTaxResults
        {
            get => _clientTaxResults;
            set
            {
                if (_clientTaxResults != value)
                {
                    _clientTaxResults = value;
                    OnPropertyChanged(nameof(CientTaxResults));
                }
            }
        }

        public ICommand UpdateClientTaxCommand { get; }

        public ClientEditTaxVM()
        {
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            FetchTaxDetails(accountNumber);

            UpdateClientTaxCommand = new RelayCommand(ExecuteUpdateClientTax);
        }

        public Action<string> CloseAndLoadAccountAction { get; set; }

        public string CustTaxId { get; private set; }
        
        private string _newCustTaxId;
        public string NewCustTaxId
        {
            get => _newCustTaxId;
            set
            {
                _newCustTaxId = value;
                OnPropertyChanged();
            }
        }

        private void ExecuteUpdateClientTax(object parameter)
        {
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            if (accountNumber != null)
            {
                ClientEditTaxModel editContact = new ClientEditTaxModel();
                editContact.EditClientTax(
                    accountNumber,
                    NewCustTaxId
                );

                CloseAndLoadAccountAction?.Invoke(accountNumber);
            }
            else
            {
                MessageBox.Show("No item selected");
            }
        }

        private void FetchTaxDetails(string accountNumber)
        {
            var clientTaxInfoList = ClientEditTaxModel
                .GetClientTaxByAcctNum(accountNumber)
                .FirstOrDefault();
            if (clientTaxInfoList != null)
            {
                CustTaxId = clientTaxInfoList.CustTaxId;

                // Initialize the new properties with existing data
                NewCustTaxId = CustTaxId;
                OnPropertyChanged("");
            }

            OnPropertyChanged(String.Empty);
        }
    }
}