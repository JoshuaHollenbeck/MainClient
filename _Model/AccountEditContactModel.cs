using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using MainClient.Utilities;

namespace MainClient._Model
{
    public class AccountEditContactModel
    {
        public string NewContactName { get; set; }
        public string NewContactAddress1 { get; set; }
        public string NewContactAddress2 { get; set; }
        public string NewContactPostalCode { get; set; }
        public bool? AllAccts { get; set; }

        // Method to edit account contact.
        public void EditAcctContact(
            string accountNumber,
            string NewContactName,
            string NewContactAddress1,
            string NewContactAddress2,
            string NewContactPostalCode,
            bool? AllAccts
        )
        {
            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[Acct_EditAcctContact]";
            
            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameters to the command.
                    command.Parameters.AddWithValue("@AcctNum", accountNumber);
                    command.Parameters.AddWithValue("@ContactName", NewContactName);
                    command.Parameters.AddWithValue("@ContactAddress", NewContactAddress1);
                    command.Parameters.AddWithValue("@ContactAddress2", NewContactAddress2);
                    command.Parameters.AddWithValue("@ContactPostalCode", NewContactPostalCode);
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
        public string ContactName { get; set; }
        public string ContactAddress1 { get; set; }
        public string ContactAddress2 { get; set; }
        public string ContactCity { get; set; }
        public string ContactState { get; set; }
        public string ContactPostalCode { get; set; }

        // Method to retrieve account contact by account number.
        public static IEnumerable<AccountEditContactModel> GetAcctContactByAcctNum(string acctNum)
        {
            // List to store accounts objects.
            var acctContacts = new List<AccountEditContactModel>();

            // Retrieving connection string from utilities.
            string connectionString = Connection.connectionString;

            // Stored procedure name.
            string storedProcedure = "[dbo].[Acct_GetAcctContactByAcctNum]";

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
                            var acctContact = new AccountEditContactModel
                            {
                                ContactName = reader["Contact Name"] as string,
                                ContactAddress1 = reader["Contact Address 1"] as string,
                                ContactAddress2 = reader["Contact Address 2"] as string,
                                ContactCity = reader["Contact City"] as string,
                                ContactState = reader["Contact State"] as string,
                                ContactPostalCode = reader["Contact Postal Code"] as string
                            };
                            acctContacts.Add(acctContact);
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

            return acctContacts;
        }
    }
}