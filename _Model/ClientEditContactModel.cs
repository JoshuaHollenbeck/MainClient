using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using MainClient.Utilities;

namespace MainClient._Model
{
    public class ClientEditContactModel
    {
        public string NewCustEmail { get; set; }
        public string NewCustPhoneHome { get; set; }
        public string NewCustPhoneBusiness { get; set; }
        public string NewCustAddress { get; set; }
        public string NewCustAddress2 { get; set; }
        public string NewCustPostalCode { get; set; }
        public bool? AllAccts { get; set; }
        
        // Method to edit client contact.
        public void EditClientContact(
            string accountNumber,
            string NewCustEmail,
            string NewCustPhoneHome,
            string NewCustPhoneBusiness,
            string NewCustAddress,
            string NewCustAddress2,
            string NewCustPostalCode,
            bool? AllAccts
        )
        {
            // Connection string to the database.
            string connectionString = Connection.connectionString;

            // Name of the stored procedure.
            string storedProcedure = "[dbo].[Client_EditClientContact]";

            try
            {
                // Using statement to ensure proper disposal of resources.
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Adding parameters to the command.
                    command.Parameters.AddWithValue("@AcctNum", accountNumber);
                    command.Parameters.AddWithValue("@CustEmail", NewCustEmail);
                    command.Parameters.AddWithValue("@CustPhoneHome", NewCustPhoneHome);
                    command.Parameters.AddWithValue("@CustPhoneBusiness", NewCustPhoneBusiness);
                    command.Parameters.AddWithValue("@CustAddress", NewCustAddress);
                    command.Parameters.AddWithValue("@CustAddress2", NewCustAddress2);
                    command.Parameters.AddWithValue("@CustPostalCode", NewCustPostalCode);
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
        public string CustEmail { get; set; }
        public string CustPhoneHome { get; set; }
        public string CustPhoneBusiness { get; set; }
        public string CustAddress { get; set; }
        public string CustAddress2 { get; set; }
        public string CustCity { get; set; }
        public string CustState { get; set; }
        public string CustPostalCode { get; set; }

        // Method to retrieve account contact by account number.
        public static IEnumerable<ClientEditContactModel> GetClientContactByAcctNum(string acctNum)
        {
            // List to store accounts objects.
            var clientContacts = new List<ClientEditContactModel>();

            // Retrieving connection string from utilities.
            string connectionString = Connection.connectionString;

            // Stored procedure name.
            string storedProcedure = "[dbo].[Client_GetClientContactByAcctNum]";

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
                            var clientContact = new ClientEditContactModel
                            {
                                CustEmail = reader["Contact Email"] as string,
                                CustPhoneHome = reader["Contact Phone Home"] as string,
                                CustPhoneBusiness = reader["Contact Phone Business"] as string,
                                CustAddress = reader["Contact Address 1"] as string,
                                CustAddress2 = reader["Contact Address 2"] as string,
                                CustCity = reader["Contact City"] as string,
                                CustState = reader["Contact State"] as string,
                                CustPostalCode = reader["Contact Postal Code"] as string
                            };
                            clientContacts.Add(clientContact);
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
            return clientContacts;
        }
    }
}