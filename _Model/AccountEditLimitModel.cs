using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using MainClient.Utilities;

namespace MainClient._Model
{
    public class AccountEditLimitModel
    {
        public int?  NewAtmLimit { get; set; }
        public int?  NewAchLimit { get; set; }
        public int?  NewWireLimit { get; set; }
        public bool? AllAccts { get; set; }
        
        // Method to edit account limit.
        public void EditAcctLimit(
            string accountNumber,
            int?  NewAtmLimit,
            int?  NewAchLimit,
            int?  NewWireLimit,
            bool? AllAccts
        )
        {
            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[Acct_EditAcctLimit]";

            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameters to the command.
                    command.Parameters.AddWithValue("@AcctNum", accountNumber);
                    command.Parameters.AddWithValue("@AtmLimit", NewAtmLimit);
                    command.Parameters.AddWithValue("@AchLimit", NewAchLimit);
                    command.Parameters.AddWithValue("@WireLimit", NewWireLimit);
                    command.Parameters.AddWithValue("@AllAccts", AllAccts);

                    // Opening connection and executing the command.
                    connection.Open();
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

        // Properties for account overview information.
        public int?  AtmLimit { get; set; }
        public int?  AchLimit { get; set; }
        public int?  WireLimit { get; set; }

        // Method to retrieve account contact by account number.
        public static IEnumerable<AccountEditLimitModel> GetAcctLimitByAcctNum(string acctNum)
        {
            // List to store accounts objects.
            var acctLimits = new List<AccountEditLimitModel>();

            // Retrieving connection string from utilities.
            string connectionString = Connection.connectionString;

            // Stored procedure name.
            string storedProcedure = "[dbo].[Acct_GetAcctLimitByAcctNum]";

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
                            var acctLimit = new AccountEditLimitModel
                            {
                                AchLimit = reader["ACH Limit"] as int?,
                                AtmLimit = reader["ATM Limit"] as int?,
                                WireLimit = reader["Wire Limit"] as int?
                            };
                            acctLimits.Add(acctLimit);
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
            return acctLimits;
        }
    }
}