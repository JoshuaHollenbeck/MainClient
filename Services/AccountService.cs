namespace MainClient.Services
{
    public class AccountService
    {   
        private static AccountService _instance;
        public static AccountService Instance => _instance ?? (_instance = new AccountService());

        private string _selectedAccountNumber;

        private AccountService() { }

        public string SelectedAccountNumber
        {
            get => _selectedAccountNumber;
            set => _selectedAccountNumber = value;
        }
    }
}