using System;

namespace MainClient.Services
{
    // Service class for managing the representative ID and related permissions.
    public class RepIdService
    {
        // Singleton instance of RepIdService.
        private static readonly RepIdService _instance = new RepIdService();

        // Property to access the singleton instance.
        public static RepIdService Instance => _instance;

        // Private constructor to prevent external instantiation.
        private RepIdService() { }

        // Representative ID.
        public string RepId { get; private set; }

        // Permission to perform trading.
        public bool Trading { get; private set; }

        // Permission to move money.
        public bool MoveMoney { get; private set; }

        // Permission for view-only access.
        public bool ViewOnly { get; private set; }

        // Method to set permissions for the representative ID.
        public void SetPermissions(string repId, bool trading, bool moveMoney, bool viewOnly)
        {
            RepId = repId;
            Trading = trading;
            MoveMoney = moveMoney;
            ViewOnly = viewOnly;
        }
    }
}
