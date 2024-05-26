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
    class ClientEditEmploymentVM : ViewModelBase
    {
        private ObservableCollection<ClientEditEmploymentModel> _clientEmploymentResults =
            new ObservableCollection<ClientEditEmploymentModel>();
        public ObservableCollection<ClientEditEmploymentModel> CientEmploymentResults
        {
            get => _clientEmploymentResults;
            set
            {
                if (_clientEmploymentResults != value)
                {
                    _clientEmploymentResults = value;
                    OnPropertyChanged(nameof(CientEmploymentResults));
                }
            }
        }

        public ICommand UpdateClientEmploymentCommand { get; }

        public ClientEditEmploymentVM()
        {
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            FetchEmploymentDetails(accountNumber);

            UpdateClientEmploymentCommand = new RelayCommand(ExecuteUpdateClientEmployment);
        }

        public Action<string> CloseAndLoadAccountAction { get; set; }

        public string EmploymentStatus { get; private set; }
        public string EmployerName { get; private set; }
        public string Occupation { get; private set; }

        private string _newEmploymentStatus;
        public string NewEmploymentStatus
        {
            get => _newEmploymentStatus;
            set
            {
                _newEmploymentStatus = value;
                OnPropertyChanged();
            }
        }

        private string _newEmployerName;
        public string NewEmployerName
        {
            get => _newEmployerName;
            set
            {
                _newEmployerName = value;
                OnPropertyChanged();
            }
        }

        private string _newOccupation;
        public string NewOccupation
        {
            get => _newOccupation;
            set
            {
                _newOccupation = value;
                OnPropertyChanged();
            }
        }

        private void ExecuteUpdateClientEmployment(object parameter)
        {
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            if (accountNumber != null)
            {
                ClientEditEmploymentModel editContact = new ClientEditEmploymentModel();
                editContact.EditClientEmployment(
                    accountNumber,
                    NewEmploymentStatus,
                    NewEmployerName,
                    NewOccupation
                );

                CloseAndLoadAccountAction?.Invoke(accountNumber);
            }
            else
            {
                MessageBox.Show("No item selected");
            }
        }

        private void FetchEmploymentDetails(string accountNumber)
        {
            var clientEmploymentInfoList = ClientEditEmploymentModel
                .GetClientEmploymentByAcctNum(accountNumber)
                .FirstOrDefault();
            if (clientEmploymentInfoList != null)
            {
                EmploymentStatus = clientEmploymentInfoList.EmploymentStatus;
                EmployerName = clientEmploymentInfoList.EmployerName;
                Occupation = clientEmploymentInfoList.Occupation;

                // Initialize the new properties with existing data
                NewEmploymentStatus = EmploymentStatus;
                NewEmployerName = EmployerName;
                NewOccupation = Occupation;
                OnPropertyChanged("");
            }

            OnPropertyChanged(String.Empty);
        }
    }
}
