using System;
using System.Windows;
using MainClient.Utilities;
using System.Windows.Input;
using MainClient._View;
using MainClient.Services;
using MainClient._Model;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using Microsoft.VisualBasic;

namespace MainClient._ViewModel
{
    public class MainWindowVM : ViewModelBase
    {
        private readonly IDialogService _dialogService;

        // Startup
        public ICommand HomeCommand { get; }

        private void Home(object parameter)
        {
            AccountNumService.Instance.SelectedAccountNumber = null; // Reset the account number.
            ClientIdService.Instance.SelectedCustId = null; // Reset the customer ID
            ClientIdService.Instance.SelectedJointCustId = null; // Reset the joint customer ID.
            ClientName = null; // Clear the client name
            
            // Clear the client account list
            Application.Current.Dispatcher.Invoke(() =>
            {
                _clientAcctsResults.Clear(); 
            });

            CurrentView = new HomeVM();
        }
        // File
        public ICommand NewSessionCommand { get; }
        public ICommand ExitCommand { get; }

        // Clients
        public ICommand AddClientCommand { get; }
        public ICommand AddAccountCommand { get; }
        public ICommand AddAccountNewClientCommand { get; }

        private void AddClient(object parameter)
        {
            if (!RepIdService.Instance.MoveMoney)
            {
                MessageBox.Show(
                    "You do not have the necessary permissions to add clients.",
                    "Permission Denied",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return; // Exit the method if no permissions are granted
            }

            var addClientWindow = new AddClient();
            var addClientVM = addClientWindow.DataContext as AddClientVM;
            if (addClientVM != null)
            {
                addClientVM.CloseAndLoadAccountAction = (customerId) =>
                {
                    ClientIdService.Instance.SelectedNewCustId = customerId;
                    addClientWindow.Close();
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        AddAccountNewClient(ClientIdService.Instance.SelectedNewCustId);
                    });
                };
            }
            addClientWindow.Show();
        }

        private void AddAccount(object parameter)
        {
            if (!RepIdService.Instance.MoveMoney)
            {
                MessageBox.Show(
                    "You do not have the necessary permissions to add clients.",
                    "Permission Denied",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return; // Exit the method if no permissions are granted
            }

            var addAccountWindow = new AddAccount();
            var addAccountVM = addAccountWindow.DataContext as AddAccountVM;
            if (addAccountVM != null)
            {
                addAccountVM.CloseAndLoadAccountAction = (accountNumber) =>
                {
                    AccountNumService.Instance.SelectedAccountNumber = accountNumber;
                    addAccountWindow.Close();
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        SetClientNameByAccountNumber(accountNumber);

                        SetClientAcctByAccountNumber(accountNumber);

                        LoadViewModel<AccountOverviewVM>(
                            accountNumber,
                            (acctNum, dialogService, cmds) =>
                                new AccountOverviewVM(acctNum, dialogService, cmds),
                            "Account"
                        );
                    });
                };
            }
            addAccountWindow.Show();
        }

        private void AddAccountNewClient(object parameter)
        {
            if (!RepIdService.Instance.MoveMoney)
            {
                MessageBox.Show(
                    "You do not have the necessary permissions to add clients.",
                    "Permission Denied",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return; // Exit the method if no permissions are granted
            }

            var addAccountNewClientWindow = new AddAccountNewClient();
            var addAccountNewClientVM =
                addAccountNewClientWindow.DataContext as AddAccountNewClientVM;
            if (addAccountNewClientVM != null)
            {
                addAccountNewClientVM.CloseAndLoadAccountAction = (accountNumber) =>
                {
                    AccountNumService.Instance.SelectedAccountNumber = accountNumber;
                    addAccountNewClientWindow.Close();
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        SetClientNameByAccountNumber(accountNumber);

                        SetClientAcctByAccountNumber(accountNumber);
                        
                        LoadViewModel<AccountOverviewVM>(
                            accountNumber,
                            (acctNum, dialogService, cmds) =>
                                new AccountOverviewVM(acctNum, dialogService, cmds),
                            "Account"
                        );
                    });
                };
            }
            addAccountNewClientWindow.Show();
        }

        // Search
        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged("SearchText");
                }
            }
        }
        public ICommand SearchCommand { get; }

        private SearchModel _searchModel;
        private SearchVM _searchVM;

        private void Search(object parameter)
        {
            string customerId = null;
            string custTaxId = null;
            string acctNum = null;

            if (
                !RepIdService.Instance.Trading
                && !RepIdService.Instance.MoveMoney
                && !RepIdService.Instance.ViewOnly
            )
            {
                MessageBox.Show(
                    "You do not have the necessary permissions to search.",
                    "Permission Denied",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            if (SearchText.Length == 12)
            {
                customerId = SearchText;
            }
            else if (SearchText.Length == 9 || SearchText.Length == 11)
            {
                custTaxId = SearchText;
            }
            else if (SearchText.Length == 10 || SearchText.Length == 14)
            {
                acctNum = SearchText;
            }
            else
            {
                MessageBox.Show("Customer ID must be 12 characters long.\n" +
                                "Account Number must be 10 or 14 digits.\n" +
                                "Customer Tax ID must be 9 or 11 digits.", "Search Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _searchVM = new SearchVM();
            var results = _searchModel.AcctSearch(customerId, custTaxId, acctNum);
            _searchVM.SearchResults = new ObservableCollection<SearchModel.SearchResult>(results);

            var searchWindow = new Search();
            searchWindow.DataContext = _searchVM;

            _searchVM.CloseAndLoadAccountAction = (accountNumber) =>
            {
                AccountNumService.Instance.SelectedAccountNumber = accountNumber;
                searchWindow.Close();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    AccountOverview(accountNumber);
                });
            };

            searchWindow.Show();;
        }
        

        // Advanced Search
        public ICommand AdvancedSearchCommand { get; private set; }

        private void AdvancedSearch(object parameter)
        {
            if (
                !RepIdService.Instance.Trading
                && !RepIdService.Instance.MoveMoney
                && !RepIdService.Instance.ViewOnly
            )
            {
                MessageBox.Show(
                    "You do not have the necessary permissions to search.",
                    "Permission Denied",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            var advancedSearchWindow = new AdvancedSearch();
            var advancedSearchVM = advancedSearchWindow.DataContext as AdvancedSearchVM;
            if (advancedSearchVM != null)
            {
                advancedSearchVM.CloseAndLoadAccountAction = (accountNumber) =>
                {
                    AccountNumService.Instance.SelectedAccountNumber = accountNumber;
                    advancedSearchWindow.Close();
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        AccountOverview(accountNumber);
                    });
                };
            }
            advancedSearchWindow.Show();
        }

        // Client & Accounts
        public delegate ICommand CommandSelector(string accountNumber);
        public ICommand ClientOverviewCommand { get; }
        public ICommand ClientEditContactCommand { get; }
        public ICommand ClientEditEmploymentCommand { get; }
        public ICommand ClientEditInformationCommand { get; }
        public ICommand ClientEditTaxCommand { get; }
        public ICommand AccountOverviewCommand { get; }
        public ICommand AccountAddBeneficiaryCommand { get; }
        public ICommand AccountEditBeneficiaryCommand { get; }
        public ICommand AccountAddPowerOfAttorneyCommand { get; }
        public ICommand AccountEditPowerOfAttorneyCommand { get; }
        public ICommand AccountEditContactCommand { get; }
        public ICommand AccountEditLimitCommand { get; }
        public ICommand AccountEditPasswordCommand { get; }
        public ICommand PositionsCommand { get; }
        public ICommand TransactionsCommand { get; }
        public ICommand AccessHistoryCommand { get; }

        private void ClientOverview(object parameter)
        {
            var accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                if (
                    !RepIdService.Instance.Trading
                    && !RepIdService.Instance.MoveMoney
                    && !RepIdService.Instance.ViewOnly
                )
                {
                    MessageBox.Show(
                        "You do not have the necessary permissions to view clients.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return; // Exit the method if no permissions are granted
                }

                LoadViewModel<ClientOverviewVM>(
                    accountNumber,
                    (acctNum, dialogService, cmds) =>
                        new ClientOverviewVM(acctNum, dialogService, cmds),
                    "Client"
                );
            }
        }

        private void ClientEditContact(object parameter)
        {
            var accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                if (!RepIdService.Instance.MoveMoney && !RepIdService.Instance.ViewOnly)
                {
                    MessageBox.Show(
                        "You do not have the necessary permissions to edit client details.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return; // Exit the method if no permissions are granted
                }

                var clientEditContactWindow = new ClientEditContact();
                var clientEditContactWindowVM =
                    clientEditContactWindow.DataContext as ClientEditContactVM;
                if (clientEditContactWindowVM != null)
                {
                    clientEditContactWindowVM.CloseAndLoadAccountAction = (accountNumber) =>
                    {
                        clientEditContactWindow.Close();
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            LoadViewModel<ClientOverviewVM>(
                                accountNumber,
                                (acctNum, dialogService, cmds) =>
                                    new ClientOverviewVM(acctNum, dialogService, cmds),
                                "Client"
                            );
                        });
                    };
                }

                clientEditContactWindow.Show();
            }
        }

        private void ClientEditEmployment(object parameter)
        {
            var accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                if (!RepIdService.Instance.MoveMoney && !RepIdService.Instance.ViewOnly)
                {
                    MessageBox.Show(
                        "You do not have the necessary permissions to edit client details.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return; // Exit the method if no permissions are granted
                }

                var clientEditEmploymentWindow = new ClientEditEmployment();
                var clientEditEmploymentWindowVM =
                    clientEditEmploymentWindow.DataContext as ClientEditEmploymentVM;
                if (clientEditEmploymentWindowVM != null)
                {
                    clientEditEmploymentWindowVM.CloseAndLoadAccountAction = (accountNumber) =>
                    {
                        clientEditEmploymentWindow.Close();
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            LoadViewModel<ClientOverviewVM>(
                                accountNumber,
                                (acctNum, dialogService, cmds) =>
                                    new ClientOverviewVM(acctNum, dialogService, cmds),
                                "Client"
                            );
                        });
                    };
                }

                clientEditEmploymentWindow.Show();
            }
        }

        private void ClientEditInformation(object parameter)
        {
            var accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                if (!RepIdService.Instance.MoveMoney && !RepIdService.Instance.ViewOnly)
                {
                    MessageBox.Show(
                        "You do not have the necessary permissions to edit client details.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return; // Exit the method if no permissions are granted
                }

                var clientEditInformationWindow = new ClientEditInformation();
                var clientEditInformationWindowVM =
                    clientEditInformationWindow.DataContext as ClientEditInformationVM;
                if (clientEditInformationWindowVM != null)
                {
                    clientEditInformationWindowVM.CloseAndLoadAccountAction = (accountNumber) =>
                    {
                        clientEditInformationWindow.Close();
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            LoadViewModel<ClientOverviewVM>(
                                accountNumber,
                                (acctNum, dialogService, cmds) =>
                                    new ClientOverviewVM(acctNum, dialogService, cmds),
                                "Client"
                            );
                        });
                    };
                }

                clientEditInformationWindow.Show();
            }
        }

        private void ClientEditTax(object parameter)
        {
            var accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                if (!RepIdService.Instance.MoveMoney && !RepIdService.Instance.ViewOnly)
                {
                    MessageBox.Show(
                        "You do not have the necessary permissions to edit client details.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return; // Exit the method if no permissions are granted
                }

                var clientEditTaxWindow = new ClientEditTax();
                var clientEditTaxWindowVM = clientEditTaxWindow.DataContext as ClientEditTaxVM;
                if (clientEditTaxWindowVM != null)
                {
                    clientEditTaxWindowVM.CloseAndLoadAccountAction = (accountNumber) =>
                    {
                        clientEditTaxWindow.Close();
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            LoadViewModel<ClientOverviewVM>(
                                accountNumber,
                                (acctNum, dialogService, cmds) =>
                                    new ClientOverviewVM(acctNum, dialogService, cmds),
                                "Client"
                            );
                        });
                    };
                }

                clientEditTaxWindow.Show();
            }
        }

        // Get Client Name

        private string _clientName;
        public string ClientName
        {
            get => _clientName;
            set
            {
                if (_clientName != value)
                {
                    _clientName = value;
                    OnPropertyChanged(nameof(ClientName));
                }
            }
        }

        // Get Client Accts List
        private ObservableCollection<MainWindowModel> _clientAcctsResults =
            new ObservableCollection<MainWindowModel>();
        public ObservableCollection<MainWindowModel> ClientAcctsResults
        {
            get => _clientAcctsResults;
            set
            {
                if (_clientAcctsResults != value)
                {
                    _clientAcctsResults = value;
                    OnPropertyChanged(nameof(ClientAcctsResults));
                }
            }
        }

        private MainWindowModel _selectedClientAccount;
        public MainWindowModel SelectedClientAccount
        {
            get => _selectedClientAccount;
            set
            {
                if (_selectedClientAccount != value)
                {
                    _selectedClientAccount = value;
                    OnPropertyChanged(nameof(SelectedClientAccount));
                }
            }
        }

        private void SetClientNameByAccountNumber(string accountNumber)
        {
            var clientNameModel = MainWindowModel.GetClientNameByAcctNum(accountNumber);

            if (clientNameModel != null)
            {
                ClientName = clientNameModel.ClientFullName;
            }
        }

        private void SetClientAcctByAccountNumber(string accountNumber)
        {
            var clientAccountList = MainWindowModel.GetClientAcctListByAcctNum(accountNumber);            

            if (clientAccountList != null)
            {
                MainWindowModel currentAccount = null;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _clientAcctsResults.Clear();
                    foreach (var accountList in clientAccountList)
                    {
                        _clientAcctsResults.Add(accountList);

                        if (accountList.ClientAcctAndName.StartsWith(accountNumber))
                        {
                            currentAccount = accountList;
                        }
                    }

                    var collectionView = CollectionViewSource.GetDefaultView(
                        _clientAcctsResults
                    );

                    if (collectionView != null && currentAccount != null)
                    {
                        collectionView.MoveCurrentTo(currentAccount);
                    }
                });
            }
        }

        private string _lastProcessedAccountNumber;
        public void HandleSelectionChange(MainWindowModel selectedAccount)
        {
            if (selectedAccount == null)
                return;

            SelectedClientAccount = selectedAccount;
            var parts = selectedAccount.ClientAcctAndName.Split(
                new string[] { " - " },
                StringSplitOptions.None
            );
            var accountNumber = parts.Length > 0 ? parts[0] : string.Empty;
            
            var accountType = selectedAccount.AcctType;

            string repId = RepIdService.Instance.RepId;
            AccountNumService.Instance.SelectedAccountNumber = accountNumber;
            AccountNumService.Instance.SelectedAccountType = accountType;
            ClientIdService.Instance.SelectedCustId = selectedAccount.CustId;
	        ClientIdService.Instance.SelectedJointCustId = selectedAccount.JointCustId;
            
            if (_lastProcessedAccountNumber != accountNumber)
            {                
                AdvancedSearchModel.InsertAcctAccessHistoryByAcctNum(accountNumber, repId); 
                _lastProcessedAccountNumber = accountNumber;
            }

            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            SetClientNameByAccountNumber(accountNumber);

            LoadViewModel<AccountOverviewVM>(
                accountNumber,
                (acctNum, dialogService, cmds) =>
                    new AccountOverviewVM(acctNum, dialogService, cmds),
                "Account"
            );       
        }

        public void AccountOverview(object parameter)
        {
            _searchText = null;
            OnPropertyChanged(nameof(SearchText));

            var accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                if (
                    !RepIdService.Instance.Trading
                    && !RepIdService.Instance.MoveMoney
                    && !RepIdService.Instance.ViewOnly
                )
                {
                    MessageBox.Show(
                        "You do not have the necessary permissions to view accounts.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return; // Exit the method if no permissions are granted
                }

                SetClientNameByAccountNumber(accountNumber);

                SetClientAcctByAccountNumber(accountNumber);
                
                LoadViewModel<AccountOverviewVM>(
                    accountNumber,
                    (acctNum, dialogService, cmds) =>
                        new AccountOverviewVM(acctNum, dialogService, cmds),
                    "Account"
                );
            }
        }

        private void AccountEditBeneficiary(object parameter) { }

        private void AccountAddBeneficiary(object parameter)
        {
            var accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                if (!RepIdService.Instance.MoveMoney && !RepIdService.Instance.ViewOnly)
                {
                    MessageBox.Show(
                        "You do not have the necessary permissions to edit account details.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return; // Exit the method if no permissions are granted
                }

                var accountAddBeneficiaryWindow = new AccountAddBeneficiary();
                var accountAddBeneficiaryVM =
                    accountAddBeneficiaryWindow.DataContext as AccountAddBeneficiaryVM;
                if (accountAddBeneficiaryVM != null)
                {
                    accountAddBeneficiaryVM.CloseAndLoadAccountAction = (accountNumber) =>
                    {
                        accountAddBeneficiaryWindow.Close();
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            LoadViewModel<AccountOverviewVM>(
                                accountNumber,
                                (acctNum, dialogService, cmds) =>
                                    new AccountOverviewVM(acctNum, dialogService, cmds),
                                "Account"
                            );
                        });
                    };
                }

                accountAddBeneficiaryWindow.Show();
            }
        }

        private void AccountEditPowerOfAttorney(object parameter) { }

        private void AccountAddPowerOfAttorney(object parameter)
        {
            var accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                if (!RepIdService.Instance.MoveMoney && !RepIdService.Instance.ViewOnly)
                {
                    MessageBox.Show(
                        "You do not have the necessary permissions to edit account details.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return; // Exit the method if no permissions are granted
                }

                var accountAddPowerOfAttorneyWindow = new AccountAddPowerOfAttorney();
                var accountAddPowerOfAttorneyWindowVM =
                    accountAddPowerOfAttorneyWindow.DataContext as AccountAddPowerOfAttorneyVM;
                if (accountAddPowerOfAttorneyWindowVM != null)
                {
                    accountAddPowerOfAttorneyWindowVM.CloseAndLoadAccountAction = (accountNumber) =>
                    {
                        accountAddPowerOfAttorneyWindow.Close();
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            LoadViewModel<AccountOverviewVM>(
                                accountNumber,
                                (acctNum, dialogService, cmds) =>
                                    new AccountOverviewVM(acctNum, dialogService, cmds),
                                "Account"
                            );
                        });
                    };
                }

                accountAddPowerOfAttorneyWindow.Show();
            }
        }

        private void AccountEditContact(object parameter)
        {
            var accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                if (!RepIdService.Instance.MoveMoney && !RepIdService.Instance.ViewOnly)
                {
                    MessageBox.Show(
                        "You do not have the necessary permissions to edit account details.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return; // Exit the method if no permissions are granted
                }

                var accountEditContactWindow = new AccountEditContact();
                var accountEditContactWindowVM =
                    accountEditContactWindow.DataContext as AccountEditContactVM;
                if (accountEditContactWindowVM != null)
                {
                    accountEditContactWindowVM.CloseAndLoadAccountAction = (accountNumber) =>
                    {
                        accountEditContactWindow.Close();
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            LoadViewModel<AccountOverviewVM>(
                                accountNumber,
                                (acctNum, dialogService, cmds) =>
                                    new AccountOverviewVM(acctNum, dialogService, cmds),
                                "Account"
                            );
                        });
                    };
                }

                accountEditContactWindow.Show();
            }
        }

        private void AccountEditLimit(object parameter)
        {
            var accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                if (!RepIdService.Instance.MoveMoney && !RepIdService.Instance.ViewOnly)
                {
                    MessageBox.Show(
                        "You do not have the necessary permissions to edit account details.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return; // Exit the method if no permissions are granted
                }

                var accountEditLimitWindow = new AccountEditLimit();
                var accountEditLimitWindowVM =
                    accountEditLimitWindow.DataContext as AccountEditLimitVM;
                if (accountEditLimitWindowVM != null)
                {
                    accountEditLimitWindowVM.CloseAndLoadAccountAction = (accountNumber) =>
                    {
                        accountEditLimitWindow.Close();
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            LoadViewModel<AccountOverviewVM>(
                                accountNumber,
                                (acctNum, dialogService, cmds) =>
                                    new AccountOverviewVM(acctNum, dialogService, cmds),
                                "Account"
                            );
                        });
                    };
                }

                accountEditLimitWindow.Show();
            }
        }

        private void AccountEditPassword(object parameter)
        {
            var accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                if (!RepIdService.Instance.MoveMoney && !RepIdService.Instance.ViewOnly)
                {
                    MessageBox.Show(
                        "You do not have the necessary permissions to edit account details.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return; // Exit the method if no permissions are granted
                }

                var accountEditPasswordWindow = new AccountEditPassword();
                var accountEditPasswordWindowVM =
                    accountEditPasswordWindow.DataContext as AccountEditPasswordVM;
                if (accountEditPasswordWindowVM != null)
                {
                    accountEditPasswordWindowVM.CloseAndLoadAccountAction = (accountNumber) =>
                    {
                        accountEditPasswordWindow.Close();
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            LoadViewModel<AccountOverviewVM>(
                                accountNumber,
                                (acctNum, dialogService, cmds) =>
                                    new AccountOverviewVM(acctNum, dialogService, cmds),
                                "Account"
                            );
                        });
                    };
                }

                accountEditPasswordWindow.Show();
            }
        }

        private void Positions(object parameter)
        {
            var accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            var accountType = AccountNumService.Instance.SelectedAccountType;

            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else if (accountType < 1 || accountType > 15)
            {
                MessageBox.Show(
                    "This account type cannot hold securities.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                if (
                    !RepIdService.Instance.Trading
                    && !RepIdService.Instance.MoveMoney
                    && !RepIdService.Instance.ViewOnly
                )
                {
                    MessageBox.Show(
                        "You do not have the necessary permissions to view positions.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return;
                }

                LoadViewModel(accountNumber, acctNum => new PositionsVM(acctNum));
            }
        }

        private void Transactions(object parameter)
        {
            var accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                if (
                    !RepIdService.Instance.Trading
                    && !RepIdService.Instance.MoveMoney
                    && !RepIdService.Instance.ViewOnly
                )
                {
                    MessageBox.Show(
                        "You do not have the necessary permissions to view transactions.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return; // Exit the method if no permissions are granted
                }

                LoadViewModel(accountNumber, acctNum => new TransactionsVM(acctNum));
            }
        }

        private void AccessHistory(object parameter)
        {
            var accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                if (
                    !RepIdService.Instance.Trading
                    && !RepIdService.Instance.MoveMoney
                    && !RepIdService.Instance.ViewOnly
                )
                {
                    MessageBox.Show(
                        "You do not have the necessary permissions to view transactions.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return; // Exit the method if no permissions are granted
                }

                LoadViewModel(accountNumber, acctNum => new AccessHistoryVM(acctNum));
            }
        }

        // Notes
        public ICommand ViewNotesCommand { get; }

        private void ViewNotes(object parameter)
        {
            var accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                if (
                    !RepIdService.Instance.Trading
                    && !RepIdService.Instance.MoveMoney
                    && !RepIdService.Instance.ViewOnly
                )
                {
                    MessageBox.Show(
                        "You do not have the necessary permissions to view transactions.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return; // Exit the method if no permissions are granted
                }

                LoadViewModel<ViewNotesVM>(
                    accountNumber,
                    (acctNum, dialogService, cmds) =>
                        new ViewNotesVM(acctNum, dialogService, cmds),
                    "Notes"
                );
            }
        }

        public ICommand AddNotesCommand { get; private set; }

        private void AddNotes(object parameter)
        {
            var accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                if (!RepIdService.Instance.Trading && !RepIdService.Instance.MoveMoney)
                {
                    MessageBox.Show(
                        "You do not have the necessary permissions to deposit into account.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return;
                }

                var addNotesWindow = new AddNotes();
                var addNotesVM = addNotesWindow.DataContext as AddNotesVM;
                if (addNotesVM != null)
                {
                    addNotesVM.CloseAndLoadAccountAction = (accountNumber) =>
                    {
                        addNotesWindow.Close();
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            ViewNotes(accountNumber);
                        });
                    };
                }

                addNotesWindow.Show();
            }
        }

        // Cashiering
        public ICommand DepositCashCommand { get; }
        public ICommand DepositCheckMultipleCommand { get; }
        public ICommand DepositCheckSingleCommand { get; }
        public ICommand DepositCheckSplitCommand { get; }
        public ICommand WithdrawalCashCommand { get; }
        public ICommand WithdrawalJournalCommand { get; }
        public ICommand WithdrawalAchCommand { get; }
        public ICommand WithdrawalCheckSingleCommand { get; }
        public ICommand WithdrawalWireCommand { get; }
        public ICommand AchLinkCommand { get; }

        private void DepositCash(object parameter)
        {
            var accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                if (!RepIdService.Instance.MoveMoney)
                {
                    MessageBox.Show(
                        "You do not have the necessary permissions to deposit into account.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return;
                }

                var depositCashWindow = new DepositCash();
                var depositCashVM = depositCashWindow.DataContext as DepositCashVM;
                if (depositCashVM != null)
                {
                    depositCashVM.CloseAndLoadAccountAction = (accountNumber) =>
                    {
                        depositCashWindow.Close();
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Transactions(accountNumber);
                        });
                    };
                }

                depositCashWindow.Show();
            }
        }

        private void DepositCheckMultiple(object parameter)
        {
            var accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                if (!RepIdService.Instance.MoveMoney)
                {
                    MessageBox.Show(
                        "You do not have the necessary permissions to deposit into account.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return; // Exit the method if no permissions are granted
                }

                var DepositCheckMultipleWindow = new DepositCheckMultiple();
                var DepositCheckMultipleVM =
                    DepositCheckMultipleWindow.DataContext as DepositCheckMultipleVM;
                if (DepositCheckMultipleVM != null)
                {
                    DepositCheckMultipleVM.CloseAndLoadAccountAction = (accountNumber) =>
                    {
                        DepositCheckMultipleWindow.Close();
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Transactions(accountNumber);
                        });
                    };
                }

                DepositCheckMultipleWindow.Show();
            }
        }

        private void DepositCheckSingle(object parameter)
        {
            var accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                if (!RepIdService.Instance.MoveMoney)
                {
                    MessageBox.Show(
                        "You do not have the necessary permissions to deposit into account.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return; // Exit the method if no permissions are granted
                }

                var DepositCheckSingleWindow = new DepositCheckSingle();
                var DepositCheckSingleVM =
                    DepositCheckSingleWindow.DataContext as DepositCheckSingleVM;
                if (DepositCheckSingleVM != null)
                {
                    DepositCheckSingleVM.CloseAndLoadAccountAction = (accountNumber) =>
                    {
                        DepositCheckSingleWindow.Close();
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Transactions(accountNumber);
                        });
                    };
                }

                DepositCheckSingleWindow.Show();
            }
        }

        private void DepositCheckSplit(object parameter)
        {
            var accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                if (!RepIdService.Instance.MoveMoney)
                {
                    MessageBox.Show(
                        "You do not have the necessary permissions to deposit into account.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return; // Exit the method if no permissions are granted
                }

                var DepositCheckSplitWindow = new DepositCheckSplit();
                var DepositCheckSplitVM =
                    DepositCheckSplitWindow.DataContext as DepositCheckSplitVM;
                if (DepositCheckSplitVM != null)
                {
                    DepositCheckSplitVM.CloseAndLoadAccountAction = (accountNumber) =>
                    {
                        DepositCheckSplitWindow.Close();
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Transactions(accountNumber);
                        });
                    };
                }

                DepositCheckSplitWindow.Show();
            }
        }

        private void WithdrawalCash(object parameter)
        {
            var accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                if (!RepIdService.Instance.MoveMoney)
                {
                    MessageBox.Show(
                        "You do not have the necessary permissions to deposit into account.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return; // Exit the method if no permissions are granted
                }

                var withdrawalCashWindow = new WithdrawalCash();
                var withdrawalCashWindowVM = withdrawalCashWindow.DataContext as WithdrawalCashVM;
                if (withdrawalCashWindowVM != null)
                {
                    withdrawalCashWindowVM.CloseAndLoadAccountAction = (accountNumber) =>
                    {
                        withdrawalCashWindow.Close();
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Transactions(accountNumber);
                        });
                    };
                }

                withdrawalCashWindow.Show();
            }
        }

        private void WithdrawalJournal(object parameter)
        {
            var accountNumber = AccountNumService.Instance.SelectedAccountNumber;

            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                if (!RepIdService.Instance.MoveMoney)
                {
                    MessageBox.Show(
                        "You do not have the necessary permissions to deposit into account.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return; // Exit the method if no permissions are granted
                }

                var withdrawalJournalWindow = new WithdrawalJournal();
                var withdrawalJournalVM =
                    withdrawalJournalWindow.DataContext as WithdrawalJournalVM;
                if (withdrawalJournalVM != null)
                {
                    withdrawalJournalVM.CloseAndLoadAccountAction = (accountNumber) =>
                    {
                        withdrawalJournalWindow.Close();
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Transactions(accountNumber);
                        });
                    };
                }

                withdrawalJournalWindow.Show();
            }
        }

        private void WithdrawalAch(object parameter)
        {
            var accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                if (!RepIdService.Instance.MoveMoney)
                {
                    MessageBox.Show(
                        "You do not have the necessary permissions to deposit into account.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return; // Exit the method if no permissions are granted
                }

                var WithdrawalAchWindow = new WithdrawalAch();
                var WithdrawalAchWindowVM = WithdrawalAchWindow.DataContext as WithdrawalAchVM;
                if (WithdrawalAchWindowVM != null)
                {
                    WithdrawalAchWindowVM.CloseAndLoadAccountAction = (accountNumber) =>
                    {
                        WithdrawalAchWindow.Close();
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Transactions(accountNumber);
                        });
                    };
                }

                WithdrawalAchWindow.Show();
            }
        }

        private void WithdrawalCheckSingle(object parameter)
        {
            var accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                if (!RepIdService.Instance.MoveMoney)
                {
                    MessageBox.Show(
                        "You do not have the necessary permissions to deposit into account.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return; // Exit the method if no permissions are granted
                }

                var WithdrawalCheckSingleWindow = new WithdrawalCheckSingle();
                var WithdrawalCheckSingleVM =
                    WithdrawalCheckSingleWindow.DataContext as WithdrawalCheckSingleVM;
                if (WithdrawalCheckSingleVM != null)
                {
                    WithdrawalCheckSingleVM.CloseAndLoadAccountAction = (accountNumber) =>
                    {
                        WithdrawalCheckSingleWindow.Close();
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Transactions(accountNumber);
                        });
                    };
                }

                WithdrawalCheckSingleWindow.Show();
            }
        }

        private void WithdrawalWire(object parameter)
        {
            var accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                if (!RepIdService.Instance.MoveMoney)
                {
                    MessageBox.Show(
                        "You do not have the necessary permissions to deposit into account.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return; // Exit the method if no permissions are granted
                }

                var WithdrawalWireWindow = new WithdrawalWire();
                var WithdrawalWireVM = WithdrawalWireWindow.DataContext as WithdrawalWireVM;
                if (WithdrawalWireVM != null)
                {
                    WithdrawalWireVM.CloseAndLoadAccountAction = (accountNumber) =>
                    {
                        WithdrawalWireWindow.Close();
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Transactions(accountNumber);
                        });
                    };
                }

                WithdrawalWireWindow.Show();
            }
        }

        private void AchLink(object parameter)
        {
            var accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                if (!RepIdService.Instance.MoveMoney && !RepIdService.Instance.ViewOnly)
                {
                    MessageBox.Show(
                        "You do not have the necessary permissions to view transactions.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return; // Exit the method if no permissions are granted
                }

                LoadViewModel<AchLinkVM>(
                    accountNumber,
                    (acctNum, dialogService, cmds) =>
                        new AchLinkVM(acctNum, dialogService, cmds),
                    "Ach"
                );
            }
        }

        // Trading
        public ICommand TradeBuyCommand { get; }
        public ICommand TradeSellCommand { get; }
        public ICommand TradeStatusCommand { get; }

        private void TradeBuy(object parameter)
        {
            var accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            var accountType = AccountNumService.Instance.SelectedAccountType;
            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected..",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else if (accountType < 1 || accountType > 15)
            {
                MessageBox.Show(
                    "This account type cannot make trades.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else 
            {
                if (!RepIdService.Instance.Trading)
                {
                    MessageBox.Show(
                        "You do not have the necessary permissions to deposit into account.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return; // Exit the method if no permissions are granted
                }

                var TradeBuyWindow = new TradeBuy();
                var TradeBuyVM = TradeBuyWindow.DataContext as TradeBuyVM;
                if (TradeBuyVM != null)
                {
                    TradeBuyVM.CloseAndLoadAccountAction = (accountNumber) =>
                    {
                        TradeBuyWindow.Close();
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            TradeStatus(accountNumber);
                        });
                    };
                }

                TradeBuyWindow.Show();
            }
        }

        private void TradeSell(object parameter)
        {
            var accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            var accountType = AccountNumService.Instance.SelectedAccountType;

            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else if (accountType < 1 || accountType > 15)
            {
                MessageBox.Show(
                    "This account type cannot make trades.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                if (!RepIdService.Instance.Trading)
                {
                    MessageBox.Show(
                        "You do not have the necessary permissions to deposit into account.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return; // Exit the method if no permissions are granted
                }

                var TradeSellWindow = new TradeSell();
                var TradeSellVM = TradeSellWindow.DataContext as TradeSellVM;
                if (TradeSellVM != null)
                {
                    TradeSellVM.CloseAndLoadAccountAction = (accountNumber) =>
                    {
                        TradeSellWindow.Close();
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            TradeStatus(accountNumber);
                        });
                    };
                }

                TradeSellWindow.Show();
            }
        }

        private void TradeStatus(object parameter)
        {
            var accountNumber = AccountNumService.Instance.SelectedAccountNumber;
            var accountType = AccountNumService.Instance.SelectedAccountType;

            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else if (accountType < 1 || accountType > 15)
            {
                MessageBox.Show(
                    "This account type cannot make trades.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                if (
                    !RepIdService.Instance.Trading
                    && !RepIdService.Instance.MoveMoney
                    && !RepIdService.Instance.ViewOnly
                )
                {
                    MessageBox.Show(
                        "You do not have the necessary permissions to view transactions.",
                        "Permission Denied",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return; // Exit the method if no permissions are granted
                }

                LoadViewModel(accountNumber, acctNum => new TradeStatusVM(acctNum));
            }
        }

        // Random Word
        private string _newWord;

        public string NewWord
        {
            get { return _newWord; }
            set
            {
                if (_newWord != value)
                {
                    _newWord = value;
                    OnPropertyChanged(nameof(NewWord));
                }
            }
        }

        public ICommand GetRandomWordCommand { get; }

        private void FetchRandomWord()
        {
            var randomWordModel = MainClient._Model.MainWindowModel.GetRandomWord();
            if (randomWordModel != null)
            {
                NewWord = randomWordModel.NewWord;
            }
        }

        // IDialog Service
        public MainWindowVM(IDialogService dialogService)
        {
            _dialogService =
                dialogService ?? throw new ArgumentNullException(nameof(dialogService));

            // Startup Page
            HomeCommand = new RelayCommand(Home);
            CurrentView = new HomeVM();
            // File
            NewSessionCommand = new RelayCommand(Home);
            // Client
            AddClientCommand = new RelayCommand(AddClient);
            AddAccountCommand = new RelayCommand(AddAccount);
            AddAccountNewClientCommand = new RelayCommand(AddAccountNewClient);
            // Search
            SearchCommand = new RelayCommand(Search);
            _searchModel = new SearchModel();
            _searchVM = new SearchVM();
            AdvancedSearchCommand = new RelayCommand(AdvancedSearch);
            // Client & Accounts
            ClientOverviewCommand = new RelayCommand(ClientOverview);
            ClientEditContactCommand = new RelayCommand(ClientEditContact);
            ClientEditEmploymentCommand = new RelayCommand(ClientEditEmployment);
            ClientEditInformationCommand = new RelayCommand(ClientEditInformation);
            ClientEditTaxCommand = new RelayCommand(ClientEditTax);
            AccountOverviewCommand = new RelayCommand(AccountOverview);
            AccountAddBeneficiaryCommand = new RelayCommand(AccountAddBeneficiary);
            AccountEditBeneficiaryCommand = new RelayCommand(AccountEditBeneficiary);
            AccountAddPowerOfAttorneyCommand = new RelayCommand(AccountAddPowerOfAttorney);
            AccountEditPowerOfAttorneyCommand = new RelayCommand(AccountEditPowerOfAttorney);
            AccountEditContactCommand = new RelayCommand(AccountEditContact);
            AccountEditLimitCommand = new RelayCommand(AccountEditLimit);
            AccountEditPasswordCommand = new RelayCommand(AccountEditPassword);
            PositionsCommand = new RelayCommand(Positions);
            TransactionsCommand = new RelayCommand(Transactions);
            AccessHistoryCommand = new RelayCommand(AccessHistory);
            // Notes
            ViewNotesCommand = new RelayCommand(ViewNotes);
            AddNotesCommand = new RelayCommand(AddNotes);
            // Cashiering
            DepositCashCommand = new RelayCommand(DepositCash);
            DepositCheckMultipleCommand = new RelayCommand(DepositCheckMultiple);
            DepositCheckSingleCommand = new RelayCommand(DepositCheckSingle);
            DepositCheckSplitCommand = new RelayCommand(DepositCheckSplit);
            WithdrawalCashCommand = new RelayCommand(WithdrawalCash);
            WithdrawalJournalCommand = new RelayCommand(WithdrawalJournal);
            WithdrawalAchCommand = new RelayCommand(WithdrawalAch);
            WithdrawalCheckSingleCommand = new RelayCommand(WithdrawalCheckSingle);
            WithdrawalWireCommand = new RelayCommand(WithdrawalWire);
            AchLinkCommand = new RelayCommand(AchLink);
            // Trading
            TradeBuyCommand = new RelayCommand(TradeBuy);
            TradeSellCommand = new RelayCommand(TradeSell);
            TradeStatusCommand = new RelayCommand(TradeStatus);
            // Random Word
            GetRandomWordCommand = new RelayCommand(o => FetchRandomWord());
            FetchRandomWord();
        }

        public MainWindowVM() { }

        protected void LoadViewModel<T>(
            string accountNumber,
            Func<string, IDialogService, ICommand[], T> viewModelFactory,
            string viewModelType // additional parameter to control behavior
        )
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                MessageBox.Show(
                    "No account selected.",
                    "Selection Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            IDialogService dialogService = new DialogService();

            ICommand[] commands;
            if (viewModelType == "Account")
            {
                commands = new ICommand[]
                {
                    new RelayCommand(AccountAddBeneficiary),
                    new RelayCommand(AccountEditBeneficiary),
                    new RelayCommand(AccountAddPowerOfAttorney),
                    new RelayCommand(AccountEditPowerOfAttorney),
                    new RelayCommand(AccountEditContact),
                    new RelayCommand(AccountEditLimit),
                    new RelayCommand(AccountEditPassword)
                };
            }
            else if (viewModelType == "Client")
            {
                commands = new ICommand[]
                {
                    new RelayCommand(ClientEditContact),
                    new RelayCommand(ClientEditEmployment),
                    new RelayCommand(ClientEditInformation),
                    new RelayCommand(ClientEditTax),
                    new RelayCommand(ViewNotes),
                    new RelayCommand(AddNotes),
                    new RelayCommand(Transactions)
                };
            }
            else if (viewModelType == "Notes")
            {
                commands = new ICommand[]
                {
                    new RelayCommand(AddNotes)
                };
            }
            else if (viewModelType == "Ach")
            {
                commands = new ICommand[]
                {
                    new RelayCommand(AchLink)
                };
            }
            else
            {
                throw new ArgumentException("Invalid viewModelType provided");
            }

            CurrentView = viewModelFactory(accountNumber, dialogService, commands);
        }
    }
}
