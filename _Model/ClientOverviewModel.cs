using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using MainClient.Utilities;


namespace MainClient._Model
{
    // This class represents the model for client overview details.
    public class ClientOverviewModel
    {
        // Properties for client overview information.
        public DateTime? CustDateOfBirth { get; set; }
        public DateTime? CustClientSince { get; set; }
        public string CustFirstName { get; set; }
        public string CustMiddleName { get; set; }
        public string CustLastName { get; set; }
        public string CustSuffix { get; set; }
        public string CustPhoneHome { get; set; }
        public string CustPhoneBusiness { get; set; }
        public string CustEmail { get; set; }
        public string CustAddress { get; set; }
        public string CustAddress2 { get; set; }
        public string CustCity { get; set; }
        public string CustState { get; set; }
        public string CustZip { get; set; }
        public bool? CustEmploymentStatus { get; set; }
        public string CustEmployer { get; set; }
        public string CustOccupation { get; set; }
        public string CustIDType { get; set; }
        public string CustIDState { get; set; }
        public string CustIDNum { get; set; }
        public string CustIDExp { get; set; }
        public string CustMothersMaiden { get; set; }
        public bool? CustVoiceAuth { get; set; }
        public bool? CustDoNotCall { get; set; }
        public bool? CustShareAffiliates { get; set; }
        public string CustBranchCity { get; set; }
        public string CustBranchState { get; set; }
        public string CustBranchZip { get; set; }
        public string CustTaxId { get; set; }

        // Method to retrieve client details by account number.
        public static ClientOverviewModel GetClientDetailsByAcctNum(string acctNum)
        {
            ClientOverviewModel clientOverview = null;

            // Retrieving connection string from utilities.
            string connectionString = Connection.connectionString;

            // Stored procedure name.
            string storedProcedure = "[dbo].[Client_GetClientDetailsByAcctNum]";

            try
            {
                // Using statement for ensuring proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameter for account number.
                    command.Parameters.Add(new SqlParameter("@AcctNum", acctNum));

                    connection.Open();

                    // Executing the stored procedure and reading the results.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clientOverview = new ClientOverviewModel
                            {
                                CustDateOfBirth = reader["Date of Birth"] as DateTime?,
                                CustClientSince = reader["Client Since"] as DateTime?,
                                CustFirstName = reader["First Name"] as string,
                                CustMiddleName = reader["Middle Name"] as string,
                                CustLastName = reader["Last Name"] as string,
                                CustSuffix = reader["Suffix"] as string,
                                CustPhoneHome = reader["Home Phone"] as string,
                                CustPhoneBusiness = reader["Business Phone"] as string,
                                CustEmail = reader["Email"] as string,
                                CustAddress = reader["Address 1"] as string,
                                CustAddress2 = reader["Address 2"] as string,
                                CustCity = reader["City"] as string,
                                CustState = reader["State"] as string,
                                CustZip = reader["Zip"] as string,
                                CustEmploymentStatus = reader["Employment Status"] as bool?,
                                CustEmployer = reader["Employer"] as string,
                                CustOccupation = reader["Occupation"] as string,
                                CustIDType = reader["ID Type"] as string,
                                CustIDState = reader["ID State"] as string,
                                CustIDNum = reader["ID Number"] as string,
                                CustIDExp = reader["ID Expiration"] as string,
                                CustMothersMaiden = reader["Mothers Maiden"] as string,
                                CustVoiceAuth = reader["Voice Auth"] as bool?,
                                CustDoNotCall = reader["DNC"] as bool?,
                                CustShareAffiliates = reader["Share Affiliates"] as bool?,
                                CustBranchCity = reader["Branch City"] as string,
                                CustBranchState = reader["Branch State"] as string,
                                CustBranchZip = reader["Branch Zip"] as string,
                                CustTaxId = reader["Tax ID"] as string
                            };
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Log the exception, show a message, or handle it as necessary
                MessageBox.Show($"A SQL error occurred: {sqlEx.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                // This catches non-SQL exceptions
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return clientOverview;
        }

        // Properties for account overview information.
        public string AccountNumber { get; set; }
        public string AccountNickname { get; set; }
        public decimal? AccountBalance { get; set; }
        public string AccountPassword { get; set; }
        public string AccountType { get; set; }
        public bool? AccountStatus { get; set; }

        // Method to retrieve account details by account number.
        public static IEnumerable<ClientOverviewModel> GetClientAcctsByAcctNum(string acctNum)
        {
            // List to store accounts objects.
            var clientAccounts = new List<ClientOverviewModel>();

            // Retrieving connection string from utilities.
            string connectionString = Connection.connectionString;

            // Stored procedure name.
            string storedProcedure = "[dbo].[Client_GetClientAcctsByAcctNum]";

            try
            {
                // Using statement for ensuring proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameter for account number.
                    command.Parameters.Add(new SqlParameter("@AcctNum", acctNum));

                    connection.Open();

                    // Executing the stored procedure and reading the results.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var clientAccount = new ClientOverviewModel
                            {
                                AccountNumber = reader["Acct Number"] as string,
                                AccountNickname = reader["Acct Nickname"] as string,
                                AccountBalance = reader["Acct Balance"] as decimal?,
                                AccountPassword = reader["Acct Password"] as string,
                                AccountType = reader["Acct Type"] as string,
                                AccountStatus = reader["Acct Closed"] as bool?
                            };
                            clientAccounts.Add(clientAccount);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Log the exception, show a message, or handle it as necessary
                MessageBox.Show($"A SQL error occurred: {sqlEx.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                // This catches non-SQL exceptions
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return clientAccounts;
        }

        // Properties for account notes information.
        public string NoteAccountNumber { get; set; }
        public string Note { get; set; }
        public DateTime? NoteCreated { get; set; }
        public string NoteRepID { get; set; }

        // Method to retrieve account notes by account number.
        public static IEnumerable<ClientOverviewModel> GetClientNotesByAcctNum(string acctNum)
        {
            // List to store account notes objects.
            var clientNotes = new List<ClientOverviewModel>();

            // Retrieving connection string from utilities.
            string connectionString = Connection.connectionString;

            // Stored procedure name.
            string storedProcedure = "[dbo].[Client_GetClientNotesByAcctNum]";

            try
            {
                // Using statement for ensuring proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameter for account number.
                    command.Parameters.Add(new SqlParameter("@AcctNum", acctNum));

                    connection.Open();

                    // Executing the stored procedure and reading the results.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var clientNote = new ClientOverviewModel
                            {
                                NoteAccountNumber = reader["Acct Number"] as string,
                                Note = reader["Note"] as string,
                                NoteCreated = reader["Date"] as DateTime?,
                                NoteRepID = reader["Rep ID"] as string,
                            };
                            clientNotes.Add(clientNote);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Log the exception, show a message, or handle it as necessary
                MessageBox.Show($"A SQL error occurred: {sqlEx.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                // This catches non-SQL exceptions
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return clientNotes;
        }

        // Properties for account transaction information.
        public string TransactionAccountNumber { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string TransactionAction { get; set; }
        public string TradeSymbol { get; set; }
        public decimal? TradeQuantity { get; set; }
        public decimal? TransactionAmount { get; set; }
        public decimal? Fees { get; set; }
        public decimal? TradePrice { get; set; }
        public string TradeStatus { get; set; }
        public string TransactionRepID { get; set; }
        public string AchWireBankName { get; set; }
        public string AchWireRoutingNum { get; set; }
        public string AchWireAccountNumber { get; set; }
        public decimal? PreBalance { get; set; }
        public decimal? PostBalance { get; set; }

        // Method to retrieve account transactions by account number.
        public static IEnumerable<ClientOverviewModel> GetClientTransactionsByAcctNum(
            string acctNum
        )
        {
            // List to store account transaction objects.
            var clientTransactions = new List<ClientOverviewModel>();

            // Retrieving connection string from utilities.
            string connectionString = Connection.connectionString;

            // Stored procedure name.
            string storedProcedure = "[dbo].[Client_GetClientTransactionsByAcctNum]";

            try
            {
                // Using statement for ensuring proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameter for account number.
                    command.Parameters.Add(new SqlParameter("@AcctNum", acctNum));

                    connection.Open();

                    // Executing the stored procedure and reading the results.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var clientTransaction = new ClientOverviewModel
                            {
                                TransactionAccountNumber = reader["Acct Num"] as string,
                                TransactionDate = reader["Transaction Date"] as DateTime?,
                                TransactionAction = reader["Action"] as string,
                                TransactionAmount = reader["Amount"] as decimal?,
                                Fees = reader["Trade/Wire Fees"] as decimal?,
                                PreBalance = reader["Pre Balance"] as decimal?,
                                PostBalance = reader["Post Balance"] as decimal?,
                                TransactionRepID = reader["Rep ID"] as string,
                            };
                            clientTransactions.Add(clientTransaction);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Log the exception, show a message, or handle it as necessary
                MessageBox.Show($"A SQL error occurred: {sqlEx.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                // This catches non-SQL exceptions
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return clientTransactions;
        }
    }
}
