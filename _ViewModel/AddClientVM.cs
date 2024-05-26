using System;
using MainClient.Utilities;
using System.Collections.ObjectModel;
using System.Linq;
using MainClient.Services;
using System.Windows.Input;
using System.Windows;
using MainClient._Model;

namespace MainClient._ViewModel
{
    // This class represents the ViewModel for adding a client.
    class AddClientVM : ViewModelBase
    {
        private string _selectedMothersMaiden;
        public string SelectedMothersMaiden
        {
            get => _selectedMothersMaiden;
            set
            {
                _selectedMothersMaiden = value;
                OnPropertyChanged(nameof(SelectedMothersMaiden));
            }
        }

        private string _selectedIdNumber;
        public string SelectedIdNumber
        {
            get => _selectedIdNumber;
            set
            {
                _selectedIdNumber = value;
                OnPropertyChanged(nameof(SelectedIdNumber));
            }
        }

        private string _selectedTaxId;
        public string SelectedTaxId
        {
            get => _selectedTaxId;
            set
            {
                _selectedTaxId = value;
                OnPropertyChanged(nameof(SelectedTaxId));
            }
        }

        private string _selectedFirstName;
        public string SelectedFirstName
        {
            get => _selectedFirstName;
            set
            {
                _selectedFirstName = value;
                OnPropertyChanged(nameof(SelectedFirstName));
            }
        }

        private string _selectedMiddleName;
        public string SelectedMiddleName
        {
            get => _selectedMiddleName;
            set
            {
                _selectedMiddleName = value;
                OnPropertyChanged(nameof(SelectedMiddleName));
            }
        }

        private string _selectedLastName;
        public string SelectedLastName
        {
            get => _selectedLastName;
            set
            {
                _selectedLastName = value;
                OnPropertyChanged(nameof(SelectedLastName));
            }
        }

        private string _selectedSuffix;
        public string SelectedSuffix
        {
            get => _selectedSuffix;
            set
            {
                _selectedSuffix = value;
                OnPropertyChanged(nameof(SelectedSuffix));
            }
        }

        private string _selectedEmail;
        public string SelectedEmail
        {
            get => _selectedEmail;
            set
            {
                _selectedEmail = value;
                OnPropertyChanged(nameof(SelectedEmail));
            }
        }

        private string _selectedPhoneHome;
        public string SelectedPhoneHome
        {
            get => _selectedPhoneHome;
            set
            {
                _selectedPhoneHome = value;
                OnPropertyChanged(nameof(SelectedPhoneHome));
            }
        }

        private string _selectedPhoneBusiness;
        public string SelectedPhoneBusiness
        {
            get => _selectedPhoneBusiness;
            set
            {
                _selectedPhoneBusiness = value;
                OnPropertyChanged(nameof(SelectedPhoneBusiness));
            }
        }

        private string _selectedAddressLine1;
        public string SelectedAddressLine1
        {
            get => _selectedAddressLine1;
            set
            {
                _selectedAddressLine1 = value;
                OnPropertyChanged(nameof(SelectedAddressLine1));
            }
        }

        private string _selectedAddressLine2;
        public string SelectedAddressLine2
        {
            get => _selectedAddressLine2;
            set
            {
                _selectedAddressLine2 = value;
                OnPropertyChanged(nameof(SelectedAddressLine2));
            }
        }

        private string _selectedEmploymentStatus;
        public string SelectedEmploymentStatus
        {
            get => _selectedEmploymentStatus;
            set
            {
                _selectedEmploymentStatus = value;
                OnPropertyChanged(nameof(SelectedEmploymentStatus));
            }
        }

        private string _selectedEmployerName;
        public string SelectedEmployerName
        {
            get => _selectedEmployerName;
            set
            {
                _selectedEmployerName = value;
                OnPropertyChanged(nameof(SelectedEmployerName));
            }
        }

        private string _selectedOccupation;
        public string SelectedOccupation
        {
            get => _selectedOccupation;
            set
            {
                _selectedOccupation = value;
                OnPropertyChanged(nameof(SelectedOccupation));
            }
        }

        // Collection of ID information results.
        private ObservableCollection<IdService.IdInfo> _idResults =
            new ObservableCollection<IdService.IdInfo>();

        public ObservableCollection<IdService.IdInfo> IdResults
        {
            get => _idResults;
            set
            {
                if (_idResults != value)
                {
                    _idResults = value;
                    OnPropertyChanged(nameof(IdResults));
                    ;
                }
            }
        }

        // Collection of country information results.
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

        // Collection of state information results.
        private ObservableCollection<LocationService.StateInfo> _idStatesResults =
            new ObservableCollection<LocationService.StateInfo>();
        public ObservableCollection<LocationService.StateInfo> IdStatesResults
        {
            get => _idStatesResults;
            set
            {
                if (_idStatesResults != value)
                {
                    _idStatesResults = value;
                    OnPropertyChanged(nameof(IdStatesResults));
                }
            }
        }

        // Selected ID type.
        private IdService.IdInfo _selectedIdType;
        public IdService.IdInfo SelectedIdType
        {
            get => _selectedIdType;
            set
            {
                if (_selectedIdType != value)
                {
                    _selectedIdType = value;
                    OnPropertyChanged(nameof(SelectedIdType));
                }
            }
        }

        // Selected ID state.
        private LocationService.StateInfo _selectedIdState;
        public LocationService.StateInfo SelectedIdState
        {
            get => _selectedIdState;
            set
            {
                if (_selectedIdState != value)
                {
                    _selectedIdState = value;
                    OnPropertyChanged(nameof(SelectedIdState));
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

        private string _selectedZipCode;
        public string SelectedZipCode
        {
            get => _selectedZipCode;
            set
            {
                if (_selectedZipCode != value)
                {
                    _selectedZipCode = value.TrimStart(new Char[] { '0' });
                    OnPropertyChanged(nameof(SelectedZipCode));
                    UpdateLocationInformation(_selectedZipCode);
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
                    if (IdStatesResults == null)
                        IdStatesResults = new ObservableCollection<LocationService.StateInfo>();
                    UpdateStatesForCountry(_selectedCountry);
                }
            }
        }

        // Collection of months.
        private ObservableCollection<string> _monthsResults;
        public ObservableCollection<string> MonthsResults
        {
            get { return _monthsResults; }
            set
            {
                _monthsResults = value;
                OnPropertyChanged(nameof(MonthsResults));
            }
        }

        // Selected month.
        private string _selectedMonth;
        public string SelectedMonth
        {
            get { return _selectedMonth; }
            set
            {
                if (_selectedMonth != value)
                {
                    _selectedMonth = value;
                    OnPropertyChanged(nameof(SelectedMonth));
                    UpdateDays();
                }
            }
        }

        // Selected ID month.
        private string _selectedIdMonth;
        public string SelectedIdMonth
        {
            get { return _selectedIdMonth; }
            set
            {
                _selectedIdMonth = value;
                OnPropertyChanged(nameof(SelectedIdMonth));
            }
        }

        // Collection of days.
        private ObservableCollection<int> _daysResults;
        public ObservableCollection<int> DaysResults
        {
            get { return _daysResults; }
            set
            {
                _daysResults = value;
                OnPropertyChanged(nameof(DaysResults));
            }
        }

        // Selected day.
        private int _selectedDay;
        public int SelectedDay
        {
            get { return _selectedDay; }
            set
            {
                _selectedDay = value;
                OnPropertyChanged(nameof(SelectedDay));
            }
        }

        // Method to generate an array of days.
        private int[] GenerateDays()
        {
            int[] days = new int[31];
            for (int i = 0; i < 31; i++)
            {
                days[i] = i + 1;
            }
            return days;
        }

        // Collection of years.
        private ObservableCollection<int> _yearsResults;
        public ObservableCollection<int> YearsResults
        {
            get { return _yearsResults; }
            set
            {
                _yearsResults = value;
                OnPropertyChanged(nameof(YearsResults));
            }
        }

        // Selected year.
        private int _selectedYear;
        public int SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;
                    OnPropertyChanged(nameof(SelectedYear));
                    UpdateDays();
                }
            }
        }

        // Method to generate an array of years.
        private int[] GenerateYears()
        {
            int currentYear = DateTime.Now.Year;
            int[] years = new int[120];
            for (int i = 0; i < 120; i++)
            {
                years[i] = currentYear - i;
            }
            return years;
        }

        // Method to generate an array of expiration years.
        private int[] GenerateExpYears()
        {
            int currentYear = DateTime.Now.Year;
            int[] expYears = new int[10];
            for (int i = 0; i < 10; i++)
            {
                expYears[i] = currentYear + i;
            }
            return expYears;
        }

        // Collection of expiration years.
        private ObservableCollection<int> _expYearsResults;
        public ObservableCollection<int> ExpYearsResults
        {
            get { return _expYearsResults; }
            set
            {
                _expYearsResults = value;
                OnPropertyChanged(nameof(ExpYearsResults));
            }
        }

        // Selected expiration year.
        private int _selectedExpYear;
        public int SelectedExpYear
        {
            get { return _selectedExpYear; }
            set
            {
                if (_selectedExpYear != value)
                {
                    _selectedExpYear = value;
                    OnPropertyChanged(nameof(SelectedExpYear));
                }
            }
        }

        // Method to update the collection of days based on the selected month.
        private void UpdateDays()
        {
            if (!string.IsNullOrEmpty(SelectedMonth))
            {
                int daysInMonth = DateTime.DaysInMonth(
                    SelectedYear,
                    MonthStringToNumber(SelectedMonth)
                );
                DaysResults = new ObservableCollection<int>(Enumerable.Range(1, daysInMonth));
            }
            else
            {
                SelectedMonth = "January";
                SelectedIdMonth = "January";
            }
        }

        // Method to convert month string to number.
        private int MonthStringToNumber(string month)
        {
            return month switch
            {
                "January" => 1,
                "February" => 2,
                "March" => 3,
                "April" => 4,
                "May" => 5,
                "June" => 6,
                "July" => 7,
                "August" => 8,
                "September" => 9,
                "October" => 10,
                "November" => 11,
                "December" => 12,
                _
                    => throw new ArgumentOutOfRangeException(
                        nameof(month),
                        $"Not expected month value: {month}"
                    )
            };
        }

        public ICommand AddClientCommand { get; set; }

        public AddClientVM()
        {
            LoadIdTypes();
            IdStatesResults = new ObservableCollection<LocationService.StateInfo>();
            AddClientCommand = new RelayCommand(ExecuteAddClient);

            // Initialize selected date values.
            SelectedYear = DateTime.Now.Year;
            SelectedExpYear = DateTime.Now.Year;
            SelectedMonth = "January";
            SelectedIdMonth = "January";
            SelectedDay = 1;

            // Initialize collections for months, days, years, and expiration years.
            MonthsResults = new ObservableCollection<string>(
                new string[]
                {
                    "January",
                    "February",
                    "March",
                    "April",
                    "May",
                    "June",
                    "July",
                    "August",
                    "September",
                    "October",
                    "November",
                    "December"
                }
            );
            UpdateDays();
            DaysResults = new ObservableCollection<int>(GenerateDays());
            YearsResults = new ObservableCollection<int>(GenerateYears());
            ExpYearsResults = new ObservableCollection<int>(GenerateExpYears());

            OnPropertyChanged(String.Empty);
        }

        // Method to load ID types.
        private void LoadIdTypes()
        {
            var idList = IdService.LookUpIdType();
            IdResults.Clear();
            foreach (var idType in idList)
            {
                IdResults.Add(idType);
            }
            SelectedIdType = IdResults.FirstOrDefault();
        }

        private void UpdateLocationInformation(string selectedZipCode)
        {
            if (!string.IsNullOrEmpty(selectedZipCode))
            {
                var locationInfo = LocationService.LookUpLocation(selectedZipCode).FirstOrDefault();

                if (locationInfo != null)
                {
                    SelectedClientLocation = locationInfo;
                    SelectedCity = locationInfo.CityName;
                    SelectedState = locationInfo.StateName;
                    SelectedCountry = locationInfo.CountryName;
                }
            }
        }

        private void UpdateStatesForCountry(string selectedCountry)
        {
            var stateInfoList = LocationService.LookUpState(selectedCountry);
            IdStatesResults.Clear();
            foreach (var state in stateInfoList)
            {
                IdStatesResults.Add(state);
            }
            SelectedIdState = IdStatesResults.FirstOrDefault();
        }

        public ICommand AddAccountNewClientCommand { get; set; }

        public Action<long> CloseAndLoadAccountAction { get; set; }

        private void ExecuteAddClient(object parameter)
        {
            string repId = RepIdService.Instance.RepId;
            string custTaxId = SelectedTaxId;
            string firstName = SelectedFirstName;
            string middleName = SelectedMiddleName;
            string lastName = SelectedLastName;
            string suffix = SelectedSuffix;
            DateTime? dateOfBirth = new DateTime(
                SelectedYear,
                MonthStringToNumber(SelectedMonth),
                SelectedDay
            );
            string custEmail = SelectedEmail;
            string custPhoneHome = SelectedPhoneHome;
            string custPhoneBusiness = SelectedPhoneBusiness;
            string custAddress = SelectedAddressLine1;
            string custAddress2 = SelectedAddressLine2;
            string custPostal = SelectedZipCode;
            string employmentStatus = SelectedEmploymentStatus;
            string employerName = SelectedEmployerName;
            string occupation = SelectedOccupation;
            string idType = SelectedIdType?.IdName;
            string idStateName = SelectedIdState?.StateName;
            string idNum = SelectedIdNumber;
            DateTime idExp = new DateTime(SelectedExpYear, MonthStringToNumber(SelectedIdMonth), 1);
            string mothersMaiden = SelectedMothersMaiden;

            if (SelectedTaxId != null)
            {
                try
                {
                    AddClientModel addClientModel = new AddClientModel();

                    addClientModel.AddClient(
                        custTaxId,
                        firstName,
                        middleName,
                        lastName,
                        suffix,
                        dateOfBirth,
                        custEmail,
                        custPhoneHome,
                        custPhoneBusiness,
                        custAddress,
                        custAddress2,
                        custPostal,
                        employmentStatus,
                        employerName,
                        occupation,
                        idType,
                        idStateName,
                        idNum,
                        idExp.ToString("MM/yyyy"),
                        mothersMaiden,
                        repId
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

                long customerId = ClientIdService.Instance.SelectedNewCustId;

                CloseAndLoadAccountAction?.Invoke(customerId);
            }
            else
            {
                MessageBox.Show("No item selected");
            }
        }
    }
}
