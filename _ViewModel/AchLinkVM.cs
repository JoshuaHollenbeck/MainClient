using System;
using System.Linq;
using MainClient._Model;
using MainClient.Utilities;
using MainClient.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;

namespace MainClient._ViewModel
{
    class AchLinkVM : ViewModelBase
    {
        private readonly IDialogService _dialogService;

        private ObservableCollection<AchLinkModel> _achBankResults =
            new ObservableCollection<AchLinkModel>();
        public ObservableCollection<AchLinkModel> AchBankResults
        {
            get => _achBankResults;
            set
            {
                if (_achBankResults != value)
                {
                    _achBankResults = value;
                    OnPropertyChanged(nameof(AchBankResults));
                }
            }
        }

        private ObservableCollection<AchLinkModel> _achBankTypeResults =
            new ObservableCollection<AchLinkModel>();
        public ObservableCollection<AchLinkModel> AchBankTypeResults
        {
            get => _achBankTypeResults;
            set
            {
                if (_achBankTypeResults != value)
                {
                    _achBankTypeResults = value;
                    OnPropertyChanged(nameof(AchBankTypeResults));
                }
            }
        }

        private ObservableCollection<AchLinkModel> _achLinkResults =
            new ObservableCollection<AchLinkModel>();
        public ObservableCollection<AchLinkModel> ACHLinkResults
        {
            get => _achLinkResults;
            set
            {
                if (_achLinkResults != value)
                {
                    _achLinkResults = value;
                    OnPropertyChanged(nameof(ACHLinkResults));
                }
            }
        }

        private ObservableCollection<AchLinkModel> _achTransactionResults =
            new ObservableCollection<AchLinkModel>();
        public ObservableCollection<AchLinkModel> ACHTransactionResults
        {
            get => _achTransactionResults;
            set
            {
                if (_achTransactionResults != value)
                {
                    _achTransactionResults = value;
                    OnPropertyChanged(nameof(ACHTransactionResults));
                }
            }
        }

        private void FetchAchDetails(string acctNum)
        {
            var bankRoutingInfoList = BankService.LookUpBankRouting();
            if (bankRoutingInfoList != null)
            {
                _achBankResults.Clear();
                foreach (var routingInfo in bankRoutingInfoList)
                {
                    var achModel = new AchLinkModel
                    {
                        AchBankName = routingInfo.BankName,
                        AchRoutingNumber = routingInfo.BankRoutingNumber
                    };
                    _achBankResults.Add(achModel);
                }
            }

            if (_achBankResults.Any())
            {
                SelectedBank = _achBankResults.First();
            }

            var bankAccountTypeList = BankService.LookUpBankAcctType();
            if (bankAccountTypeList != null)
            {
                _achBankTypeResults.Clear();
                foreach (var accountType in bankAccountTypeList)
                {
                    var achTypeModel = new AchLinkModel
                    {
                        AchBankType = accountType.BankAccountType
                    };
                    _achBankTypeResults.Add(achTypeModel);
                }
            }

            if (_achBankTypeResults.Any())
            {
                SelectedBankType = _achBankTypeResults.First();
            }

            var accountAchList = AchLinkModel.GetAcctAchLinksByAcctNum(acctNum);
            if (accountAchList != null)
            {
                _achLinkResults.Clear();
                foreach (var accountACH in accountAchList)
                {
                    _achLinkResults.Add(accountACH);
                }
            }

            var accountAchTransactionList = AchLinkModel.GetAcctTransactionsAchByAcctNum(acctNum);
            if (accountAchTransactionList != null)
            {
                _achTransactionResults.Clear();
                foreach (var accountAchTransaction in accountAchTransactionList)
                {
                    _achTransactionResults.Add(accountAchTransaction);
                }
            }

            OnPropertyChanged(String.Empty);
        }

        private AchLinkModel _selectedBank;
        public AchLinkModel SelectedBank
        {
            get => _selectedBank;
            set
            {
                if (_selectedBank != value)
                {
                    _selectedBank = value;
                    OnPropertyChanged(nameof(SelectedBank));
                    UpdateACHRoutingNumber();
                }
            }
        }

        private AchLinkModel _selectedBankType;
        public AchLinkModel SelectedBankType
        {
            get => _selectedBankType;
            set
            {
                if (_selectedBankType != value)
                {
                    _selectedBankType = value;
                    OnPropertyChanged(nameof(SelectedBankType));
                }
            }
        }

        private void UpdateACHRoutingNumber()
        {
            ACHRoutingNumber = SelectedBank?.AchRoutingNumber;
        }

        private string _achRoutingNumber;
        public string ACHRoutingNumber
        {
            get => _achRoutingNumber;
            set
            {
                if (_achRoutingNumber != value)
                {
                    _achRoutingNumber = value;
                    OnPropertyChanged(nameof(ACHRoutingNumber));
                }
            }
        }

        public string NewAchAccountNumber { get; set; }
        public string NewAchNickname { get; set; }

        private void ExecuteAddAchLink(object parameter)
        {
            string accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            string repId = RepIdService.Instance.RepId;

            try
            {
                AchLinkModel.InsertAcctAchSetupByAcctNum(
                    accountNumber,
                    SelectedBank?.AchRoutingNumber,
                    NewAchAccountNumber,
                    SelectedBankType?.AchBankType,
                    NewAchNickname,
                    repId
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to add new ACH link: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private decimal? _achPaymentAmount;

        public decimal? ACHPaymentAmount
        {
            get { return _achPaymentAmount; }
            set
            {
                if (_achPaymentAmount != value)
                {
                    _achPaymentAmount = value;
                    OnPropertyChanged(nameof(ACHPaymentAmount));
                }
            }
        }

        private string _achAccountNumber;
        public string ACHAccountNumber
        {
            get { return _achAccountNumber; }
            set
            {
                _achAccountNumber = value;
                OnPropertyChanged(nameof(ACHAccountNumber));
            }
        }

        public ICommand AddAchLinkCommand { get; }
        public ICommand AchLinkCommand { get; }

        private ICommand _compositeAddAchLinkCommand;
        public ICommand CompositeAddAchLinkCommand
        {
            get
            {
                if (_compositeAddAchLinkCommand == null)
                {
                    _compositeAddAchLinkCommand = new RelayCommand(ExecuteCompositeCommand);
                }
                return _compositeAddAchLinkCommand;
            }
        }

        private void ExecuteCompositeCommand(object parameter)
        {
            if (AddAchLinkCommand.CanExecute(null))
            {
                AddAchLinkCommand.Execute(null);
            }
            if (AchLinkCommand.CanExecute(null))
            {
                AchLinkCommand.Execute(null);
            }
        }


        public AchLinkVM(string accountNumber, IDialogService dialogService, ICommand[] commands)
            : base()
        {
            _dialogService =
                dialogService ?? throw new ArgumentNullException(nameof(dialogService));

            AchLinkCommand = commands[0];

            string acctNum = accountNumber;
            FetchAchDetails(acctNum);
            AddAchLinkCommand = new RelayCommand(ExecuteAddAchLink);
        }
    }
}
