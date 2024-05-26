using MainClient.Utilities;
using MainClient.Services;
using System;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;
using MainClient._Model;

namespace MainClient._ViewModel
{
    public class MultiPowerOfAttorneyDetail
    {
        public string MultiPoaFirstName { get; set; }
        public string MultiPoaLastName { get; set; }
        public string MultiPoaTaxId { get; set; }
        public string MultiPoaRole { get; set; }
    }

    class AccountAddPowerOfAttorneyVM : ViewModelBase
    {
        private ObservableCollection<AddAccountService.PoaRoleInfo> _poaInfos;
        public ObservableCollection<AddAccountService.PoaRoleInfo> PoaInfos
        {
            get => _poaInfos;
            set
            {
                _poaInfos = value;
                OnPropertyChanged(nameof(PoaInfos));
            }
        }

        private AddAccountService.PoaRoleInfo _selectedPoaRoleInfo;
        public AddAccountService.PoaRoleInfo SelectedPoaRoleInfo
        {
            get => _selectedPoaRoleInfo;
            set
            {
                _selectedPoaRoleInfo = value;
                OnPropertyChanged(nameof(SelectedPoaRoleInfo));
                NewMultiPowerOfAttorney.MultiPoaRole = value?.PoaRole;
            }
        }

        private ObservableCollection<MultiPowerOfAttorneyDetail> _multiPowerOfAttorneyDetails =
            new ObservableCollection<MultiPowerOfAttorneyDetail>();
        public ObservableCollection<MultiPowerOfAttorneyDetail> MultiPowerOfAttorneyDetails
        {
            get => _multiPowerOfAttorneyDetails;
            set
            {
                _multiPowerOfAttorneyDetails = value;
                OnPropertyChanged();
            }
        }

        private MultiPowerOfAttorneyDetail _newMultiPowerOfAttorney =
            new MultiPowerOfAttorneyDetail();
        public MultiPowerOfAttorneyDetail NewMultiPowerOfAttorney
        {
            get => _newMultiPowerOfAttorney;
            set
            {
                _newMultiPowerOfAttorney = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddMultiPowerOfAttorneyCommand { get; }
        public ICommand AddPowerOfAttorneyMultipleCommand { get; }

        public AccountAddPowerOfAttorneyVM()
        {
            LoadPoaRoleInfos();
            AddMultiPowerOfAttorneyCommand = new RelayCommand(AddMultiPowerOfAttorney);
            AddPowerOfAttorneyMultipleCommand = new RelayCommand(AddPowerOfAttorneyMultiple);
        }

        private void AddMultiPowerOfAttorney(object parameter)
        {
            MultiPowerOfAttorneyDetails.Add(NewMultiPowerOfAttorney);
            NewMultiPowerOfAttorney = new MultiPowerOfAttorneyDetail();
        }

        public Action<string> CloseAndLoadAccountAction { get; set; }

        // Reference to MainWindowVM
        private MainWindowVM mainWindowViewModel;

        // Property to hold reference to MainWindowVM
        public MainWindowVM MainWindowViewModel
        {
            get { return mainWindowViewModel; }
            set
            {
                mainWindowViewModel = value;
                OnPropertyChanged(nameof(MainWindowViewModel));
            }
        }

        private void AddPowerOfAttorneyMultiple(object parameter)
        {
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            if (MultiPowerOfAttorneyDetails != null)
            {
                AccountAddPowerOfAttorneyModel multiPowerOfAttorneyService =
                    new AccountAddPowerOfAttorneyModel();
                multiPowerOfAttorneyService.AddPowerOfAttorney(
                    MultiPowerOfAttorneyDetails,
                    accountNumber
                );
                MultiPowerOfAttorneyDetails.Clear();

                CloseAndLoadAccountAction?.Invoke(accountNumber);
            }
            else
            {
                MessageBox.Show("No item selected");
            }
        }

        public void LoadPoaRoleInfos()
        {
            var poaRoles = AddAccountService.LookUpPoaRole();

            PoaInfos = new ObservableCollection<AddAccountService.PoaRoleInfo>(poaRoles);
        }
    }
}
