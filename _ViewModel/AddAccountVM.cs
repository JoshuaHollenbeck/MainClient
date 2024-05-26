using System;
using MainClient.Utilities;
using MainClient.Services;
using System.Windows.Input;
using System.Windows;
using MainClient._Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace MainClient._ViewModel
{
    class AddAccountVM : ViewModelBase
    {
        private AddAccountModel _selectedInitialContactMethod;
        public AddAccountModel SelectedInitialContactMethod
        {
            get => _selectedInitialContactMethod;
            set
            {
                _selectedInitialContactMethod = value;
                OnPropertyChanged(nameof(SelectedInitialContactMethod));
            }
        }

        private AddAccountModel _selectedAcctType;
        public AddAccountModel SelectedAcctType
        {
            get => _selectedAcctType;
            set
            {
                _selectedAcctType = value;
                OnPropertyChanged(nameof(SelectedAcctType));
            }
        }

        private AddAccountModel _selectedAcctFunding;
        public AddAccountModel SelectedAcctFunding
        {
            get => _selectedAcctFunding;
            set
            {
                _selectedAcctFunding = value;
                OnPropertyChanged(nameof(SelectedAcctFunding));
            }
        }

        private AddAccountModel _selectedAcctPurpose;
        public AddAccountModel SelectedAcctPurpose
        {
            get => _selectedAcctPurpose;
            set
            {
                _selectedAcctPurpose = value;
                OnPropertyChanged(nameof(SelectedAcctPurpose));
            }
        }

        private AddAccountModel _selectedAcctActivity;
        public AddAccountModel SelectedAcctActivity
        {
            get => _selectedAcctActivity;
            set
            {
                _selectedAcctActivity = value;
                OnPropertyChanged(nameof(SelectedAcctActivity));
            }
        }

        private AddAccountModel _selectedAcctObjective;
        public AddAccountModel SelectedAcctObjective
        {
            get => _selectedAcctObjective;
            set
            {
                _selectedAcctObjective = value;
                OnPropertyChanged(nameof(SelectedAcctObjective));
            }
        }

        private bool _selectedContactSame;
        public bool SelectedContactSame
        {
            get => _selectedContactSame;
            set
            {
                _selectedContactSame = value;
                OnPropertyChanged(nameof(SelectedContactSame));
            }
        }

        private string _selectedContactName;
        public string SelectedContactName
        {
            get => _selectedContactName;
            set
            {
                _selectedContactName = value;
                OnPropertyChanged(nameof(SelectedContactName));
            }
        }

        private string _selectedContactAddress;
        public string SelectedContactAddress
        {
            get => _selectedContactAddress;
            set
            {
                _selectedContactAddress = value;
                OnPropertyChanged(nameof(SelectedContactAddress));
            }
        }

        private string _selectedContactAddress2;
        public string SelectedContactAddress2
        {
            get => _selectedContactAddress2;
            set
            {
                _selectedContactAddress2 = value;
                OnPropertyChanged(nameof(SelectedContactAddress2));
            }
        }

        private ObservableCollection<AddAccountModel> _initialContactResults =
            new ObservableCollection<AddAccountModel>();
        public ObservableCollection<AddAccountModel> InitialContactResults
        {
            get => _initialContactResults;
            set
            {
                if (_initialContactResults != value)
                {
                    _initialContactResults = value;
                    OnPropertyChanged(nameof(InitialContactResults));
                }
            }
        }

        private ObservableCollection<AddAccountModel> _acctTypeResults =
            new ObservableCollection<AddAccountModel>();
        public ObservableCollection<AddAccountModel> AcctTypeResults
        {
            get => _acctTypeResults;
            set
            {
                if (_acctTypeResults != value)
                {
                    _acctTypeResults = value;
                    OnPropertyChanged(nameof(AcctTypeResults));
                }
            }
        }

        private ObservableCollection<AddAccountModel> _acctObjectiveResults =
            new ObservableCollection<AddAccountModel>();
        public ObservableCollection<AddAccountModel> AcctObjectiveResults
        {
            get => _acctObjectiveResults;
            set
            {
                if (_acctObjectiveResults != value)
                {
                    _acctObjectiveResults = value;
                    OnPropertyChanged(nameof(AcctObjectiveResults));
                }
            }
        }

        private ObservableCollection<AddAccountModel> _acctFundingResults =
            new ObservableCollection<AddAccountModel>();
        public ObservableCollection<AddAccountModel> AcctFundingResults
        {
            get => _acctFundingResults;
            set
            {
                if (_acctFundingResults != value)
                {
                    _acctFundingResults = value;
                    OnPropertyChanged(nameof(AcctFundingResults));
                }
            }
        }

        private ObservableCollection<AddAccountModel> _acctPurposeResults =
            new ObservableCollection<AddAccountModel>();
        public ObservableCollection<AddAccountModel> AcctPurposeResults
        {
            get => _acctPurposeResults;
            set
            {
                if (_acctPurposeResults != value)
                {
                    _acctPurposeResults = value;
                    OnPropertyChanged(nameof(AcctPurposeResults));
                }
            }
        }

        private ObservableCollection<AddAccountModel> _acctActivityResults =
            new ObservableCollection<AddAccountModel>();
        public ObservableCollection<AddAccountModel> AcctActivityResults
        {
            get => _acctActivityResults;
            set
            {
                if (_acctActivityResults != value)
                {
                    _acctActivityResults = value;
                    OnPropertyChanged(nameof(AcctActivityResults));
                }
            }
        }

        private ObservableCollection<LocationService.LocationInfo> _locationResults =
            new ObservableCollection<LocationService.LocationInfo>();
        public ObservableCollection<LocationService.LocationInfo> LocationResults
        {
            get => _locationResults;
            set
            {
                if (_locationResults != value)
                {
                    _locationResults = value;
                    OnPropertyChanged(nameof(LocationResults));
                }
            }
        }

        // Selected client location.
        private LocationService.LocationInfo _selectedClientLocation;
        public LocationService.LocationInfo SelectedClientLocation
        {
            get { return _selectedClientLocation; }
            set
            {
                _selectedClientLocation = value;
                OnPropertyChanged(nameof(SelectedClientLocation));
            }
        }

        private string _selectedContactPostalCode;
        public string SelectedContactPostalCode
        {
            get => _selectedContactPostalCode;
            set
            {
                if (_selectedContactPostalCode != value)
                {
                    _selectedContactPostalCode = value.TrimStart(new Char[] { '0' });
                    OnPropertyChanged(nameof(SelectedContactPostalCode));
                    UpdateLocationInformation(_selectedContactPostalCode);
                }
            }
        }

        private string _selectedCity;
        public string SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                if (_selectedCity != value)
                {
                    _selectedCity = value;
                    OnPropertyChanged(nameof(SelectedCity));
                }
            }
        }

        private string _selectedState;
        public string SelectedState
        {
            get { return _selectedState; }
            set
            {
                if (_selectedState != value)
                {
                    _selectedState = value;
                    OnPropertyChanged(nameof(SelectedState));
                }
            }
        }

        private string _selectedCountry;
        public string SelectedCountry
        {
            get { return _selectedCountry; }
            set
            {
                if (_selectedCountry != value)
                {
                    _selectedCountry = value;
                    OnPropertyChanged(nameof(SelectedCountry));
                }
            }
        }

        public ICommand AddAccountCommand { get; set; }
        public AddAccountVM()
        {
            FetchAddAcctDetails();
            AddAccountCommand = new RelayCommand(ExecuteAddAccount);
        }

        public Action<string> CloseAndLoadAccountAction { get; set; }
        public ICommand AddNewAccountCommand { get; set; }
        private void ExecuteAddAccount(object parameter)
        {
            string repId = RepIdService.Instance.RepId;
            string acctId = AccountNumService.Instance.SelectedAccountNumber;

            if (SelectedInitialContactMethod != null)
            {
                try
                {
                    AddAccountModel addAccountModel = new AddAccountModel();

                    addAccountModel.AddAcct(
                        acctId,
                        repId,
                        SelectedInitialContactMethod?.InitialContactMethod,
                        SelectedAcctType?.AcctType,
                        SelectedAcctObjective?.AcctObjective,
                        SelectedAcctFunding?.AcctFunding,
                        SelectedAcctPurpose?.AcctPurpose,
                        SelectedAcctActivity?.AcctActivity,
                        SelectedContactSame,
                        SelectedContactName,
                        SelectedContactAddress,
                        SelectedContactAddress2,
                        SelectedContactPostalCode
                    );                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Failed to add new client: {ex.Message}",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }

                string newAcctId = AccountNumService.Instance.SelectedAccountNumber;

                CloseAndLoadAccountAction?.Invoke(newAcctId);
            }
            else
            {
                MessageBox.Show("No item selected");
            }
        }

        private void FetchAddAcctDetails()
        {
            var initialContactList = AddAccountService.LookUpInitialContactMethod();
            if (initialContactList != null)
            {
                _initialContactResults.Clear();
                foreach (var intialContactInfo in initialContactList)
                {
                    var intialContactModel = new AddAccountModel
                    {
                        InitialContactMethod = intialContactInfo.InitialContact,
                    };
                    _initialContactResults.Add(intialContactModel);
                }
            }

            if (_initialContactResults.Any())
            {
                SelectedInitialContactMethod = _initialContactResults.First();
            }

            var acctTypeList = AddAccountService.LookUpAcctType();
            if (acctTypeList != null)
            {
                _acctTypeResults.Clear();
                foreach (var acctTypeInfo in acctTypeList)
                {
                    var acctTypeModel = new AddAccountModel
                    {
                        AcctType = acctTypeInfo.AcctType
                    };
                    _acctTypeResults.Add(acctTypeModel);
                }
            }

            if (_acctTypeResults.Any())
            {
                SelectedAcctType = _acctTypeResults.First();
            }

            var acctObjectiveList = AddAccountService.LookUpAcctObjective();
            if (acctObjectiveList != null)
            {
                _acctObjectiveResults.Clear();
                foreach (var acctObjectiveInfo in acctObjectiveList)
                {
                    var acctObjectiveModel = new AddAccountModel
                    {
                        AcctObjective = acctObjectiveInfo.AcctObjective
                    };
                    _acctObjectiveResults.Add(acctObjectiveModel);
                }
            }

            if (_acctObjectiveResults.Any())
            {
                SelectedAcctObjective = _acctObjectiveResults.First();
            }

            var acctFundingList = AddAccountService.LookUpAcctFunding();
            if (acctFundingList != null)
            {
                _acctFundingResults.Clear();
                foreach (var acctFundingInfo in acctFundingList)
                {
                    var acctFundingModel = new AddAccountModel
                    {
                        AcctFunding = acctFundingInfo.AcctFunding
                    };
                    _acctFundingResults.Add(acctFundingModel);
                }
            }

            if (_acctFundingResults.Any())
            {
                SelectedAcctFunding = _acctFundingResults.First();
            }

            var acctPurposeList = AddAccountService.LookUpAcctPurpose();
            if (acctPurposeList != null)
            {
                _acctPurposeResults.Clear();
                foreach (var acctPurposeInfo in acctPurposeList)
                {
                    var acctPurposeModel = new AddAccountModel
                    {
                        AcctPurpose = acctPurposeInfo.AcctPurpose
                    };
                    _acctPurposeResults.Add(acctPurposeModel);
                }
            }

            if (_acctPurposeResults.Any())
            {
                SelectedAcctPurpose = _acctPurposeResults.First();
            }

            var acctActivityList = AddAccountService.LookUpAcctActivity();
            if (acctActivityList != null)
            {
                _acctActivityResults.Clear();
                foreach (var acctActivityInfo in acctActivityList)
                {
                    var acctActivityModel = new AddAccountModel
                    {
                        AcctActivity = acctActivityInfo.AcctActivity
                    };
                    _acctActivityResults.Add(acctActivityModel);
                }
            }

            if (_acctActivityResults.Any())
            {
                SelectedAcctActivity = _acctActivityResults.First();
            }
        }

        private void UpdateLocationInformation(string selectedContactPostalCode)
        {
            if (!string.IsNullOrEmpty(selectedContactPostalCode))
            {
                var locationInfo = LocationService.LookUpLocation(selectedContactPostalCode).FirstOrDefault();

                if (locationInfo != null)
                {
                    SelectedClientLocation = locationInfo;
                    SelectedCity = locationInfo.CityName;
                    SelectedState = locationInfo.StateName;
                    SelectedCountry = locationInfo.CountryName;
                }
            }
        }
    }
}
