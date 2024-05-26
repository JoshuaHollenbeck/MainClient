using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using MainClient._Model;
using MainClient.Services;
using MainClient.Utilities;

namespace MainClient._ViewModel
{
    class AdvancedSearchVM : ViewModelBase
    {
        private readonly AdvancedSearchModel _advancedSearchService;
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string CustomerID { get; set; }
        public string CustPhone { get; set; }
        public string CustTaxId { get; set; }
        public string CustZip { get; set; }
        public string AcctNum { get; set; }
        public string CustEmail { get; set; }

        private ObservableCollection<AdvancedSearchModel.AdvancedSearchResult> _advancedSearchResults;

        public ObservableCollection<AdvancedSearchModel.AdvancedSearchResult> AdvancedSearchResults
        {
            get { return _advancedSearchResults; }
            set
            {
                _advancedSearchResults = value;
                OnPropertyChanged(nameof(AdvancedSearchResults));
            }
        }

        public AdvancedSearchVM()
        {
            _advancedSearchService = new AdvancedSearchModel();
            AdvancedSearchResults =
                new ObservableCollection<AdvancedSearchModel.AdvancedSearchResult>();
            PerformAdvancedSearchCommand = new RelayCommand(ExecuteAdvancedSearch);
            OpenAccountOverviewCommand = new RelayCommand(OpenAccountOverview);
        }

        public ICommand PerformAdvancedSearchCommand { get; private set; }

        private void ExecuteAdvancedSearch(object parameter)
        {
            PerformAdvancedSearch(
                lastName: this.LastName,
                firstName: this.FirstName,
                middleInitial: this.MiddleInitial,
                CustomerID: this.CustomerID,
                custPhone: this.CustPhone,
                custZip: this.CustZip,
                custTaxId: this.CustTaxId,
                acctNum: this.AcctNum,
                custEmail: this.CustEmail
            );
        }

        // Method to perform the search and update SearchResults
        public void PerformAdvancedSearch(
            string lastName,
            string firstName,
            string middleInitial,
            string CustomerID,
            string custPhone,
            string custZip,
            string custTaxId,
            string acctNum,
            string custEmail
        )
        {
            var results = _advancedSearchService.AcctAdvancedSearch(
                lastName,
                firstName,
                middleInitial,
                CustomerID,
                custPhone,
                custZip,
                custTaxId,
                acctNum,
                custEmail
            );
            Application.Current.Dispatcher.Invoke(() =>
            {
                AdvancedSearchResults.Clear();
                foreach (var result in results)
                {
                    AdvancedSearchResults.Add(result);
                }

                ResetSearchFields();
            });
        }

        private void ResetSearchFields()
        {
            LastName = null;
            FirstName = null;
            MiddleInitial = null;
            CustomerID = null;
            CustPhone = null;
            CustZip = null;
            CustTaxId = null;
            AcctNum = null;
            CustEmail = null;

            OnPropertyChanged(nameof(LastName));
            OnPropertyChanged(nameof(FirstName));
            OnPropertyChanged(nameof(MiddleInitial));
            OnPropertyChanged(nameof(CustomerID));
            OnPropertyChanged(nameof(CustPhone));
            OnPropertyChanged(nameof(CustZip));
            OnPropertyChanged(nameof(CustTaxId));
            OnPropertyChanged(nameof(AcctNum));
            OnPropertyChanged(nameof(CustEmail));
        }

        private AdvancedSearchModel.AdvancedSearchResult _selectedItem;

        public AdvancedSearchModel.AdvancedSearchResult SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public Action<string> CloseAndLoadAccountAction { get; set; }

        public ICommand OpenAccountOverviewCommand { get; private set; }

        public void OpenAccountOverview(object parameter)
        {
            if (SelectedItem != null)
            {
                // Set the global account number when an account is selected
                AccountNumService.Instance.SelectedAccountNumber = SelectedItem.AccountNumber;
                AccountNumService.Instance.SelectedAccountType = SelectedItem.AccountType;
                ClientIdService.Instance.SelectedCustId = SelectedItem.CustomerID;
                ClientIdService.Instance.SelectedJointCustId = SelectedItem.JointCustomerId;
                string repId = RepIdService.Instance.RepId;
                CloseAndLoadAccountAction?.Invoke(SelectedItem.AccountNumber);

                // AdvancedSearchModel.InsertAcctAccessHistoryByAcctNum(SelectedItem.AccountNumber, repId);  
            }
            else
            {
                MessageBox.Show("No item selected");
            }
        }
    }
}
