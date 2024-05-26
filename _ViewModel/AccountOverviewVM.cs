using System;
using System.Windows;
using MainClient._Model;
using MainClient.Utilities;
using MainClient.Services;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MainClient._ViewModel
{
    class AccountOverviewVM : ViewModelBase
    {
        private readonly IDialogService _dialogService;

        private AccountOverviewModel _accountDetails;

        // Properties to bind to the view
        public string RegistrationName => _accountDetails?.RegistrationName;
        public string InitialContactMethod => _accountDetails?.InitialContactMethod;
        public string AccountType => _accountDetails?.AccountType;
        public string ClientFirstName => _accountDetails?.ClientFirstName;
        public string ClientMiddleName => _accountDetails?.ClientMiddleName;
        public string ClientLastName => _accountDetails?.ClientLastName;
        public string ClientSuffix => _accountDetails?.ClientSuffix;
        public string ClientAddress => _accountDetails?.ClientAddress;
        public string ClientAddress2 => _accountDetails?.ClientAddress2;
        public string ClientCity => _accountDetails?.ClientCity;
        public string ClientState => _accountDetails?.ClientState;
        public string ClientZip => _accountDetails?.ClientZip;
        public string ClientCountry => _accountDetails?.ClientCountry;
        public string PrimaryContactName => _accountDetails?.PrimaryContactName;
        public string PrimaryContactAddress => _accountDetails?.PrimaryContactAddress;
        public string PrimaryContactAddress2 => _accountDetails?.PrimaryContactAddress2;
        public string PrimaryContactCity => _accountDetails?.PrimaryContactCity;
        public string PrimaryContactState => _accountDetails?.PrimaryContactState;
        public string PrimaryContactZip => _accountDetails?.PrimaryContactZip;
        public string RepID => _accountDetails?.RepID;
        public DateTime? EstablishedDate => _accountDetails?.EstablishedDate;
        public bool? AccountStatus => _accountDetails?.AccountStatus;
        public string JurisdictionCountry => _accountDetails?.JurisdictionCountry;
        public string JurisdictionState => _accountDetails?.JurisdictionState;
        public string AccountPassword => _accountDetails?.AccountPassword;
        public string BranchLocation => _accountDetails?.BranchLocation;
        public string CustTaxId => _accountDetails?.CustTaxId;
        public int? AtmLimit => _accountDetails?.AtmLimit;
        public int? AchLimit => _accountDetails?.AchLimit;
        public int? WireLimit => _accountDetails?.WireLimit;
        public string EmailAddress => _accountDetails?.EmailAddress;
        public bool? WebBanking => _accountDetails?.WebBanking;
        public bool? MobileBanking => _accountDetails?.MobileBanking;
        public bool? TwoFactor => _accountDetails?.TwoFactor;
        public bool? Biometrics => _accountDetails?.Biometrics;

        // TODO Account objective binding needs fixed
        public byte? AcctObjective
        {
            get => _accountDetails?.AcctObjective;
            set
            {
                if (_accountDetails?.AcctObjective != value)
                {
                    _accountDetails.AcctObjective = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(AcctObjectiveVisibility1));
                    OnPropertyChanged(nameof(AcctObjectiveVisibility2));
                    OnPropertyChanged(nameof(AcctObjectiveVisibility3));
                    OnPropertyChanged(nameof(AcctObjectiveVisibility4));
                    OnPropertyChanged(nameof(AcctObjectiveVisibility5));
                }
            }
        }

        // TODO Account funding binding needs fixed
        public byte? AcctFunding
        {
            get => _accountDetails?.AcctFunding;
            set
            {
                if (_accountDetails?.AcctFunding != value)
                {
                    _accountDetails.AcctFunding = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(AcctFundingVisibility1));
                    OnPropertyChanged(nameof(AcctFundingVisibility2));
                    OnPropertyChanged(nameof(AcctFundingVisibility3));
                    OnPropertyChanged(nameof(AcctFundingVisibility4));
                    OnPropertyChanged(nameof(AcctFundingVisibility5));
                    OnPropertyChanged(nameof(AcctFundingVisibility6));
                    OnPropertyChanged(nameof(AcctFundingVisibility7));
                }
            }
        }

        // TODO Account purpose binding needs fixed
        public byte? AcctPurpose
        {
            get => _accountDetails?.AcctPurpose;
            set
            {
                if (_accountDetails?.AcctPurpose != value)
                {
                    _accountDetails.AcctPurpose = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(AcctPurposeVisibility1));
                    OnPropertyChanged(nameof(AcctPurposeVisibility2));
                    OnPropertyChanged(nameof(AcctPurposeVisibility3));
                    OnPropertyChanged(nameof(AcctPurposeVisibility4));
                    OnPropertyChanged(nameof(AcctPurposeVisibility5));
                    OnPropertyChanged(nameof(AcctPurposeVisibility6));
                    OnPropertyChanged(nameof(AcctPurposeVisibility7));
                }
            }
        }

        // TODO Account activity binding needs fixed
        public byte? AcctActivity
        {
            get => _accountDetails?.AcctActivity;
            set
            {
                if (_accountDetails?.AcctActivity != value)
                {
                    _accountDetails.AcctActivity = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(AcctActivityVisibility1));
                    OnPropertyChanged(nameof(AcctActivityVisibility2));
                    OnPropertyChanged(nameof(AcctActivityVisibility3));
                    OnPropertyChanged(nameof(AcctActivityVisibility4));
                }
            }
        }

        public Visibility AcctObjectiveVisibility1 => ConvertToVisibility(_accountDetails?.AcctObjective, 1);
        public Visibility AcctObjectiveVisibility2 => ConvertToVisibility(_accountDetails?.AcctObjective, 2);
        public Visibility AcctObjectiveVisibility3 => ConvertToVisibility(_accountDetails?.AcctObjective, 3);
        public Visibility AcctObjectiveVisibility4 => ConvertToVisibility(_accountDetails?.AcctObjective, 4);
        public Visibility AcctObjectiveVisibility5 => ConvertToVisibility(_accountDetails?.AcctObjective, 5);

        public Visibility AcctPurposeVisibility1 => ConvertToVisibility(_accountDetails?.AcctPurpose, 1);
        public Visibility AcctPurposeVisibility2 => ConvertToVisibility(_accountDetails?.AcctPurpose, 2);
        public Visibility AcctPurposeVisibility3 => ConvertToVisibility(_accountDetails?.AcctPurpose, 3);
        public Visibility AcctPurposeVisibility4 => ConvertToVisibility(_accountDetails?.AcctPurpose, 4);
        public Visibility AcctPurposeVisibility5 => ConvertToVisibility(_accountDetails?.AcctPurpose, 5);
        public Visibility AcctPurposeVisibility6 => ConvertToVisibility(_accountDetails?.AcctPurpose, 6);
        public Visibility AcctPurposeVisibility7 => ConvertToVisibility(_accountDetails?.AcctPurpose, 7);

        public Visibility AcctFundingVisibility1 => ConvertToVisibility(_accountDetails?.AcctFunding, 1);
        public Visibility AcctFundingVisibility2 => ConvertToVisibility(_accountDetails?.AcctFunding, 2);
        public Visibility AcctFundingVisibility3 => ConvertToVisibility(_accountDetails?.AcctFunding, 3);
        public Visibility AcctFundingVisibility4 => ConvertToVisibility(_accountDetails?.AcctFunding, 4);
        public Visibility AcctFundingVisibility5 => ConvertToVisibility(_accountDetails?.AcctFunding, 5);
        public Visibility AcctFundingVisibility6 => ConvertToVisibility(_accountDetails?.AcctFunding, 6);
        public Visibility AcctFundingVisibility7 => ConvertToVisibility(_accountDetails?.AcctFunding, 7);

        public Visibility AcctActivityVisibility1 => ConvertToVisibility(_accountDetails?.AcctActivity, 1);
        public Visibility AcctActivityVisibility2 => ConvertToVisibility(_accountDetails?.AcctActivity, 2);
        public Visibility AcctActivityVisibility3 => ConvertToVisibility(_accountDetails?.AcctActivity, 3);
        public Visibility AcctActivityVisibility4 => ConvertToVisibility(_accountDetails?.AcctActivity, 4);

        private Visibility ConvertToVisibility(byte? value, byte target)
        {
            return value == target ? Visibility.Visible : Visibility.Collapsed;
        }

        private ObservableCollection<AccountOverviewModel> _accountHolderResults = new ObservableCollection<AccountOverviewModel>();
        public ObservableCollection<AccountOverviewModel> AccountHolderResults
        {
            get  => _accountHolderResults;
            set
            {
                if (_accountHolderResults != value)
                {
                    _accountHolderResults = value;
                    OnPropertyChanged(nameof(AccountHolderResults));
                }
            }
        }

        private ObservableCollection<AccountOverviewModel> _accountBeneResults = new ObservableCollection<AccountOverviewModel>();
        public ObservableCollection<AccountOverviewModel> AccountBeneResults
        {
            get  => _accountBeneResults;
            set
            {
                if (_accountBeneResults != value)
                {
                    _accountBeneResults = value;
                    OnPropertyChanged(nameof(AccountBeneResults));
                }
            }
        }

        private ObservableCollection<AccountOverviewModel> _accountPOAResults = new ObservableCollection<AccountOverviewModel>();
        public ObservableCollection<AccountOverviewModel> AccountPOAResults
        {
            get  => _accountPOAResults;
            set
            {
                if (_accountPOAResults != value)
                {
                    _accountPOAResults = value;
                    OnPropertyChanged(nameof(AccountPOAResults));
                }
            }
        }

        public ICommand AddBeneficiaryCommand  { get; private set; }
        public ICommand EditBeneficiaryCommand { get; private set; }
        public ICommand AddPowerOfAttorneyCommand { get; private set; }        
        public ICommand EditPowerOfAttorneyCommand { get; private set; }
        public ICommand EditLimitCommand { get; private set; }
        public ICommand EditContactCommand { get; private set; }
        public ICommand EditPasswordCommand { get; private set; }


        public AccountOverviewVM(
            string accountNumber,
            IDialogService dialogService,
            ICommand[] commands) : base()
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));

            AddBeneficiaryCommand = commands[0];
            EditBeneficiaryCommand = commands[1];
            AddPowerOfAttorneyCommand = commands[2];
            EditPowerOfAttorneyCommand = commands[3];
            EditContactCommand = commands[4];
            EditLimitCommand = commands[5];
            EditPasswordCommand = commands[6];

            string acctNum = accountNumber;
            
            FetchAccountDetails(acctNum);
        }

        // TODO Check account funding, purpose, objective, and activies are correct
        private void FetchAccountDetails(string acctNum)
        {
            _accountDetails = AccountOverviewModel.GetAccountDetailsByAcctNum(acctNum);

            var accountHolderList = AccountOverviewModel.GetAccountHoldersByAcctNum(acctNum);
            if (accountHolderList != null)
            {
                _accountHolderResults.Clear();
                foreach(var accountHolder in accountHolderList)
                {
                    _accountHolderResults.Add(accountHolder);
                }
            }

            var accountBeneList = AccountOverviewModel.GetAccountBeneByAcctNum(acctNum);
            if (accountBeneList != null)
            {
                _accountBeneResults.Clear();
                foreach(var accountBene in accountBeneList)
                {
                    _accountBeneResults.Add(accountBene);
                }
            }

            var accountPOAList = AccountOverviewModel.GetAccountPOAByAcctNum(acctNum);
            if (accountPOAList != null)
            {
                _accountPOAResults.Clear();
                foreach(var accountPOA in accountPOAList)
                {
                    _accountPOAResults.Add(accountPOA);
                }
            }

            OnPropertyChanged(String.Empty);
        }
    }
}