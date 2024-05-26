namespace MainClient.Services
{
    // This class provides methods for setting client id.
    public class ClientIdService
    {
        // Singleton instance of ClientIdService.
        private static ClientIdService _instance;

        // Property to access the singleton instance.
        public static ClientIdService Instance => _instance ?? (_instance = new ClientIdService());

        // Private constructor to prevent external instantiation.
        private ClientIdService() { }
        
        // Private field to store the new customer id.
        private long _selectedNewCustId;

        // Property to get or set the new customer id.
        public long SelectedNewCustId
        {
            get => _selectedNewCustId;
            set => _selectedNewCustId = value;

        }

        // Private field to store the selected customer id.
        private string _selectedCustId;

        // Property to get or set the selected customer id.
        public string SelectedCustId
        {
            get => _selectedCustId;
            set => _selectedCustId = value;

        }

        // Private field to store the selected customer id.
        private string _selectedJointCustId;

        // Property to get or set the selected customer id.
        public string SelectedJointCustId
        {
            get => _selectedJointCustId;
            set => _selectedJointCustId = value;

        }
    }
}