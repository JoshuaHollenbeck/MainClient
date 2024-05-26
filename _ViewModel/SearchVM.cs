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
    class SearchVM : ViewModelBase
    {
        private ObservableCollection<SearchModel.SearchResult> _searchResults;
        public ObservableCollection<SearchModel.SearchResult> SearchResults
        {
            get { return _searchResults; }
            set
            {
                _searchResults = value;
                OnPropertyChanged(nameof(SearchResults));
            }
        }

        public SearchVM()
        {
            SearchResults = new ObservableCollection<SearchModel.SearchResult>();
            OpenAccountOverviewCommand = new RelayCommand(OpenAccountOverview);            
        }

        private SearchModel.SearchResult _selectedItem;
        public SearchModel.SearchResult SelectedItem
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