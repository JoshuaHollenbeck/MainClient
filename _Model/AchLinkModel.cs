using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using MainClient.Utilities;

namespace MainClient._Model
{
    // This class represents the model for handling ACH (Automated Clearing House) transactions.
    public class AchLinkModel
    {
        // Properties to store ACH details.
        public string AchBankType { get; set; }
        public string AchNickname { get; set; }
        public string AchBankName { get; set; }
        public string AchRoutingNumber { get; set; }
        public string AchAccountNumber { get; set; }
        public string AchAccountType { get; set; }
        public DateTime? AchAddedDate { get; set; }
        public DateTime? AchRemovedDate { get; set; }
        public string AchRepID { get; set; }

        // Method to retrieve ACH links associated with an account.
        public static IEnumerable<AchLinkModel> GetAcctAchLinksByAcctNum(string acctNum)
        {
            // List to store ACH link objects.
            var accountAchs = new List<AchLinkModel>();

            // Retrieving connection string from utilities.
            string connectionString = Connection.connectionString;

            // Stored procedure name.
            string storedProcedure = "[dbo].[Acct_GetAcctAchLinksByAcctNum]";

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
                            var accountAch = new AchLinkModel
                            {
                                AchNickname = reader["Nickname"] as string,
                                AchBankName = reader["Bank Name"] as string,
                                AchRoutingNumber = reader["Routing Num"] as string,
                                AchAccountNumber = reader["Acct Num"] as string,
                                AchAccountType = reader["Acct Type"] as string,
                                AchAddedDate = reader["Added On"] as DateTime?,
                                AchRemovedDate = reader["Removed On"] as DateTime?,
                                AchRepID = reader["Rep ID"] as string
                            };
                            accountAchs.Add(accountAch);
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
            return accountAchs;
        }

        // Method to insert a new ACH setup for an account.
        public static void InsertAcctAchSetupByAcctNum(
            string acctNum,
            string newAchRoutingNum,
            string newAchAccountNum,
            string newAchAccountType,
            string newAchNickname,
            string repId
        )
        {
            // Retrieving connection string from utilities.
            string connectionString = Connection.connectionString;

            // Stored procedure name.
            string storedProcedure = "[dbo].[Acct_InsertAcctAchSetupByAcctNum]";

            try
            {
                // Using statement for ensuring proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameter for account number.
                    command.Parameters.Add(new SqlParameter("@AcctNum", acctNum));
                    command.Parameters.Add(new SqlParameter("@AchRoutingNum", newAchRoutingNum));
                    command.Parameters.Add(new SqlParameter("@AchAcctNum", newAchAccountNum));
                    command.Parameters.Add(new SqlParameter("@AchAcctType", newAchAccountType));
                    command.Parameters.Add(new SqlParameter("@Nickname", newAchNickname));
                    command.Parameters.Add(new SqlParameter("@RepId", repId));

                    connection.Open();
                    
                    // Executing the non-query command to insert the notes into the database.
                    command.ExecuteNonQuery();
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
        }

        // Properties to store transaction details.
        public DateTime? TransactionDate { get; set; }
        public decimal? TransactionAmt { get; set; }
        public string TransactionBankName { get; set; }
        public string TransactionRoutingNumber { get; set; }
        public string TransactionAccountNumber { get; set; }
        public string TransactionRepID { get; set; }

        // Method to retrieve ACH transactions associated with an account.
        public static IEnumerable<AchLinkModel> GetAcctTransactionsAchByAcctNum(string acctNum)
        {
            // List to store ACH transactions objects.
            var accountAchTransactions = new List<AchLinkModel>();

            // Retrieving connection string from utilities.
            string connectionString = Connection.connectionString;

            // Stored procedure name.
            string storedProcedure = "[dbo].[Acct_GetAcctTransactionsAchByAcctNum]";

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
                            var accountAchTransaction = new AchLinkModel
                            {
                                TransactionDate = reader["Transaction Date"] as DateTime?,
                                TransactionAmt = reader["Transaction Amount"] as decimal?,
                                TransactionBankName = reader["Bank Name"] as string,
                                TransactionRoutingNumber = reader["Bank Routing"] as string,
                                TransactionAccountNumber = reader["Acct Num"] as string,
                                TransactionRepID = reader["Rep ID"] as string
                            };
                            accountAchTransactions.Add(accountAchTransaction);
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
            return accountAchTransactions;
        }
    }
}