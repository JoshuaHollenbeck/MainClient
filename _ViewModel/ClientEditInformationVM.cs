using System;
using System.Windows;
using MainClient._Model;
using MainClient.Utilities;
using System.Collections.ObjectModel;
using MainClient.Services;
using System.Windows.Input;
using System.Linq;
using System.Collections.Generic;

namespace MainClient._ViewModel
{
    class ClientEditInformationVM : ViewModelBase
    {
        public ObservableCollection<string> StateNames { get; private set; } =
            new ObservableCollection<string>();

        public ObservableCollection<string> IdNames { get; private set; } =
            new ObservableCollection<string>();

        private ObservableCollection<ClientEditInformationModel> _clientInformationResults =
            new ObservableCollection<ClientEditInformationModel>();
        public ObservableCollection<ClientEditInformationModel> CientInformationResults
        {
            get => _clientInformationResults;
            set
            {
                if (_clientInformationResults != value)
                {
                    _clientInformationResults = value;
                    OnPropertyChanged(nameof(CientInformationResults));
                }
            }
        }

        public ICommand UpdateClientInformationCommand { get; }

        public ClientEditInformationVM()
        {
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            UpdateClientInformationCommand = new RelayCommand(ExecuteUpdateClientInformation);
            FetchInformationDetails(accountNumber);

            Months = new ObservableCollection<string>(Enumerable.Range(1, 12).Select(i => i.ToString("00")));
            Years = new ObservableCollection<string>(Enumerable.Range(DateTime.Now.Year, 20).Select(i => i.ToString()));

            FetchStateNames();
            FetchIdTypes();
            OnPropertyChanged(String.Empty);
        }

        public Action<string> CloseAndLoadAccountAction { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? VoiceAuth { get; set; }
        public bool? DoNotCall { get; set; }
        public bool? ShareAffiliates { get; set; }
        public string IdType { get; set; }
        public string IdStateName { get; set; }
        public string IdNum { get; set; }
        public string IdExp { get; set; }
        public string MothersMaiden { get; set; }
        public string CountryName { get; set; }

        private string _newFirstName;
        public string NewFirstName
        {
            get => _newFirstName;
            set
            {
                _newFirstName = value;
                OnPropertyChanged();
            }
        }

        private string _newMiddleName;
        public string NewMiddleName
        {
            get => _newMiddleName;
            set
            {
                _newMiddleName = value;
                OnPropertyChanged();
            }
        }

        private string _newLastName;
        public string NewLastName
        {
            get => _newLastName;
            set
            {
                _newLastName = value;
                OnPropertyChanged();
            }
        }

        private string _newSuffix;
        public string NewSuffix
        {
            get => _newSuffix;
            set
            {
                _newSuffix = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _newDateOfBirth;
        public DateTime? NewDateOfBirth
        {
            get => _newDateOfBirth;
            set
            {
                _newDateOfBirth = value;
                OnPropertyChanged();
            }
        }

        private bool? _newVoiceAuth;
        public bool? NewVoiceAuth
        {
            get => _newVoiceAuth;
            set
            {
                _newVoiceAuth = value;
                OnPropertyChanged();
            }
        }

        private bool? _newDoNotCall;
        public bool? NewDoNotCall
        {
            get => _newDoNotCall;
            set
            {
                _newDoNotCall = value;
                OnPropertyChanged();
            }
        }

        private bool? _newShareAffiliates;
        public bool? NewShareAffiliates
        {
            get => _newShareAffiliates;
            set
            {
                _newShareAffiliates = value;
                OnPropertyChanged();
            }
        }

        private string _newIdNum;
        public string NewIdNum
        {
            get => _newIdNum;
            set
            {
                _newIdNum = value;
                OnPropertyChanged();
            }
        }

        private string _newIdType;
        public string NewIdType
        {
            get => _newIdType;
            set
            {
                _newIdType = value;
                OnPropertyChanged();
            }
        }

        private string _newIdStateName;
        public string NewIdStateName
        {
            get => _newIdStateName;
            set
            {
                _newIdStateName = value;
                OnPropertyChanged(NewIdStateName);
            }
        }

        private string _newIdExp;
        public string NewIdExp
        {
            get => _newIdExp;
            set
            {
                if (_newIdExp != value)
                {
                    _newIdExp = value;
                    OnPropertyChanged();

                    var parts = _newIdExp.Split('/');
                    if (parts.Length == 2)
                    {
                        SelectedMonth = parts[0];
                        SelectedYear = parts[1];
                    }
                }
            }
        }

        private string _newMothersMaiden;
        public string NewMothersMaiden
        {
            get => _newMothersMaiden;
            set
            {
                _newMothersMaiden = value;
                OnPropertyChanged();
            }
        }

        private void ExecuteUpdateClientInformation(object parameter)
        {
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            NewIdExp = $"{SelectedMonth}/{SelectedYear}";
            
            if (accountNumber != null)
            {
                ClientEditInformationModel editContact = new ClientEditInformationModel();
                editContact.EditClientInfo(
                    accountNumber,
                    NewFirstName,
                    NewMiddleName,
                    NewLastName,
                    NewSuffix,
                    NewDateOfBirth,
                    NewVoiceAuth,
                    NewDoNotCall,
                    NewShareAffiliates,
                    NewIdType,
                    NewIdStateName,
                    NewIdNum,
                    NewIdExp,
                    NewMothersMaiden
                );

                CloseAndLoadAccountAction?.Invoke(accountNumber);
            }
            else
            {
                MessageBox.Show("No item selected");
            }
        }

        private void FetchInformationDetails(string accountNumber)
        {
            var clientInformationInfoList = ClientEditInformationModel
                .GetClientInfoByAcctNum(accountNumber)
                .FirstOrDefault();
            if (clientInformationInfoList != null)
            {
                FirstName = clientInformationInfoList.FirstName;
                MiddleName = clientInformationInfoList.MiddleName;
                LastName = clientInformationInfoList.LastName;
                Suffix = clientInformationInfoList.Suffix;
                DateOfBirth = clientInformationInfoList.DateOfBirth;
                VoiceAuth = clientInformationInfoList.VoiceAuth;
                DoNotCall = clientInformationInfoList.DoNotCall;
                ShareAffiliates = clientInformationInfoList.ShareAffiliates;
                IdType = clientInformationInfoList.IdType;
                IdStateName = clientInformationInfoList.IdStateName;
                IdNum = clientInformationInfoList.IdNum;
                IdExp = clientInformationInfoList.IdExp;
                MothersMaiden = clientInformationInfoList.MothersMaiden;
                CountryName = clientInformationInfoList.CountryName;

                // Initialize the new properties with existing data
                NewFirstName = FirstName;
                NewMiddleName = MiddleName;
                NewLastName = LastName;
                NewSuffix = Suffix;
                NewDateOfBirth = DateOfBirth;
                NewVoiceAuth = VoiceAuth;
                NewDoNotCall = DoNotCall;
                NewShareAffiliates = ShareAffiliates;
                NewIdType = IdType;
                NewIdStateName = IdStateName;
                NewIdNum = IdNum;
                NewIdExp = IdExp;
                NewMothersMaiden = MothersMaiden;

                OnPropertyChanged("");

                ParseIdExpToMonthAndYear();
            }

            OnPropertyChanged(String.Empty);
        }

        private void FetchStateNames()
        {
            var stateInfos = MainClient.Services.LocationService.LookUpState(CountryName);
            foreach (var stateInfo in stateInfos)
            {
                StateNames.Add(stateInfo.StateName);
            }
        }

        private void FetchIdTypes()
        {
            var idInfos = MainClient.Services.IdService.LookUpIdType();
            foreach (var idInfo in idInfos)
            {
                IdNames.Add(idInfo.IdName);
            }
        }

        // Properties for months and years
        public ObservableCollection<string> Months { get; private set; }
        public ObservableCollection<string> Years { get; private set; }

        private string _selectedMonth;
        public string SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                if (_selectedMonth != value)
                {
                    _selectedMonth = value;
                    OnPropertyChanged(nameof(SelectedMonth));
                    UpdateNewIdExpFromMonthAndYear();
                }
            }
        }

        private string _selectedYear;
       public string SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;
                    OnPropertyChanged(nameof(SelectedYear));
                    UpdateNewIdExpFromMonthAndYear();
                }
            }
        }

        private void ParseIdExpToMonthAndYear()
        {
            if (!string.IsNullOrEmpty(NewIdExp))
            {
                var parts = NewIdExp.Split('/');
                if (parts.Length == 2)
                {
                    SelectedMonth = parts[0].PadLeft(2, '0');
                    SelectedYear = parts[1];
                }
            }
        }

        private void UpdateNewIdExpFromMonthAndYear()
        {
            if (!string.IsNullOrEmpty(SelectedMonth) && !string.IsNullOrEmpty(SelectedYear))
            {
                NewIdExp = $"{SelectedMonth}/{SelectedYear}";
                OnPropertyChanged(nameof(NewIdExp));
            }
        }
    }
}
