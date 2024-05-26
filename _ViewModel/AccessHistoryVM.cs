using System;
using MainClient._Model;
using MainClient.Utilities;
using System.Collections.ObjectModel;

namespace MainClient._ViewModel
{
    class AccessHistoryVM : ViewModelBase
    {
        private ObservableCollection<AccessHistoryModel> _acctAccessHistoryResults = new ObservableCollection<AccessHistoryModel>();
        public ObservableCollection<AccessHistoryModel> AcctAccessHistoryResults
        {
            get  => _acctAccessHistoryResults;
            set
            {
                if (_acctAccessHistoryResults != value)
                {
                    _acctAccessHistoryResults = value;
                    OnPropertyChanged(nameof(AcctAccessHistoryResults));
                }
            }
        }
        public AccessHistoryVM(string accountNumber)
        {
            string acctNum = accountNumber;
            FetchClientAccessHistoryDetails(acctNum);
        }

        private void FetchClientAccessHistoryDetails(string acctNum)
        {
            var acctAccessHistoryList = AccessHistoryModel.GetClientAccessHistoryByAcctNum(acctNum);
            if (acctAccessHistoryList != null)
            {
                _acctAccessHistoryResults.Clear();
                foreach(var acctAccessHistory in acctAccessHistoryList)
                {
                    _acctAccessHistoryResults.Add(acctAccessHistory);
                }
            }

            OnPropertyChanged(String.Empty);
        }
    }
}
