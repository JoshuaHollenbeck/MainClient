using MainClient.Utilities;
using MainClient.Services;
using System;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;
using MainClient._Model;

namespace MainClient._ViewModel
{
    public class MultiBeneficiaryDetail
    {
        public string MultiBeneFirstName { get; set; }
        public string MultiBeneLastName { get; set; }
        public string MultiBeneTaxId { get; set; }
        public string MultiRelationshipType { get; set; }
        public decimal MultiBenePortion { get; set; }
    }

    class AccountAddBeneficiaryVM : ViewModelBase
    {
        private ObservableCollection<AddAccountService.RelationshipTypeInfo> _relationshipInfos;
        public ObservableCollection<AddAccountService.RelationshipTypeInfo> RelationshipInfos
        {
            get => _relationshipInfos;
            set
            {
                _relationshipInfos = value;
                OnPropertyChanged(nameof(RelationshipInfos));
            }
        }

        private AddAccountService.RelationshipTypeInfo _selectedRelationshipTypeInfo;
        public AddAccountService.RelationshipTypeInfo SelectedRelationshipTypeInfo
        {
            get => _selectedRelationshipTypeInfo;
            set
            {
                _selectedRelationshipTypeInfo = value;
                OnPropertyChanged(nameof(SelectedRelationshipTypeInfo));
                NewMultiBeneficiary.MultiRelationshipType = value?.RelationshipType;
            }
        }

        private ObservableCollection<MultiBeneficiaryDetail> _multiBeneficiaryDetails =
            new ObservableCollection<MultiBeneficiaryDetail>();
        public ObservableCollection<MultiBeneficiaryDetail> MultiBeneficiaryDetails
        {
            get => _multiBeneficiaryDetails;
            set
            {
                _multiBeneficiaryDetails = value;
                OnPropertyChanged();
            }
        }

        private MultiBeneficiaryDetail _newMultiBeneficiary = new MultiBeneficiaryDetail();
        public MultiBeneficiaryDetail NewMultiBeneficiary
        {
            get => _newMultiBeneficiary;
            set
            {
                _newMultiBeneficiary = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddMultiBeneficiaryCommand { get; }
        public ICommand AddBeneficiaryMultipleCommand { get; }

        public AccountAddBeneficiaryVM()
        {
            LoadRelationshipTypeInfos();
            AddMultiBeneficiaryCommand = new RelayCommand(AddMultiBeneficiary);
            AddBeneficiaryMultipleCommand = new RelayCommand(AddBeneficiaryMultiple);
        }

        private void AddMultiBeneficiary(object parameter)
        {
            MultiBeneficiaryDetails.Add(NewMultiBeneficiary);
            NewMultiBeneficiary = new MultiBeneficiaryDetail();
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

        private void AddBeneficiaryMultiple(object parameter)
        {
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            if (MultiBeneficiaryDetails != null)
            {
                AccountAddBeneficiaryModel multiBeneficiaryService = new AccountAddBeneficiaryModel();
                multiBeneficiaryService.AddBeneficiary(MultiBeneficiaryDetails, accountNumber);
                MultiBeneficiaryDetails.Clear();

                CloseAndLoadAccountAction?.Invoke(accountNumber);

            }
            else
            {
                MessageBox.Show("No item selected");
            }
        }

        public void LoadRelationshipTypeInfos()
        {
            var relationshipTypes = AddAccountService.LookUpRelationshipType();

            RelationshipInfos = new ObservableCollection<AddAccountService.RelationshipTypeInfo>(relationshipTypes);

        }
    }
}
