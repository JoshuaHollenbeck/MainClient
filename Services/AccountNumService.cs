namespace MainClient.Services
{
    // Service class for managing getting and setting the account number.
    public class AccountNumService
    {
        // Singleton instance of AccountNumService.
        private static AccountNumService _instance;

        // Property to access the singleton instance.
        public static AccountNumService Instance => _instance ?? (_instance = new AccountNumService());

        // Private constructor to prevent external instantiation.
        private AccountNumService() { }
        
        // Private field to store the selected account number.
        private string _selectedAccountNumber;

        // Property to get or set the selected account number.
        public string SelectedAccountNumber
        {
            get => _selectedAccountNumber;
            set => _selectedAccountNumber = value;

        }

        // Private field to store the selected account number.
        private byte? _selectedAccountType;

        // Property to get or set the selected account number.
        public byte? SelectedAccountType
        {
            get => _selectedAccountType;
            set => _selectedAccountType = value;

        }
    }
}
